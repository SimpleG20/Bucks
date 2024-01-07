using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SearchManager : MonoBehaviour
{
    [SerializeField] private Button _mainPageSearchInput;
    [SerializeField] private TMP_InputField _searchInput;

    [SerializeField] private Transform _searchItemsParent;
    [SerializeField] private SearchItemPrefab _searchItemPrefab;

    [SerializeField] private SearchItemPrefab[] _searchItemsPreloaded;

    private Queue<SearchItemPrefab> _searchItemsPreloadedQueue;

    private void Awake()
    {
        MainPageManager.onUserReady += HandleStart;
    }

    private void HandleStart()
    {
        _mainPageSearchInput.interactable = true;

        _mainPageSearchInput.onClick.AddListener(() =>
        {
            PagesManager.Instance.GoToSearchScene();

            _searchInput.interactable = true;
            _searchInput.Select();
        });

        _searchInput.onEndEdit.AddListener((value) =>
        {
            if (value == null) return;
            SearchFor(value);
        });
    }

    private void SearchFor(string searchText)
    {
        CleanSearch();

        StartQueue();
        int maxPreloaded = _searchItemsPreloadedQueue.Count;

        var expenseItems = User.Instance.ExpenseList.Where(t => t.Name.ToLower().Contains(searchText.ToLower())).ToArray();
        var additionalItems = User.Instance.AdditionalList.Where(t => t.Name.ToLower().Contains(searchText.ToLower())).ToArray();
        var wishItems = User.Instance.WishList.Where(t => t.Name.ToLower().Contains(searchText.ToLower())).ToArray();

        foreach (var item in expenseItems)
        {
            SearchItemPrefab prefab;
            if (maxPreloaded > 0)
            {
                prefab = _searchItemsPreloadedQueue.Dequeue();
                prefab.gameObject.SetActive(true);
                maxPreloaded--;
            }
            else prefab = Instantiate(_searchItemPrefab, _searchItemsParent);

            prefab.Initialize(item);
        }

        foreach (var item in additionalItems)
        {
            SearchItemPrefab prefab;
            if (maxPreloaded > 0)
            {
                prefab = _searchItemsPreloadedQueue.Dequeue();
                prefab.gameObject.SetActive(true);
                maxPreloaded--;
            }
            else prefab = Instantiate(_searchItemPrefab, _searchItemsParent);

            prefab.Initialize(item);
        }

        foreach (var item in wishItems)
        {
            SearchItemPrefab prefab;
            if (maxPreloaded > 0)
            {
                prefab = _searchItemsPreloadedQueue.Dequeue();
                prefab.gameObject.SetActive(true);
                maxPreloaded--;
            }
            else prefab = Instantiate(_searchItemPrefab, _searchItemsParent);

            prefab.Initialize(item);
        }
    }

    private void StartQueue()
    {
        _searchItemsPreloadedQueue = new Queue<SearchItemPrefab>();

        foreach(SearchItemPrefab item in _searchItemsPreloaded)
        {
            _searchItemsPreloadedQueue.Enqueue(item);
            item.gameObject.SetActive(false);
        }
    }

    private void CleanSearch()
    {
        foreach(Transform child in _searchItemsParent)
        {
            if (_searchItemsPreloaded.Contains(child.GetComponent<SearchItemPrefab>()))
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }
}
