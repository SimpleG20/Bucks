using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ExpenseCreationUI : BaseCreationUi
{
    [SerializeField] private TMP_InputField _installmentInput;
    [SerializeField] private Toggle _showTotalPriceTg;
    [SerializeField] private Toggle _subscriptionTg;
    [SerializeField] private Toggle _creditedTg;

   
    protected override void InitializeInputs()
    {
        base.InitializeInputs();

        _installmentInput.onEndEdit.AddListener((value) =>
        {
            float number = _installmentInput.SetTextToFloat();

            if (number <= 0)
            {
                InstallmentItem = 1;
                _showTotalPriceTg.isOn = true;
            }
            else InstallmentItem = (int)number;

            _installmentInput.text = InstallmentItem.ToString("00");
        });

        _showTotalPriceTg.onValueChanged.AddListener((value) => ShowTotalItem = value);

        _subscriptionTg.onValueChanged.AddListener((value) => SubscriptionItem = value);

        _creditedTg.onValueChanged.AddListener((value) => CreditedItem = value);
    }

    protected override void ResetInputs()
    {
        base.ResetInputs();

        _installmentInput.text = "01";
        InstallmentItem = 1;

        _showTotalPriceTg.isOn = false;
        ShowTotalItem = false;

        _subscriptionTg.isOn = false;
        SubscriptionItem = false;

        _creditedTg.isOn = true;
        CreditedItem = true;
    }

    public override void Setup(Item item)
    {
        base.Setup(item);

        Expense expense = item as Expense;
        SubscriptionItem = expense.Subscription;
        CreditedItem = expense.Credited;

        _installmentInput.text = InstallmentItem.ToString("00");
        _showTotalPriceTg.isOn = ShowTotalItem;
        _subscriptionTg.isOn = SubscriptionItem;
        _creditedTg.isOn = CreditedItem;
    }

    protected override void Confirm()
    {
        if (IsMissingInformation())
        {
            print("Faltando Informação");
            return;
        }

        if (EditMode)
        {
            User.Instance.RemoveExpenseByID(ID);
        }

        CreditedItem = _creditedTg.isOn;
        ShowTotalItem = _showTotalPriceTg.isOn;
        SubscriptionItem = _subscriptionTg.isOn;

        Expense newExpense = new Expense(NameItem, PriceItem, InstallmentItem, 
            DayItem, MonthItem, YearItem, 
            ShowTotalItem, SubscriptionItem, CreditedItem);

        ID = Guid.NewGuid();
        newExpense.SetID(ID);

        User.Instance.AddItemInExpenses(newExpense);

        if (!EditMode) ResetInputs();
    }

    protected override bool IsMissingInformation() 
    {
        if (NameItem == "") IsMissingName = true;
        else IsMissingName = false;

        if (PriceItem <= 0) IsMissingPrice = true;
        else IsMissingPrice = false;

        return IsMissingName || IsMissingPrice;
    }

    protected override void LeaveScenario()
    {
        _installmentInput.onEndEdit.RemoveAllListeners();
        _creditedTg.onValueChanged.RemoveAllListeners();
        _subscriptionTg.onValueChanged.RemoveAllListeners();
        _showTotalPriceTg.onValueChanged.RemoveAllListeners();

        base.LeaveScenario();
    }
}
