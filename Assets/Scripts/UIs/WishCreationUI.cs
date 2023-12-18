using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WishCreationUI : BaseCreationUi
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _priceInput;
    [SerializeField] private TMP_InputField _dateInput;

    private bool _isMissingName = true;
    private bool _isMissingPrice = true;

    protected override void Awake()
    {
        base.Awake();

        _dateInput.onValidateInput += delegate (string input, int charIndex, char addedChar) {
            string newText = _dateInput.text.Remove(charIndex, 1);
            newText = newText.Insert(charIndex, addedChar.ToString());
            if (System.Text.RegularExpressions.Regex.IsMatch(newText, regexDatePattern))
            {
                return addedChar;
            }
            return '\0';
        };
    }

    public override void Setup<T>(T item)
    {
        Wish wish = item as Wish;

        _nameInput.text = wish.Name;
        _priceInput.text = wish.Value.ToMoney();
        _dateInput.text = new DateTime(wish.Year, wish.Month, wish.Day).ToShortDateString();
    }

    protected override void Confirm()
    {
        if (IsMissingInformation())
        {
            print("Faltando Informação");
            return;
        }

        Wish newWish = new Wish(NameItem, PriceItem, DayItem, MonthItem, YearItem);

        User.Instance.AddItemInWishes(newWish);
        ResetInputs();
    }

    protected override void InitializeInputs()
    {
        _nameInput.onEndEdit.AddListener((value) =>
        {
            if (value != "")
            {
                NameItem = value;
                _isMissingName = false;
            }
            else _isMissingName = true;
        });

        _priceInput.onEndEdit.AddListener((value) =>
        {
            float number = _priceInput.SetTextToFloat();
            if (number > 0)
            {
                PriceItem = number;
                _isMissingPrice = false;
                _priceInput.text = number.ToMoney();
            }
            else
            {
                _isMissingPrice = true;
                _priceInput.text = string.Empty;
            }
        });

        _dateInput.onEndEdit.AddListener((value) =>
        {
            if (value != "")
            {
                var temp = value.Split("/");

                int.TryParse(temp[0], out DayItem);
                int.TryParse(temp[1], out MonthItem);
                int.TryParse(temp[2], out YearItem);
            }
        });
    }

    protected override bool IsMissingInformation() => _isMissingName || _isMissingPrice;

    protected override void ResetInputs()
    {
        base.ResetInputs();

        _nameInput.text = NameItem;
        _priceInput.text = PriceItem.ToMoney();
        _dateInput.text = new DateTime(YearItem, MonthItem, DayItem).ToShortDateString();
    }

    protected override void LeaveScenario()
    {
        _nameInput.onEndEdit.RemoveAllListeners();
        _dateInput.onEndEdit.RemoveAllListeners();
        _priceInput.onEndEdit.RemoveAllListeners();

        base.LeaveScenario();
    }
}
