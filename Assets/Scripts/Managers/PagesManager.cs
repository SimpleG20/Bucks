using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesManager : MonoBehaviour
{
    public static PagesManager _instance;
    public static PagesManager Instance { 
        get
        {
            if (_instance != null) return _instance;
            else return null;
        }
        set
        {
            if (_instance == null) _instance = value;
        }
    }

    public enum Pages { Home, Add, Report, Config, Search }
    [SerializeField] private Pages _currentPage;

    [SerializeField] private CanvasGroup _homePage;
    [SerializeField] private CanvasGroup _addPage;
    [SerializeField] private CanvasGroup _reportPage;
    [SerializeField] private CanvasGroup _searchPage;
    [SerializeField] private CanvasGroup _configPage;

    [SerializeField] private MenuManager _menuSlider;

    private Dictionary<Pages, CanvasGroup> _scenesDict = new Dictionary<Pages, CanvasGroup>();

    private void Awake()
    {
        Instance = this;
        InitializePageDictionary();
    }

    public void GoToHomeScene()
    {
        LoadPage(Pages.Home);
    }

    public void GoToAddScene()
    {
        LoadPage(Pages.Add);
    }

    public void GoToReportScene()
    {
        LoadPage(Pages.Report);
    }

    public void GoToConfigScene()
    {
        LoadPage(Pages.Config);
    }

    public void GoToSearchScene()
    {
        LoadPage(Pages.Search);
    }

    public bool IsInPage(Pages page)
    {
        return _currentPage == page;
    }

    private void InitializePageDictionary()
    {
        _scenesDict.Add(Pages.Home, _homePage);
        _scenesDict.Add(Pages.Add, _addPage);
        _scenesDict.Add(Pages.Report, _reportPage);
        _scenesDict.Add(Pages.Config, _configPage);
        _scenesDict.Add(Pages.Search, _searchPage);
    }

    private void LoadPage(Pages page)
    {
        _scenesDict[_currentPage].alpha = 0;
        _scenesDict[_currentPage].blocksRaycasts = false;
        _scenesDict[_currentPage].interactable = false;

        _currentPage = page;
        _scenesDict[_currentPage].alpha = 1;
        _scenesDict[_currentPage].blocksRaycasts = true;
        _scenesDict[_currentPage].interactable = true;
        _menuSlider.SetSliderTo(page);
    }
}
