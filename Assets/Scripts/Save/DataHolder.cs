using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;
using Unity.VisualScripting;

public static class DataHolder
{
    private static void Save(UserDataRaw data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Items_MD.asdg";

        FileStream stream = new FileStream(path, FileMode.Create);


        formatter.Serialize(stream, data);
        stream.Close();
    }
    private static UserDataRaw Load()
    {
        string path = Application.persistentDataPath + "/Items_MD.asdg";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

            UserDataRaw data;

            data = formatter.Deserialize(stream) as UserDataRaw;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("DataHolder file not found in " + path);

            return null;
        }
    }


    public static UserDataRaw LoadData()
    {
        UserDataRaw itemsData = Load();

        if (itemsData == null ) 
        {
            Debug.Log("Novo Salvamento");
            return null;
        }

        Debug.Log("Carregado Com Sucesso");
        return itemsData;
    }

    public static void SaveData()
    {
        UserDataRaw data = new UserDataRaw();

        data.AvailableMoney = User.Instance.AvailableMoney;
        data.Limit = User.Instance.LimitCreditCard;
        data.Income = User.Instance.MonthlyIncome;
        data.LastDay = User.Instance.LastDay;

        data.AutoSave = User.Instance.AutoSave ? 1 : 0;

        int size = User.Instance.ExpenseList.Count + User.Instance.AdditionalList.Count + User.Instance.WishList.Count + User.Instance.BoxesList.Count;

        data.Names = new string[size];
        data.MatrixItems = new float[size, 10];

        int index = 0;
        foreach (Expense expense in User.Instance.ExpenseList)
        {
            data.Names[index] = expense.Name;
            data.MatrixItems[index, 0] = expense.Value;
            data.MatrixItems[index, 1] = (int)expense.ItemType;
            data.MatrixItems[index, 2] = expense.AmountParceled;
            data.MatrixItems[index, 3] = expense.Day;
            data.MatrixItems[index, 4] = expense.Month;
            data.MatrixItems[index, 5] = expense.Year;
            data.MatrixItems[index, 6] = expense.ShowTotal ? 1 : 0;
            data.MatrixItems[index, 7] = expense.Subscription ? 1 : 0;
            data.MatrixItems[index, 8] = expense.Credited ? 1 : 0;
            data.MatrixItems[index, 9] = 0;

            index++;
        }

        foreach (Additional additional in User.Instance.AdditionalList)
        {
            data.Names[index] = additional.Name;
            data.MatrixItems[index, 0] = additional.Value;
            data.MatrixItems[index, 1] = (int)additional.ItemType;
            data.MatrixItems[index, 2] = additional.AmountParceled;
            data.MatrixItems[index, 3] = additional.Day;
            data.MatrixItems[index, 4] = additional.Month;
            data.MatrixItems[index, 5] = additional.Year;
            data.MatrixItems[index, 6] = additional.ShowTotal ? 1 : 0;
            data.MatrixItems[index, 7] = 0;
            data.MatrixItems[index, 8] = 0;
            data.MatrixItems[index, 9] = 0;

            index++;
        }

        foreach (Wish wish in User.Instance.WishList)
        {
            data.Names[index] = wish.Name;
            data.MatrixItems[index, 0] = wish.Value;
            data.MatrixItems[index, 1] = (int)wish.ItemType;
            data.MatrixItems[index, 2] = 0;
            data.MatrixItems[index, 3] = wish.Day;
            data.MatrixItems[index, 4] = wish.Month;
            data.MatrixItems[index, 5] = wish.Year;
            data.MatrixItems[index, 6] = 1;
            data.MatrixItems[index, 7] = 0;
            data.MatrixItems[index, 8] = 0;
            data.MatrixItems[index, 9] = 0;

            index++;
        }

        foreach(MoneyBox box in User.Instance.BoxesList)
        {
            data.Names[index] = box.Name;
            data.MatrixItems[index, 0] = box.Value;
            data.MatrixItems[index, 1] = (int)box.ItemType;
            data.MatrixItems[index, 2] = 0;
            data.MatrixItems[index, 3] = box.Day;
            data.MatrixItems[index, 4] = box.Month;
            data.MatrixItems[index, 5] = box.Year;
            data.MatrixItems[index, 6] = 1;
            data.MatrixItems[index, 7] = 0;
            data.MatrixItems[index, 8] = 0;
            data.MatrixItems[index, 9] = box.Percentage;

            index++;
        }

        Save(data);
    }
}
