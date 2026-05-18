using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
    public event EventHandler OnGameModeButtonClicked;
 
    [Serializable]
    private struct GameModeButton
    {
        public GameModeSO gameModeSO;
        public Button button;
    }
    
    [SerializeField] private MainMenuUI mainMenuUI;
    [Space]
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    [SerializeField] private GameModeButton[] gameModeButtonList;

    private GameModeButtonUI _hoveredButton;
    private GameModeButtonUI _selectedButton;
    
    private void Awake()
    {
        foreach (var gameModeButton in gameModeButtonList)
        {
            gameModeButton.button.onClick.AddListener(() =>
            {
                GameModeSelector.SetGameModeSO(gameModeButton.gameModeSO);
                OnGameModeButtonClicked?.Invoke(this, EventArgs.Empty);
                Hide();
            });
        }
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

    private void MainMenuUI_OnPlayButtonClicked(object sender, EventArgs e) => Show();

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);

    private void UpdateDescription()
    {
        GameModeButtonUI activeButton = _hoveredButton ?? _selectedButton;
        descriptionText.text = activeButton != null ? activeButton.GetGameModeSO().description : "";
    }
}
