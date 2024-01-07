using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

public class AccountManager
{
    public void CalculateMonthExpense(MonthReport report)
    {

    }

    public List<Item> VerifyItemsInCurrentMonth(int month, int year)
    {
        List<Item> items = new List<Item>();


        return items;
    }
}

public struct MonthReport
{
    public float StartedWith;
    public float EndedWith;

    public int Year;
    public int Month;

    public List<Item> Items;

    public MonthReport(float startMoney, List<Item> items, int month, int year)
    {
        StartedWith = startMoney;
        EndedWith = 0;

        Year = year;
        Month = month;
        
        Items = items;
    }
}
