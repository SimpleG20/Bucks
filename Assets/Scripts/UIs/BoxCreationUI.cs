using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxCreationUI : BaseCreationUi
{
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _priceInput;
    [SerializeField] private TMP_InputField _durationMonthsInput;
    [SerializeField] private TMP_InputField _percentageInput;

    private float _percentageItem;

    private bool _isMissingName = true;
    private bool _isMissingPercentage = true;

    public override void Setup<T>(T item)
    {
        MoneyBox box = item as MoneyBox;

        _nameInput.text = box.Name;
        _priceInput.text = box.Value.ToMoney();
        _durationMonthsInput.text = box.AmountParceled.ToString("00");
        _percentageInput.text = box.Percentage.ToString("P2");
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
                _priceInput.text = number.ToMoney();
            }
            else
            {
                _priceInput.text = string.Empty;
            }
        });

        _durationMonthsInput.onEndEdit.AddListener((value) =>
        {

            float number = _priceInput.SetTextToFloat();
            if (number >= 0)
            {
                InstallmentItem = (int)number;
                _priceInput.text = number.ToString("00");
            }
            else
            {
                _priceInput.text = string.Empty;
            }
        });

        _percentageInput.onEndEdit.AddListener((value) =>
        {
            float number = _percentageInput.SetTextToFloat();
            if (number > 0)
            {
                PriceItem = number;
                _percentageInput.text = number.ToString("P2");
                _isMissingPercentage = false;
            }
            else
            {
                _isMissingPercentage = true;
                _percentageInput.text = string.Empty;
            }
        });
    }

    protected override void ResetInputs()
    {
        base.ResetInputs();
        _percentageItem = 0.1f;

        _nameInput.text = NameItem;
        _priceInput.text = PriceItem.ToMoney();
        _durationMonthsInput.text = InstallmentItem.ToString("00");
        _percentageInput.text = _percentageItem.ToString("P2");
    }

    protected override void Confirm()
    {
        if (IsMissingInformation())
        {
            print("Faltando Informação");
            return;
        }

        MoneyBox newBox = new MoneyBox(NameItem, PriceItem, _percentageItem, InstallmentItem);

        User.Instance.AddBox(newBox);
        ResetInputs();
    }

    protected override bool IsMissingInformation() => _isMissingName || _isMissingPercentage;

    protected override void LeaveScenario()
    {
        _nameInput.onEndEdit.RemoveAllListeners();
        _priceInput.onEndEdit.RemoveAllListeners();
        _percentageInput.onEndEdit.RemoveAllListeners();
        _durationMonthsInput.onEndEdit.RemoveAllListeners();

        base.LeaveScenario();
    }

}
