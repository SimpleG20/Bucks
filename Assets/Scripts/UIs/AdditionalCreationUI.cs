using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AdditionalCreationUI : BaseCreationUi
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _priceInput;
    [SerializeField] private TMP_InputField _dateInput;
    [SerializeField] private TMP_InputField _installmentInput;
    [SerializeField] private Toggle _showTotalPriceTg;

    private bool _isMissingName = true;
    private bool _isMissingPrice = true;
    private bool _isMissingDate = true;

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

                _isMissingDate = false;
            }
            else _isMissingDate = true;
        });

        _installmentInput.onEndEdit.AddListener((value) =>
        {
            float number = _installmentInput.SetTextToFloat();

            if (number <= 0)
            {
                InstallmentItem = 1;
                _showTotalPriceTg.isOn = true;
            }
            else InstallmentItem = (int)number;

            _priceInput.text = InstallmentItem.ToString("00");
        });

        _showTotalPriceTg.onValueChanged.AddListener((value) => ShowTotalItem = value);
    }

    protected override void ResetInputs()
    {
        base.ResetInputs();

        _nameInput.text = NameItem;
        _priceInput.text = PriceItem.ToMoney();
        _dateInput.text = new DateTime(YearItem, MonthItem, DayItem).ToShortDateString();
        _installmentInput.text = InstallmentItem.ToString("00");

        _showTotalPriceTg.isOn = ShowTotalItem;
    }

    public override void Setup<T>(T item)
    {
        Additional additional = item as Additional;

        _nameInput.text = additional.Name;
        _priceInput.text = additional.Value.ToMoney();
        _dateInput.text = new DateTime(additional.Year, additional.Month, additional.Year).ToShortDateString();
        _installmentInput.text = additional.AmountParceled.ToString("00");

        _showTotalPriceTg.isOn = additional.ShowTotal;
    }

    protected override void Confirm()
    {
        if (IsMissingInformation())
        {
            print("Faltando Informação");
            return;
        }

        Additional newAdditional = new Additional(NameItem, PriceItem, InstallmentItem,
            DayItem, MonthItem, YearItem, ShowTotalItem);

        User.Instance.AddItemInAdditionals(newAdditional);
        ResetInputs();
    }

    protected override bool IsMissingInformation() => _isMissingPrice || _isMissingDate || _isMissingName;

    protected override void LeaveScenario()
    {
        _nameInput.onEndEdit.RemoveAllListeners();
        _dateInput.onEndEdit.RemoveAllListeners();
        _priceInput.onEndEdit.RemoveAllListeners();
        _installmentInput.onEndEdit.RemoveAllListeners();

        _showTotalPriceTg.onValueChanged.RemoveAllListeners();

        base.LeaveScenario();
    }
}
