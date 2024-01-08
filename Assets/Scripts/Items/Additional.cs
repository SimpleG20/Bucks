using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class Additional : Item
{
    public Guid ID { get; private set; }
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

        InitializeManually();
    }

    public void SetID(Guid id) => ID = id;
    public void InitializeManually()
    {
        ID = Guid.NewGuid();

        int amountParceled = AmountParceled;
        int month = Month;
        int year = Year;
        int yearsAmount = Mathf.CeilToInt((month + amountParceled) / 12) + 1;
        TimeSpams = new ItemTimeSpam[yearsAmount];

        for (int i = 0; i < yearsAmount; i++)
        {
            var listMonths = new List<int>();
            while (month < 13 && amountParceled > 0)
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
