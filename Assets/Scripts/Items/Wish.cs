
using System;
using UnityEngine;

[Serializable]
public class Wish : Item
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public float Value { get; private set; }

    [field: SerializeField] public int Day { get; private set; }

    [field: SerializeField] public int Month { get; private set; }

    [field: SerializeField] public int Year { get; private set; }

    public int AmountParceled => 0;

    public bool ShowTotal => true;

    public Item.TypeItem ItemType => Item.TypeItem.Wish;

    public ItemTimeSpam[] TimeSpams => null;

    public Wish(string name, float value, int day, int month, int year)
    {
        Name = name;
        Value = value;
        Day = day;
        Month = month;
        Year = year;
    }
}
