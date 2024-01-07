using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System;

public class User: MonoBehaviour
{
    public static User Instance { get; private set; }

    [field: SerializeField] public bool Test { get; private set; }

    [field: Space(10),SerializeField] public float AvailableMoney { get; private set; }
    [field: SerializeField] public float MonthlyIncome { get; private set; }
    [field: SerializeField] public float LimitCreditCard { get; private set; }
    [field: SerializeField] public int LastDay { get; private set; }


    [field: SerializeField] public bool AutoSave { get; private set; }

    [field: SerializeField] public List<Expense> ExpenseList { get; private set; }
    [field: SerializeField] public List<Additional> AdditionalList { get; private set; }
    [field: SerializeField] public List<Wish> WishList { get; private set; }
    [field: SerializeField] public List<MoneyBox> BoxesList { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        await GetDataFromDatabase();

        MainPageManager.OnUserReady();
    }

    private async Task GetDataFromDatabase()
    {
        if (Test)
        {
            foreach (Additional additional in AdditionalList) additional.InitializeManually();
            foreach (Expense expense in ExpenseList) expense.InitializeManually();
            foreach (MoneyBox box in BoxesList) box.InitializeManually();
            foreach (Wish wish in WishList) wish.InitializeManually();

            return;
        }

        UserDataRaw data = DataHolder.LoadData();

        ExpenseList = new List<Expense>();
        AdditionalList = new List<Additional>();
        BoxesList = new List<MoneyBox>();
        WishList = new List<Wish>();


        if (data == null)
        {
            AvailableMoney = 0;
            MonthlyIncome = 0;
            LimitCreditCard = 0;
            LastDay = 1;
            AutoSave = true;

            return;
        }

        AvailableMoney = data.AvailableMoney;
        MonthlyIncome = data.Income;
        LimitCreditCard = data.Limit;
        LastDay = data.LastDay;

        AutoSave = data.AutoSave == 1;

        for(int i = 0; i < data.MatrixItems.GetLongLength(0);  i++)
        {
            string name             = data.Names[i];
            float value             = data.MatrixItems[i, 0];
            int amountParceled      = (int)data.MatrixItems[i, 2];
            int day                 = (int)data.MatrixItems[i, 3];
            int month               = (int)data.MatrixItems[i, 4];
            int year                = (int)data.MatrixItems[i, 5];
            bool showTotal          = data.MatrixItems[i, 6] == 1;
            bool cancellable        = data.MatrixItems[i, 7] == 1;
            bool credited           = data.MatrixItems[i, 8] == 1;
            float percentage        = data.MatrixItems[i, 9];

            switch ((Item.TypeItem)data.MatrixItems[i, 1])
            {
                case Item.TypeItem.Expense:
                    Expense newExpense = new Expense(name, value, amountParceled, day, month, year, showTotal, cancellable, credited);
                    ExpenseList.Add(newExpense);
                    break;
                case Item.TypeItem.Additional:
                    Additional newAdditional = new Additional(name, value, amountParceled, day, month, year, showTotal);
                    AdditionalList.Add(newAdditional);
                    break;
                case Item.TypeItem.Wish:
                    Wish newWish = new Wish(name, value, day, month, year);
                    WishList.Add(newWish);
                    break;
                case Item.TypeItem.Box:
                    MoneyBox box = new MoneyBox(name, value, percentage, amountParceled);
                    BoxesList.Add(box);
                    break;
                default:
                    Debug.Log("Objects type not assigned");
                    break;
            }
        }

        await Task.Yield();
    }

    public bool SaveChange()
    {
        if (AutoSave) return false;

        DataHolder.SaveData();

        return true;
    }

    public void SetLastDayInput(int value) => LastDay = value;
    public void SetIncomeInput(float value) => MonthlyIncome = value;
    public void SetAvailableMoneyInput(float value) => AvailableMoney = value;
    public void SetLimitCreditCardInput(float value) => LimitCreditCard = value;

    public void AddBox(MoneyBox box) => BoxesList.Add(box);
    public void AddItemInWishes(Wish wish) => WishList.Add(wish);
    public void AddItemInExpenses(Expense expense) => ExpenseList.Add(expense);
    public void AddItemInAdditionals(Additional additional) => AdditionalList.Add(additional);

    public void RemoveBoxByID(Guid id)
    {
        for (int i = 0; i < BoxesList.Count; i++)
        {
            if (BoxesList[i].ID == id)
            {
                BoxesList.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveWishByID(Guid id)
    {
        for (int i = 0; i < WishList.Count; i++)
        {
            if (WishList[i].ID == id)
            {
                WishList.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveExpenseByID(Guid id)
    {
        for(int i = 0; i < ExpenseList.Count; i++)
        {
            if (ExpenseList[i].ID == id)
            {
                ExpenseList.RemoveAt(i);
                return;
            }
        }
    }
    public void RemoveAdditionalByID(Guid id)
    {
        for (int i = 0; i < AdditionalList.Count; i++)
        {
            if (AdditionalList[i].ID == id)
            {
                AdditionalList.RemoveAt(i);
                return;
            }
        }
    }
}
