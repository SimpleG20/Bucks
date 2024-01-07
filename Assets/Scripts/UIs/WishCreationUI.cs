using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WishCreationUI : BaseCreationUi
{
    protected override void Confirm()
    {
        if (IsMissingInformation())
        {
            print("Faltando Informação");
            return;
        }

        if (EditMode)
        {
            User.Instance.RemoveWishByID(ID);
        }

        Wish newWish = new Wish(NameItem, PriceItem, DayItem, MonthItem, YearItem);

        ID = Guid.NewGuid();
        newWish.SetID(ID);

        User.Instance.AddItemInWishes(newWish);
        
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
}
