using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;
    
    public enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        TimeExpired,
        GameOver
    }
    
    private State _state;
    private float _countDownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 30f;  // TODO: Change to 100 when not testing
    private float _timeExpiredTimer = 3f;
    private bool _isGamePaused;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of GameManager in your scene.");
        }
        Instance = this;
        
        _state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    private void OnDestroy()
    {
        GameInput.Instance.OnPauseAction -= GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    private void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                _countDownToStartTimer -= Time.deltaTime;
                if (_countDownToStartTimer <= 0f)
                {
                    _state = State.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer <= 0f)
                {
                    _state = State.TimeExpired;
                    Time.timeScale = 0f;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.TimeExpired:
                _timeExpiredTimer -= Time.unscaledDeltaTime;
                if (_timeExpiredTimer <= 0f)
                {
                    Time.timeScale = 1f;
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public bool IsCountDownToStartActive()
    {
        return _state == State.CountDownToStart;
    }

    public bool IsTimeExpired()
    {
        return _state == State.TimeExpired;
    }
    
    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }

    public float GetCountDownToStartTimer()
    {
        return _countDownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (_gamePlayingTimer / _gamePlayingTimerMax);
    }
    
    public void TogglePauseGame()
    {
        if (IsTimeExpired() || IsGameOver()) return;
        
        if (_isGamePaused)
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        _isGamePaused = !_isGamePaused;
    }
    
    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }
    
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (_state != State.WaitingToStart) return;
        _state = State.CountDownToStart;
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
}
