using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class AdditionalCreationUI : BaseCreationUi
{
    [SerializeField] private TMP_InputField _installmentInput;
    [SerializeField] private Toggle _showTotalPriceTg;


    protected override void InitializeInputs()
    {
        base.InitializeInputs();

        _installmentInput.onEndEdit.AddListener((value) =>
        {
            float number = _installmentInput.SetTextToFloat();

            if (number <= 0)
            {
                InstallmentItem = 1;
                _showTotalPriceTg.isOn = true;
            }
            else InstallmentItem = (int)number;

            _installmentInput.text = InstallmentItem.ToString("00");
        });

        _showTotalPriceTg.onValueChanged.AddListener((value) => ShowTotalItem = value);
    }

    protected override void ResetInputs()
    {
        base.ResetInputs();

        _installmentInput.text = InstallmentItem.ToString("00");
        _showTotalPriceTg.isOn = ShowTotalItem;
    }

    public override void Setup(Item item)
    {
        base.Setup(item);

        Additional additional = item as Additional;

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

        if (EditMode)
        {
            User.Instance.RemoveAdditionalByID(ID);
        }

        Additional newAdditional = new Additional(NameItem, PriceItem, InstallmentItem,
            DayItem, MonthItem, YearItem, ShowTotalItem);

        ID = Guid.NewGuid();
        newAdditional.SetID(ID);

        User.Instance.AddItemInAdditionals(newAdditional);

        if (!EditMode) ResetInputs();
    }

    protected override bool IsMissingInformation()
    {
        if (NameItem == "") IsMissingName = true;
        else IsMissingName = false;

        if (PriceItem <= 0) IsMissingPrice = true;
        else IsMissingPrice = false;

        return IsMissingName || IsMissingPrice;
    }

    protected override void LeaveScenario()
    {
        _installmentInput.onEndEdit.RemoveAllListeners();
        _showTotalPriceTg.onValueChanged.RemoveAllListeners();

        base.LeaveScenario();
    }
}
