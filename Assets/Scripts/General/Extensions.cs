using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public static class Extensions
{
    public static readonly string regexDatePattern = @"^(0[1-9]|[12][0-9]|3[01])\/(0[1-9]|1[0-2])\/20[0-9][0-9]$";

    public static T PickRandom<T>(this List<T> list)
    {
        if (list == null) return default(T);

        var item = list[Random.Range(0, list.Count)];
        return item;
    }

    public static T GetByName<T>(this List<T> list, string itemToPick)
    {
        foreach(T item in list)
        {
            if (item.Equals(itemToPick)) return item;
        }

        return default(T);
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

    public static string CorrectDateFormat(this string str)
    {
        string input = str;

        if (Regex.IsMatch(input, regexDatePattern) == false)
        {
            return "";
        }

        return input;
    }

    public static void DeleteChildren(this Transform parent)
    {
        if (parent == null) return;

        foreach(Transform child in parent) Object.Destroy(child.gameObject);
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
