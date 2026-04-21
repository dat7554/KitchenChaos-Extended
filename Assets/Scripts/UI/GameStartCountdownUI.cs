using System;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private static readonly int NumberPopup = Animator.StringToHash("NumberPopup");
    
    [SerializeField] private TextMeshProUGUI countdownText;
    
    private Animator _animator;
    private int _previousCountdownNumber;

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

    private void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountDownToStartTimer());
        
        if (countdownNumber != _previousCountdownNumber)
        {
            countdownText.text = countdownNumber.ToString();
            _previousCountdownNumber = countdownNumber;
            _animator.SetTrigger(NumberPopup);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountDownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        countdownText.gameObject.SetActive(true);
    }

    private void Hide()
    {
        countdownText.gameObject.SetActive(false);
    }
}
