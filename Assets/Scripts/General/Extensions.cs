using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public static class Extensions
{
    public static T PickRandom<T>(this List<T> list)
    {
        if (list == null) return default(T);

        var item = list[Random.Range(0, list.Count)];
        return item;
    }

    public static float SetTextToFloat(this TMP_InputField input)
    {
        char[] charsToTrim = { 'R', '$', ' ', '%' };
        string text = input.text.Trim(charsToTrim);
        float.TryParse(text, out float value);

        return value;
    }

    public static string ToMoney(this float number)
    {
        return number.ToString("C", CultureInfo.CurrentCulture);
    }

    public static void FadeIn(this CanvasGroup canvas, float time = 1f)
    {
        canvas.blocksRaycasts = true;
        LeanTween.value(0, 1, time).setOnUpdate((value) =>
        {
            canvas.alpha = value;
        });
    }

    public static void FadeOut(this CanvasGroup canvas, float time = 1f)
    {
        LeanTween.value(1, 0, time).setOnUpdate((value) =>
        {
            canvas.alpha = value;
        }).setOnComplete(() => canvas.blocksRaycasts = false);
    }
}
