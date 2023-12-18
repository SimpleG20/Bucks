using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class MenuManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button _homeBt;
    [SerializeField] private Button _addBt;
    [SerializeField] private Button _reportBt;
    [SerializeField] private Button _searchBt;
    [SerializeField] private Button _configBt;


    private bool selectedToScroll;
    private ScrollRect _scroll;

    private CancellationTokenSource _token;


    private void Start()
    {
        _scroll = GetComponent<ScrollRect>();
        _token = new CancellationTokenSource();

        InitializeButtons();
    }

    private void InitializeButtons()
    {
        _homeBt.onClick.AddListener(() =>
        {
            PagesManager.Instance.HomeScene();
        });

        _addBt.onClick.AddListener(() =>
        {
            PagesManager.Instance.AddScene();
        });

        _reportBt.onClick.AddListener(() =>
        {
            PagesManager.Instance.ReportScene();
        });

        _searchBt.onClick.AddListener(() =>
        {
            PagesManager.Instance.SearchScene();
        });

        _configBt.onClick.AddListener(() =>
        {
            PagesManager.Instance.ConfigScene();
        });
    }

    public async void OnPointerUp(PointerEventData eventData)
    {
        selectedToScroll = false;

        await Task.Delay(1500, _token.Token);

        if (selectedToScroll)
        {
            _token = new CancellationTokenSource();
            selectedToScroll = false;
            return;
        }

        LeanTween.value(_scroll.horizontalNormalizedPosition, 0.5f, 0.2f).setOnUpdate((value) =>
        {
            _scroll.horizontalNormalizedPosition = value;
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        selectedToScroll = true;
    }

    public void SetSliderTo(PagesManager.Pages page)
    {
        LeanTween.cancel(gameObject);

        switch(page)
        {
            case PagesManager.Pages.Home:
            case PagesManager.Pages.Add:
            case PagesManager.Pages.Report:
                _scroll.horizontalNormalizedPosition = 0.5f;
                break;
            case PagesManager.Pages.Config:
                _scroll.horizontalNormalizedPosition = 0;
                break;
            case PagesManager.Pages.Search:
                _scroll.horizontalNormalizedPosition = 1;
                break;
        }
    }

    public void GoSliderBackToInitialPosition()
    {
        LeanTween.value(_scroll.horizontalNormalizedPosition, 0.5f, 0.2f).setOnUpdate((value) =>
        {
            _scroll.horizontalNormalizedPosition = value;
        });
    }
}
