using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public abstract class BaseCreationUi : MonoBehaviour
{
    [SerializeField] protected CanvasGroup scenario;

    [Space(10)]
    [SerializeField] protected TMP_InputField NameInput;
    [SerializeField] protected TMP_InputField PriceInput;
    [SerializeField] protected TMP_InputField DateInput;

    [Space(10)]
    [SerializeField] protected Button ConfirmBt;
    [SerializeField] protected Button LeaveBt;

    [SerializeField] protected AddManager _addManager;

    protected Guid ID;
    protected string NameItem;
    protected float PriceItem;
    protected int DayItem;
    protected int MonthItem;
    protected int YearItem;
    protected int InstallmentItem;
    protected bool ShowTotalItem;
    protected bool SubscriptionItem;
    protected bool CreditedItem;

    protected bool EditMode;
    protected bool IsMissingName = true;
    protected bool IsMissingPrice = true;


    protected virtual void Awake()
    {
        ConfirmBt.onClick.AddListener(Confirm);
        LeaveBt.onClick.AddListener(LeaveScenario);
    }


    protected virtual void InitializeInputs()
    {
        NameInput.onEndEdit.AddListener((value) =>
        {
            if (value != "") NameItem = value;
            else NameItem = "";
        });

        PriceInput.onEndEdit.AddListener((value) =>
        {
            float number = PriceInput.SetTextToFloat();
            if (number > 0)
            {
                PriceItem = number;
                PriceInput.text = number.ToMoney();
            }
            else
            {
                PriceItem = 0;
                PriceInput.text = string.Empty;
            }
        });

        DateInput.onEndEdit.AddListener((value) =>
        {
            if (value.CorrectDateFormat() != "")
            {
                var temp = value.Split("/");

                int.TryParse(temp[0], out DayItem);
                int.TryParse(temp[1], out MonthItem);
                int.TryParse(temp[2], out YearItem);
            }
            else DateInput.text = "";
        });

    }
    protected virtual void ResetInputs()
    {
        NameItem = string.Empty;
        PriceItem = 0;
        DayItem = DateTime.Now.Day;
        MonthItem = DateTime.Now.Month;
        YearItem = DateTime.Now.Year;
        InstallmentItem = 1;
        ShowTotalItem = false;
        SubscriptionItem = false;
        CreditedItem = true;

        NameInput.text = NameItem;
        PriceInput.text = PriceItem.ToMoney();
        DateInput.text = new DateTime(YearItem, MonthItem, DayItem).ToString("dd/MM/yyyy");

        IsMissingPrice = true;
        IsMissingName = true;
    }


    public void InEditMode(bool value) => EditMode = value;
    public virtual void Setup(Item item)
    {
        ID = item.ID;
        NameItem = item.Name;
        PriceItem = item.Value;

        DayItem = item.Day;
        MonthItem = item.Month;
        YearItem = item.Year;

        InstallmentItem = item.AmountParceled;
        ShowTotalItem = item.ShowTotal;

        NameInput.text = NameItem;
        PriceInput.text = PriceItem.ToMoney();
        DateInput.text = new DateTime(YearItem, MonthItem, DayItem).ToString("dd/MM/yyyy");
    }
    protected abstract bool IsMissingInformation();

    protected abstract void Confirm();
    protected virtual void LeaveScenario()
    {
        NameInput.onEndEdit.RemoveAllListeners();
        DateInput.onEndEdit.RemoveAllListeners();
        PriceInput.onEndEdit.RemoveAllListeners();

        EditMode = false;
        _addManager.LeaveCurrentScene();
    }

    public void FadeIn(float time)
    {
        if (scenario == null) return;

        InitializeInputs();
        ResetInputs();
        scenario.FadeIn(time);
    }
    public void FadeOut(float time)
    {
        if (scenario == null) return;
        scenario.FadeOut(time);
    }
}
