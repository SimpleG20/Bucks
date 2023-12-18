using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SearchManager : MonoBehaviour
{
    [SerializeField] private Button _mainPageSearchInput;
    [SerializeField] private TMP_InputField _searchInput;

    [SerializeField] private Transform _searchItemsParent;
    [SerializeField] private SearchItemPrefab _searchItemPrefab;

    private void Awake()
    {
        MainPageManager.onUserReady += HandleStart;
    }

    private void HandleStart()
    {
        _mainPageSearchInput.interactable = true;

        _mainPageSearchInput.onClick.AddListener(() =>
        {
            PagesManager.Instance.SearchScene();

            _searchInput.interactable = true;
            _searchInput.Select();
        });

        _searchInput.onEndEdit.AddListener((value) =>
        {
            SearchFor(value);
        });
    }

    private void SearchFor(string searchText)
    {
        var newItem = Instantiate(_searchItemPrefab, _searchItemsParent);

        Expense expenseItem = User.Instance.ExpenseList.First(t => t.Name.ToLower().Contains(searchText.ToLower()));
        if (expenseItem == null)
        {
            Additional additionalItem = User.Instance.AdditionalList.First(t => t.Name.ToLower() == searchText.ToLower());
            if (additionalItem == null)
            {
                Wish wishItem = User.Instance.WishList.First(t => t.Name.ToLower() == searchText.ToLower());
                if (wishItem == null)
                {
                    Destroy(newItem.gameObject);
                    return;
                }

                newItem.Initialize(wishItem);
                return;
            }
            newItem.Initialize(additionalItem);
            return;
        }
        newItem.Initialize(expenseItem);
    }
}
