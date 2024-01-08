using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoneyBox : Item
{
    public Guid ID { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float Value { get; private set; }
    [field: SerializeField] public float Percentage { get; set; }
    [field: SerializeField] public int AmountParceled { get; private set; }
    public int Day => 1;
    public int Month => 1;
    public int Year => DateTime.Now.Year;
    public bool ShowTotal => true;
    public Item.TypeItem ItemType => Item.TypeItem.Box;

    public ItemTimeSpam[] TimeSpams { get; private set; }

    public MoneyBox(string name, float value, float percentage, int amountMonths)
    {
        Name = name;
        Value = value;
        Percentage = percentage;
        AmountParceled = amountMonths;

        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;

        InitializeManually();
    }

    public void SetID(Guid id) => ID = id;
    public void InitializeManually()
    {
        ID = Guid.NewGuid();

        int amountMonths = AmountParceled;
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;

        int yearsAmount = Mathf.CeilToInt((month + amountMonths) / 12) + 1;
        TimeSpams = new ItemTimeSpam[yearsAmount];

        for (int i = 0; i < yearsAmount; i++)
        {
            var listMonths = new List<int>();
            while (month < 13 && amountMonths > 0)
            {
                listMonths.Add(month);
                amountMonths--;
                month++;
            }
            TimeSpams[i] = new ItemTimeSpam(year + i, listMonths);
            month = 1;
        }
    }
}