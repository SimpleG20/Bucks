using System;
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
        _boxSceneBt.onClick.AddListener(() => LoadSubScenario(AddScenes.Box));
        _wishSceneBt.onClick.AddListener(() => LoadSubScenario(AddScenes.Wish));
        _expenseSceneBt.onClick.AddListener(() => LoadSubScenario(AddScenes.Expense));
        _additionalSceneBt.onClick.AddListener(() => LoadSubScenario(AddScenes.Additional));
    }

    public void EditItem(Item item)
    {
        if (!PagesManager.Instance.IsInPage(PagesManager.Pages.Add))
        {
            PagesManager.Instance.GoToAddScene();
        }

        int scenario = (int)item.ItemType + 1;

        LoadSubScenario((AddScenes)scenario);
        SetupItem(item);
    }

    public async void EditBox(MoneyBox box)
    {
        if (!PagesManager.Instance.IsInPage(PagesManager.Pages.Add))
        {
            await Task.Run(() => PagesManager.Instance.GoToAddScene());
        }

        _boxScenario.FadeIn(0.75f);
    }

    private void SetupItem(Item item)
    {
        switch (item.ItemType)
        {
            case Item.TypeItem.Additional:
                _additionalScenario.InEditMode(true);
                _additionalScenario.Setup(item);
                break;
            case Item.TypeItem.Box:
                _boxScenario.InEditMode(true);
                _boxScenario.Setup(item);
                break;
            case Item.TypeItem.Expense:
                _expenseScenario.InEditMode(true);
                _expenseScenario.Setup(item);
                break;
            case Item.TypeItem.Wish:
                _wishScenario.InEditMode(true);
                _wishScenario.Setup(item);
                break;
        }
    }

    private void LoadSubScenario(AddScenes type)
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
                _titleTx.text = "Novo extra";
                break;
            case AddScenes.Expense:
                _expenseScenario.FadeIn(0.75f);
                _currentScene = AddScenes.Expense;
                _titleTx.text = "Nova despesa";
                break;
            case AddScenes.Wish:
                _wishScenario.FadeIn(0.75f);
                _currentScene = AddScenes.Wish;
                _titleTx.text = "Novo desejo";
                break;
            case AddScenes.Box:
                _boxScenario.FadeIn(0.75f);
                _currentScene = AddScenes.Box;
                _titleTx.text = "Nova caixinha";
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

        if (!User.Instance.Test) User.Instance.SaveChange();
    }
}
