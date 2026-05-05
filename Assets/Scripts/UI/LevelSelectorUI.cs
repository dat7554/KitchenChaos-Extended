using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorUI : MonoBehaviour
{
    [SerializeField] private MainMenuUI mainMenuUI;
    
    [Header("Buttons")]
    [SerializeField] private Button easyGameModeButton;
    [SerializeField] private Button normalGameModeButton;
    [SerializeField] private Button hardGameModeButton;
    
    [Header("Game Modes")]
    [SerializeField] private GameModeSO easyGameModeSO;
    [SerializeField] private GameModeSO normalGameModeSO;
    [SerializeField] private GameModeSO hardGameModeSO;
    
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
}
