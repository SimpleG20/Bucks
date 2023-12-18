using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class AddManager : MonoBehaviour
{
    public static Action<Item> onEditItem {  get; set; }
    public static void OnEditItem(Item item)
    {
        onEditItem?.Invoke(item);
    }

    private enum AddScenes { None, Additional, Box, Expense, Wish }
    private AddScenes _currentScene;

    [Header("UIs")]
    [SerializeField] private Button _boxSceneBt;
    [SerializeField] private Button _wishSceneBt;
    [SerializeField] private Button _expenseSceneBt;
    [SerializeField] private Button _additionalSceneBt;

    [SerializeField] private TMP_Text _titleTx;
    [SerializeField] private CanvasGroup _optionsScenario;
    [SerializeField] private BoxCreationUI _boxScenario;
    [SerializeField] private WishCreationUI _wishScenario;
    [SerializeField] private ExpenseCreationUI _expenseScenario;
    [SerializeField] private AdditionalCreationUI _additionalScenario;

    private void Awake()
    {
        onEditItem += EditItem;

        InitializeButtons();
    }


    private void InitializeButtons()
    {
        _boxSceneBt.onClick.AddListener(() => LoadOtherScenario(AddScenes.Box));
        _wishSceneBt.onClick.AddListener(() => LoadOtherScenario(AddScenes.Wish));
        _expenseSceneBt.onClick.AddListener(() => LoadOtherScenario(AddScenes.Expense));
        _additionalSceneBt.onClick.AddListener(() => LoadOtherScenario(AddScenes.Additional));
    }

    public async void EditItem(Item item)
    {
        if (!PagesManager.Instance.IsInPage(PagesManager.Pages.Add))
        {
            await Task.Run(() => PagesManager.Instance.AddScene());
        }

        int scenario = (int)item.ItemType + 1;

        await SetupItem(item);

        LoadOtherScenario((AddScenes)scenario);
    }

    public async void EditBox(MoneyBox box)
    {
        if (!PagesManager.Instance.IsInPage(PagesManager.Pages.Add))
        {
            await Task.Run(() => PagesManager.Instance.AddScene());
        }

        _boxScenario.FadeIn(0.75f);
    }

    private async Task SetupItem(Item item)
    {
        switch (item.ItemType)
        {
            case Item.TypeItem.Additional:
                Additional additional = item as Additional;
                _additionalScenario.Setup(additional);
                break;
            case Item.TypeItem.Box:
                MoneyBox box = item as MoneyBox;
                _boxScenario.Setup(box);
                break;
            case Item.TypeItem.Expense:
                Expense expense = item as Expense;
                _expenseScenario.Setup(expense);
                break;
            case Item.TypeItem.Wish:
                Wish wish = item as Wish;
                _wishScenario.Setup(wish);
                break;
        }

        await Task.Yield();
    }

    private void LoadOtherScenario(AddScenes type)
    {
        if (type == AddScenes.None)
        {
            _optionsScenario.FadeIn(0.75f);
            _currentScene = AddScenes.None;
            return;
        }

        switch(type)
        {
            case AddScenes.Additional:
                _additionalScenario.FadeIn(0.75f);
                _currentScene = AddScenes.Additional;
                break;
            case AddScenes.Expense:
                _expenseScenario.FadeIn(0.75f);
                _currentScene = AddScenes.Expense;
                break;
            case AddScenes.Wish:
                _wishScenario.FadeIn(0.75f);
                _currentScene = AddScenes.Wish;
                break;
            case AddScenes.Box:
                _boxScenario.FadeIn(0.75f);
                _currentScene = AddScenes.Box;
                break;
        }

        _optionsScenario.FadeOut(0.45f);
    }

    public void LeaveCurrentScene()
    {
        switch (_currentScene)
        {
            case AddScenes.Additional:
                _additionalScenario.FadeOut(0.5f);
                break;
            case AddScenes.Box:
                _boxScenario.FadeOut(0.5f);
                break;
            case AddScenes.Expense: 
                _expenseScenario.FadeOut(0.5f);
                break;
            case AddScenes.Wish: 
                _wishScenario.FadeOut(0.5f);
                break;
        }

        _optionsScenario.FadeIn();
        _titleTx.text = "Escolha o que deseja";

        User.Instance.SaveChange();
    }
}
