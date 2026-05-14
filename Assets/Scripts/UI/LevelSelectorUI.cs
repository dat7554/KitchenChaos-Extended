using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    [Space]
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    [Header("Buttons")]
    [SerializeField] private Button easyGameModeButton;
    [SerializeField] private Button normalGameModeButton;
    [SerializeField] private Button hardGameModeButton;
    
    [Header("Game Modes")]
    [SerializeField] private GameModeSO easyGameModeSO;
    [SerializeField] private GameModeSO normalGameModeSO;
    [SerializeField] private GameModeSO hardGameModeSO;

    private GameModeButtonUI _hoveredButton;
    private GameModeButtonUI _selectedButton;
    
    private void Awake()
    {
        easyGameModeButton.onClick.AddListener(() =>
        {
            GameModeSelector.SetGameModeSO(easyGameModeSO);
            Loader.Load(Loader.Scene.GameScene);
        });
        
        normalGameModeButton.onClick.AddListener(() =>
        {
            GameModeSelector.SetGameModeSO(normalGameModeSO);
            Loader.Load(Loader.Scene.GameScene);
        });
        
        hardGameModeButton.onClick.AddListener(() =>
        {
            GameModeSelector.SetGameModeSO(hardGameModeSO);
            Loader.Load(Loader.Scene.GameScene);
        });
    }

    private void Start()
    {
        mainMenuUI.OnPlayButtonClicked += MainMenuUI_OnPlayButtonClicked;
        
        Hide();
    }

    private void OnDestroy()
    {
        mainMenuUI.OnPlayButtonClicked -= MainMenuUI_OnPlayButtonClicked;
    }

    public void OnButtonHovered(GameModeButtonUI button)
    {
        _hoveredButton = button;
        UpdateDescription();
    }
    
    public void OnButtonUnhovered(GameModeButtonUI button)
    {
        if (_hoveredButton == button) _hoveredButton = null;
        UpdateDescription();
    }
    
    public void OnButtonSelected(GameModeButtonUI button)
    {
        _selectedButton = button;
        UpdateDescription();
    }
    
    public void OnButtonDeselected(GameModeButtonUI button)
    {
        if (_selectedButton == button) _selectedButton = null;
        UpdateDescription();
    }

    private void MainMenuUI_OnPlayButtonClicked(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UpdateDescription()
    {
        GameModeButtonUI activeButton = _hoveredButton ?? _selectedButton;
        descriptionText.text = activeButton != null ? activeButton.GetGameModeSO().description : "";
    }
}
