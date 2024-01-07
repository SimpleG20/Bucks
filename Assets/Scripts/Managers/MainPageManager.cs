using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainPageManager : MonoBehaviour
{
    public static Action onUserReady { get; set; }
    public static void OnUserReady()
    {
        onUserReady?.Invoke();
    }


    [SerializeField] private TMP_InputField _availableMoneyTx;
    [SerializeField] private TMP_InputField _limitInput;
    [SerializeField] private TMP_InputField _incomeInput;
    [SerializeField] private TMP_InputField _lastDayInput;

    //[SerializeField] private Toggle _moneyVisualizationTg;

    [Header("Save")]
    [SerializeField] private GameObject _savedChangesImg;
    [SerializeField] private Button _saveChangesBt;

    [SerializeField] private Boxes _boxes;

    private void Awake()
    {
        onUserReady += HandleStart;
    }


    private void HandleStart()
    {
        InitializeVisualization();
        InitializeInputsFields();

        _saveChangesBt.onClick.AddListener(SaveChanges);
        _saveChangesBt.interactable = false;
    }

    public void InitializeVisualization()
    {
        _availableMoneyTx.text = User.Instance.AvailableMoney.ToMoney();
        _limitInput.text = User.Instance.LimitCreditCard.ToMoney();
        _incomeInput.text = User.Instance.MonthlyIncome.ToMoney();
        _lastDayInput.text = User.Instance.LastDay.ToString("00");

        _boxes.UpdateBoxes();
    }

    public void SaveChanges()
    {
        DataHolder.SaveData();
        _saveChangesBt.interactable = false;
        _savedChangesImg.SetActive(true);
    }

    private void InitializeInputsFields()
    {
        _availableMoneyTx.onEndEdit.AddListener((value) => {
            float number = _availableMoneyTx.SetTextToFloat();
            if (number < 0) _availableMoneyTx.text = string.Empty;
            else
            {
                _availableMoneyTx.text = number.ToMoney();

                User.Instance.SetAvailableMoneyInput(number);
                User.Instance.SaveChange();

                SetSaveInteractable();
            }
        });

        _incomeInput.onEndEdit.AddListener((value) => {
            float number = _incomeInput.SetTextToFloat();
            if (number < 0) _incomeInput.text = string.Empty;
            else
            {
                _incomeInput.text = number.ToMoney();

                User.Instance.SetIncomeInput(number);
                User.Instance.SaveChange();

                SetSaveInteractable();
            }
        });

        _limitInput.onEndEdit.AddListener((value) => {
            float number = _limitInput.SetTextToFloat();
            if (number < 0) _limitInput.text = string.Empty;
            else
            {
                _limitInput.text = number.ToMoney();

                User.Instance.SetLimitCreditCardInput(number);
                User.Instance.SaveChange();

                SetSaveInteractable();
            }
        });

        _lastDayInput.onEndEdit.AddListener((value) => {
            float number = _lastDayInput.SetTextToFloat();
            if (number > 0 && number < 28)
            {
                _lastDayInput.text = number.ToString("00");

                User.Instance.SetLastDayInput((int)number);
                User.Instance.SaveChange();

                SetSaveInteractable();
            }
            else
            {
                _lastDayInput.text = string.Empty;
            }
        });
    }

    private void SetSaveInteractable()
    {
        _saveChangesBt.interactable = true;
        _savedChangesImg.SetActive(false);
    }
}
