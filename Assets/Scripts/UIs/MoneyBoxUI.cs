using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;

public class MoneyBoxUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameTx;
    [SerializeField] private TMP_Text _currentMoneyTx;
    [SerializeField] private TMP_InputField _percentageInput;

    private MoneyBox _moneyBox;

    private void Start()
    {
        _percentageInput.onEndEdit.AddListener((value) => MoneyBoxEdited());
    }

    public void InitializeVisualization(MoneyBox box)
    {
        _moneyBox = box;

        _nameTx.text = box.Name;
        _currentMoneyTx.text = box.Value.ToString("C", CultureInfo.CurrentCulture);
        _percentageInput.text = box.Percentage.ToString("P");
    }

    private void MoneyBoxEdited()
    {
        float.TryParse(_percentageInput.text, out float value);
        _moneyBox.Percentage = value / 100;
    }
}
