using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public abstract class BaseCreationUi : MonoBehaviour
{
    protected readonly string regexDatePattern = "^(0[1 - 9] | [12][0 - 9] | 3[01]) / (0[1 - 9] | 1[0 - 2]) / 202[0-9]$";


    [SerializeField] protected CanvasGroup scenario;
    [SerializeField] protected Button _confirmBt;
    [SerializeField] protected Button _leaveBt;

    [SerializeField] protected AddManager _addManager;

    protected string NameItem;
    protected float PriceItem;
    protected int DayItem;
    protected int MonthItem;
    protected int YearItem;
    protected int InstallmentItem;
    protected bool ShowTotalItem;
    protected bool SubscriptionItem;
    protected bool CreditedItem;

    protected virtual void Awake()
    {
        _confirmBt.onClick.AddListener(Confirm);
        _leaveBt.onClick.AddListener(LeaveScenario);
    }


    protected abstract void InitializeInputs();
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
    }

    protected abstract void Confirm();

    protected abstract bool IsMissingInformation();
    public abstract void Setup<T>(T item);

    protected virtual void LeaveScenario()
    {
        _addManager.LeaveCurrentScene();
    }

    public void FadeIn(float time)
    {
        if (scenario == null) return;

        InitializeInputs();
        scenario.FadeIn(time);
    }
    public void FadeOut(float time)
    {
        if (scenario == null) return;
        scenario.FadeOut(time);
    }
}
