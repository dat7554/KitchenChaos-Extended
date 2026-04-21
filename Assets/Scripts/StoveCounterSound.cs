using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource _audioSource;
    private bool _playWarningSound;
    private float _warningSoundTimer;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void OnDestroy()
    {
        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged -= StoveCounter_OnProgressChanged;
    }

    private void Update()
    {
        if (_playWarningSound)
        {
            _warningSoundTimer -= Time.deltaTime;
            if (_warningSoundTimer <= 0f)
            {
                float warningSoundTimerMax = 0.2f;
                _warningSoundTimer = warningSoundTimerMax;
                
                SoundManager.Instance.PlayWarninigSound(stoveCounter.transform.position);
            }
        }
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }
    
    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f; 
        _playWarningSound = stoveCounter.IsFried() && e.NormalizedProgress >= burnShowProgressAmount;
    }
}
