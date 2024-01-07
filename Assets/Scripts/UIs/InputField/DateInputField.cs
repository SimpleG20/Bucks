using System;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class DateInputField : MonoBehaviour
{
    private TMP_InputField _inputField;
    private string regex = @"^([1-9]|[0-9])\.([1-9]|1[0-2])\.20\d{2}$";

    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();

        //Setup();
    }

    private void Update()
    {
        if (!Regex.IsMatch(_inputField.text, regex))
        {
            _inputField.text = "";
            return;
        }
    }

    private void Setup()
    {
        _inputField.onValueChanged.AddListener((value) =>
        {
            string regex = @"^([1-9]|[12][0-9]|3[01])\.(0[1-9]|1[0-2])\.\d{4}$";

            if (!Regex.IsMatch(value, regex))
            {
                _inputField.text = string.Empty;
                return;
            }
        });

        //_inputField.onEndEdit.AddListener((value) =>
        //{
        //    if (value == null) return;

        //    string[] inputs = value.Split('.');
        //    int day = int.Parse(inputs[0]);
        //    int month = int.Parse(inputs[1]);
        //    int year = int.Parse(inputs[2]);
        //});
    }
}
