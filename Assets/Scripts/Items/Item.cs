using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    public enum TypeItem { Additional, Box, Expense, Wish }

    public Guid ID { get; }
    public string Name { get; }
    public float Value { get; }
    public int AmountParceled { get; }
    public int Day { get; }
    public int Month { get; }
    public int Year { get; }
    public bool ShowTotal { get; }
    public TypeItem ItemType { get; }

    public ItemTimeSpam[] TimeSpams { get; }

    public void SetID(Guid id);

    public void InitializeManually();

}

[Serializable]
public struct ItemTimeSpam
{
    [field: SerializeField] public int Year { get; private set; }
    [field: SerializeField] public List<int> Months { get; private set; }

    public ItemTimeSpam(int year, List<int> months)
    {
        Year = year;
        Months = months;
    }
}
