using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReportItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameTx;
    [SerializeField] private TMP_Text _valueTx;

    [SerializeField] private Color _debtColor;
    [SerializeField] private Color _extraColor;

    private Image _imageSource;

    private void Awake()
    {
        _imageSource = GetComponent<Image>();
    }

    public void ShowInfo(string name, string value, bool debt)
    {
        _nameTx.text = name;
        _valueTx.text = value;
        if (debt) _imageSource.color = _debtColor;
        else _imageSource.color = _extraColor;

        gameObject.SetActive(true);
    }
}
