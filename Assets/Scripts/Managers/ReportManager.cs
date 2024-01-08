using System;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using static UnityEditor.Progress;

public class ReportManager : MonoBehaviour
{
    [SerializeField] private Button _nextMonthBt;
    [SerializeField] private Button _prevMonthBt;

    [SerializeField] private ReportItem _reportItemPrefab;
    [SerializeField] private List<ReportItem> _reportItemAvailables;
    [SerializeField] private CanvasGroup _detailsWindow;

    [SerializeField] private Transform _reportItemsParent;

    [SerializeField] private TMP_Text _monthTx;

    private int _reportItemsCount;
    private int _currentPage = 0;
    private int _currentMonth = 1;
    private int _currentYear = 2023;
    private int _maxPages;

    private List<Item> itemListMonthly = new List<Item>();
    private List<Item> itemListGeneral = new List<Item>();

    private void Start()
    {
        _nextMonthBt.onClick.AddListener(NextMonth);
        _prevMonthBt.onClick.AddListener(PrevMonth);

        InitializeReportSecenario();
    }

    public void InitializeReportSecenario()
    {
        foreach (var item in User.Instance.ExpenseList) itemListGeneral.Add(item);
        foreach (var item in User.Instance.AdditionalList) itemListGeneral.Add(item);

        _currentMonth = DateTime.Now.Month;
        _currentYear = DateTime.Now.Year;
        
        _currentPage = 0;

        UpdateMonthTx();
        UpdateMonthButtons();
        
        GetItems();
        GetMaxPagesCountPossible();

        ResetReportItems();
    }

    private void GetMaxPagesCountPossible()
    {
        int maxMonth = DateTime.Now.Month;
        int maxYear = DateTime.Now.Year;

        foreach (Item item in itemListGeneral)
        {
            var years = item.TimeSpams;
            var yearsCount = years.Length - 1;
            var lastYear = years[yearsCount];
            var monthsCount = lastYear.Months.Count - 1;
            var lastMonth = lastYear.Months[monthsCount];

            if (years[yearsCount].Year > maxYear) maxMonth = lastMonth;
            else
            {
                if (lastMonth > maxMonth) maxMonth = lastMonth;
            }
        }

        if (_currentYear == maxYear)
        {
            _maxPages = maxMonth - _currentMonth;
            return;
        }

        _maxPages = 12 * (maxYear - _currentYear) + (maxMonth - _currentMonth);
    }

    private void GetItems()
    {
        foreach(Item item in itemListGeneral)
        {
            if (CheckItemAccordingDate(item))
            {
                AddItemPrefab(item.Name, item.Value, item.ItemType == Item.TypeItem.Expense);
                itemListMonthly.Add(item);
            }
        }
    }

    private void AddItemPrefab(string name, float value, bool debt)
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

    private void ResetReportItems()
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
        if (_currentPage == _maxPages) return;

        _currentMonth++;
        if (_currentMonth == 13)
        {
            _currentMonth = 1;
            _currentYear++;
        }

        UpdateMonthTx();
        ResetReportItems();

        GetItems();

        _currentPage++;
        UpdateMonthButtons();
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

        UpdateMonthTx();
        ResetReportItems();

        GetItems();

        _currentPage--;
        UpdateMonthButtons();
    }

    private void UpdateMonthTx()
    {
        var date = new DateTime(_currentYear, _currentMonth, 1);
        string text = $"{date.ToString("MMMM", CultureInfo.CurrentCulture)} - {date.Year}";
        text = text[0].ToString().ToUpper() + text.Substring(1);
        _monthTx.text = text;
    }

    private void UpdateMonthButtons()
    {
        if (_currentPage == 0)
        {
            _nextMonthBt.gameObject.SetActive(true);
            _prevMonthBt.gameObject.SetActive(false);
        }
        else if (_currentPage > 0 && _currentPage < _maxPages)
        {
            _nextMonthBt.gameObject.SetActive(true);
            _prevMonthBt.gameObject.SetActive(true);
        }
        else
        {
            _nextMonthBt.gameObject.SetActive(false);
            _prevMonthBt.gameObject.SetActive(true);
        }
    }
}
