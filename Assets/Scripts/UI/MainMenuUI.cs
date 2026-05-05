using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public event EventHandler OnPlayButtonClicked;
    
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            OnPlayButtonClicked?.Invoke(this, EventArgs.Empty);
        });
        
        quitButton.onClick.AddListener(Application.Quit);
        
        Time.timeScale = 1f;
    }
}
