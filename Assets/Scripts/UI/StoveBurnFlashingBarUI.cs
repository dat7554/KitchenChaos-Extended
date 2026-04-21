using System;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    private static readonly int IsFlashing = Animator.StringToHash("IsFlashing");
    
    [SerializeField] private StoveCounter stoveCounter;

    private Animator _animator;

    private void Awake()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void OnDestroy()
    {
        stoveCounter.OnProgressChanged -= StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = stoveCounter.IsFried() && e.NormalizedProgress >= burnShowProgressAmount;

        _animator.SetBool(IsFlashing, show);
    }
}
