using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SearchItemPrefab : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private Button _editBt;

    private Item.TypeItem _typeItem;

    public void Initialize(Item item)
    {
        _name.text = item.Name;
        //_value.text = item.Value.ToMoney();

        _typeItem = item.ItemType;

        _editBt.onClick.AddListener(EditItem);
    }

    private void EditItem()
    {
        Item item = _typeItem switch
        {
            Item.TypeItem.Additional => User.Instance.AdditionalList.First(t => t.Name == _name.text),
            Item.TypeItem.Expense => User.Instance.ExpenseList.First(t => t.Name == _name.text),
            Item.TypeItem.Wish => User.Instance.WishList.First(t => t.Name == _name.text),
            Item.TypeItem.Box => User.Instance.BoxesList.First(t => t.Name == _name.text),
            _ => null
        };

        if (item != null ) AddManager.OnEditItem(item);
    }
}
