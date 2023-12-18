using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class MoneyBox : Item
{
    public string Name { get; private set; }
    public float Value { get; private set; }
    public float Percentage { get; set; }
    public int AmountParceled { get; private set; }
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

        if (amountMonths == 0)
        {
            int yearsAmount = 3;
            TimeSpams = new ItemTimeSpam[yearsAmount];

            for (int i = 0; i < yearsAmount; i++)
            {
                var listMonths = new List<int>();
                while (month < 13)
                {
                    listMonths.Add(month);
                    month++;
                }
                TimeSpams[i] = new ItemTimeSpam(year + i, listMonths);
                month = 1;
            }
        }
        else
        {
            int yearsAmount = Mathf.CeilToInt(amountMonths / 12);
            TimeSpams = new ItemTimeSpam[yearsAmount];

            for (int i = 0; i < yearsAmount; i++)
            {
                var listMonths = new List<int>();
                while (month < 13 || amountMonths != 0)
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
}