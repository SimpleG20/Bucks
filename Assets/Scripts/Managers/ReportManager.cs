using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportManager : MonoBehaviour
{
    [SerializeField] private Button _nextMonthBt;
    [SerializeField] private Button _prevMonthBt;

    [SerializeField] private ReportItem _reportItemPrefab;
    [SerializeField] private List<ReportItem> _reportItemAvailables;
    [SerializeField] private CanvasGroup _detailsWindow;

    [SerializeField] private Transform _reportItemsParent;

    private int _reportItemsCount;
    private int _currentPage = 0;
    private int _currentMonth = 1;
    private int _currentYear = 2023;

    private List<Item> itemList = new List<Item>();


    private void Start()
    {
        _currentMonth = DateTime.Now.Month;
        _currentYear = DateTime.Now.Year;

        ResetReportItems();
    }

    public void InitializeReportSecenario()
    {
        ResetReportItems();
        GetItems();
    }

    private void GetItems()
    {
        foreach(var item in User.Instance.ExpenseList)
        {
            itemList.Add(item);
        }
        foreach(var item in User.Instance.AdditionalList)
        {
            itemList.Add(item);
        }

        for(int i = 0; i < itemList.Count; i++)
        {
            var random = itemList.PickRandom();

            if (CheckItemAccordingDate(random))
            {
                AddItem(random.Name, random.Value, random.ItemType == Item.TypeItem.Expense);
            }
        }
    }

    public void AddItem(string name, float value, bool debt)
    {
        if (_reportItemsCount == _reportItemAvailables.Count)
        {
            var newReportItem = Instantiate(_reportItemPrefab, _reportItemsParent);

            newReportItem.ShowInfo(name, value.ToMoney(), debt);
            _reportItemAvailables[_reportItemsCount] = newReportItem;
            _reportItemsCount++;

            return;
        }

        _reportItemAvailables[_reportItemsCount].ShowInfo(name, value.ToMoney(), debt);
        _reportItemsCount++;
    }

    private bool CheckItemAccordingDate(Item item)
    {
        foreach(ItemTimeSpam time in item.TimeSpams)
        {
            if (time.Year == _currentYear)
            {
                if (time.Months.Contains(_currentMonth)) return true;
            }
        }

        return false;
    }

    public void ResetReportItems()
    {
        _reportItemsCount = 0;

        foreach(var reportItem in _reportItemAvailables)
        {
            reportItem.gameObject.SetActive(false);
        }
    }

    public void ShowMoreDetails()
    {
        _detailsWindow.FadeIn(0.5f);
    }

    public void HideMoreDetails()
    {
        _detailsWindow.FadeOut(0.25f);
    }

    private void NextMonth()
    {
        bool advancedYear = false;

        _currentMonth++;
        if (_currentMonth == 13)
        {
            _currentMonth = 1;
            _currentYear++;
            advancedYear = true;
        }

        int none = 0;

        for(int i = 0; i < itemList.Count; i++)
        {
            var random = itemList.PickRandom();

            if (CheckItemAccordingDate(random))
            {
                AddItem(random.Name, random.Value, random.ItemType == Item.TypeItem.Expense);
            }
            else none++;
        }

        if (none == itemList.Count)
        {
            _nextMonthBt.gameObject.SetActive(false);

            if (advancedYear)
            {
                _currentYear--;
                _currentMonth = 12;
            }
            else _currentMonth--;
            return;
        }

        _currentPage++;
    }

    private void PrevMonth()
    {
        if (_currentPage == 0) return;

        _currentMonth--;
        if (_currentMonth == 0)
        {
            _currentMonth = 12;
            _currentYear--;
        }

        for (int i = 0; i < itemList.Count; i++)
        {
            var random = itemList.PickRandom();

            if (CheckItemAccordingDate(random))
            {
                AddItem(random.Name, random.Value, random.ItemType == Item.TypeItem.Expense);
            }
        }

        _currentPage--;
    }
}
