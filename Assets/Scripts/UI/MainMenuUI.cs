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
            Hide();
        });
        
        quitButton.onClick.AddListener(Application.Quit);
        
        Time.timeScale = 1f;
    }

    private void Start()
    {
        if (SceneLoader.Instance.IsSkipToShopEnable)
            Hide();
        else
            Show();
    }

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);
}
