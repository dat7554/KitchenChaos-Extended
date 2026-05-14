using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Update()
    {
        float timer = Mathf.Max(0f, GameManager.Instance.GetGamePlayingTimer());
        
        timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
        
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
