using System;
using UnityEngine;
using TMPro;

public class BoxCreationUI : BaseCreationUi
{
    [SerializeField] private TMP_InputField _percentageInput;

    private float _percentageItem;
    private bool _isMissingPercentage = true;

    protected override void InitializeInputs()
    {
        base.InitializeInputs();

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
        _percentageInput.text = _percentageItem.ToString("P2");
    }

    public override void Setup(Item item)
    {
        base.Setup(item);

        MoneyBox box = item as MoneyBox;
        _percentageInput.text = box.Percentage.ToString("P2");
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
            User.Instance.RemoveBoxByID(ID);
        }

        MoneyBox newBox = new MoneyBox(NameItem, PriceItem, _percentageItem, InstallmentItem);

        ID = Guid.NewGuid();
        newBox.SetID(Guid.NewGuid());

        User.Instance.AddBox(newBox);

        if (!EditMode) ResetInputs();
    }

    protected override bool IsMissingInformation()
    {
        if (NameItem == "") IsMissingName = true;
        else IsMissingName = false;

        if (_percentageItem <= 0) _isMissingPercentage= true;
        else _isMissingPercentage = false;

        return IsMissingName || _isMissingPercentage;
    }

    protected override void LeaveScenario()
    {
        _percentageInput.onEndEdit.RemoveAllListeners();

        base.LeaveScenario();
    }

}
