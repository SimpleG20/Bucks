using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SearchItemPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private Button _editBt;

    public void Initialize(Item item)
    {
        _name.text = item.Name;
        _value.text = item.Value.ToMoney();

        switch (item.ItemType)
        {
            case Item.TypeItem.Expense:
                break;
            case Item.TypeItem.Additional:
                break;
            case Item.TypeItem.Wish:
                break;
        }

        _editBt.onClick.AddListener(() => { });
    }
}
