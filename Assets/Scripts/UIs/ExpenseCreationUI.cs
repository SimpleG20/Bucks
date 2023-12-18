using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpenseCreationUI : BaseCreationUi
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _priceInput;
    [SerializeField] private TMP_InputField _dateInput;
    [SerializeField] private TMP_InputField _installmentInput;
    [SerializeField] private Toggle _showTotalPriceTg;
    [SerializeField] private Toggle _subscriptionTg;
    [SerializeField] private Toggle _creditedTg;
    

    private bool _isMissingName = true;
    private bool _isMissingPrice = true;

    protected override void Awake()
    {
        base.Awake();

        _dateInput.onValidateInput += delegate (string input, int charIndex, char addedChar) {
            string newText = _dateInput.text.Remove(charIndex, 1);
            newText = newText.Insert(charIndex, addedChar.ToString());
            if (System.Text.RegularExpressions.Regex.IsMatch(newText, regexDatePattern))
            {
                return addedChar;
            }
            return '\0';
        };
    }

   
    protected override void InitializeInputs()
    {
        _nameInput.onEndEdit.AddListener((value) =>
        {
            if (value != "")
            {
                NameItem = value;
                _isMissingName = false;
            }
            else _isMissingName = true;
        });

        _priceInput.onEndEdit.AddListener((value) =>
        {
            float number = _priceInput.SetTextToFloat();
            if (number > 0)
            {
                PriceItem = number;
                _isMissingPrice = false;
                _priceInput.text = number.ToMoney();
            }
            else
            {
                _isMissingPrice = true;
                _priceInput.text = string.Empty;
            }
        });

        _dateInput.onEndEdit.AddListener((value) =>
        {
            if (value != "")
            {
                var temp = value.Split("/");

                int.TryParse(temp[0], out DayItem);
                int.TryParse(temp[1], out MonthItem);
                int.TryParse(temp[2], out YearItem);
            }
        });

        _installmentInput.onEndEdit.AddListener((value) =>
        {
            float number = _installmentInput.SetTextToFloat();

            if (number <= 0)
            {
                InstallmentItem = 1;
                _showTotalPriceTg.isOn = true;
            }
            else InstallmentItem = (int)number;

            _priceInput.text = InstallmentItem.ToString("00");
        });

        _showTotalPriceTg.onValueChanged.AddListener((value) => ShowTotalItem = value);

        _subscriptionTg.onValueChanged.AddListener((value) => SubscriptionItem = value);

        _creditedTg.onValueChanged.AddListener((value) => CreditedItem = value);
    }

    protected override void ResetInputs()
    {
        _nameInput.text = "";
        NameItem = "";

        _priceInput.text = "";
        PriceItem = 0;

        _dateInput.text = DateTime.Now.Date.ToString();
        DayItem = DateTime.Now.Day;
        MonthItem = DateTime.Now.Month;
        YearItem = DateTime.Now.Year;

        _installmentInput.text = "01";
        InstallmentItem = 1;

        _showTotalPriceTg.isOn = false;
        ShowTotalItem = false;

        _subscriptionTg.isOn = false;
        SubscriptionItem = false;

        _creditedTg.isOn = true;
        CreditedItem = true;

        _isMissingPrice = true;
        _isMissingName = true;
    }

    public override void Setup<T>(T item)
    {
        Expense expense = item as Expense;

        _nameInput.text = expense.Name;
        _priceInput.text = expense.Value.ToMoney();
        _dateInput.text = new DateTime(expense.Year, expense.Month, expense.Year).ToShortDateString();
        _installmentInput.text = expense.AmountParceled.ToString("00");

        _showTotalPriceTg.isOn = expense.ShowTotal;
        _subscriptionTg.isOn = expense.Subscription;
        _creditedTg.isOn = expense.Credited;
    }

    protected override void Confirm()
    {
        if (IsMissingInformation())
        {
            print("Faltando Informação");
            return;
        }

        Expense newExpense = new Expense(NameItem, PriceItem, InstallmentItem, 
            DayItem, MonthItem, YearItem, 
            ShowTotalItem, SubscriptionItem, CreditedItem);

        User.Instance.AddItemInExpenses(newExpense);
        ResetInputs();
    }

    protected override bool IsMissingInformation() => _isMissingName || _isMissingPrice;

    protected override void LeaveScenario()
    {
        _nameInput.onEndEdit.RemoveAllListeners();
        _dateInput.onEndEdit.RemoveAllListeners();
        _priceInput.onEndEdit.RemoveAllListeners();
        _installmentInput.onEndEdit.RemoveAllListeners();

        _creditedTg.onValueChanged.RemoveAllListeners();
        _subscriptionTg.onValueChanged.RemoveAllListeners();
        _showTotalPriceTg.onValueChanged.RemoveAllListeners();

        base.LeaveScenario();
    }
}
