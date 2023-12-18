using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Additional : Item
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float Value { get; private set; }
    [field: SerializeField] public int AmountParceled { get; private set; }
    [field: SerializeField] public int Day { get; private set; }
    [field: SerializeField] public int Month { get; private set; }
    [field: SerializeField] public int Year { get; private set; }
    [field: SerializeField] public bool ShowTotal { get; private set; }
    public Item.TypeItem ItemType => Item.TypeItem.Additional;

    public ItemTimeSpam[] TimeSpams { get; private set;}

    public Additional(string name, float value, int amountParceled, int day, int month, int year, bool showTotal)
    {
        Name = name;
        Value = value;
        AmountParceled = amountParceled;
        Day = day;
        Month = month;
        Year = year;
        ShowTotal = showTotal;

        int yearsAmount = Mathf.CeilToInt(amountParceled / 12);
        TimeSpams = new ItemTimeSpam[yearsAmount];

        for (int i = 0; i < yearsAmount; i++)
        {
            var listMonths = new List<int>();
            while (month < 13 || amountParceled != 0)
            {
                listMonths.Add(month);
                amountParceled--;
                month++;
            }
            TimeSpams[i] = new ItemTimeSpam(year + i, listMonths);
            month = 1;
        }
    }
}