using System;
using TMPro;
using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    private static readonly int Popup = Animator.StringToHash("Popup");
    
    [SerializeField] private TextMeshProUGUI _gameEndText;
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        
        Hide();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnStateChanged -= GameManager_OnStateChanged;
    }
    
    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsTimeExpired())
        {
            Show();
            _animator.SetTrigger(Popup);
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        _gameEndText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        _gameEndText.gameObject.SetActive(false);
    }
}
