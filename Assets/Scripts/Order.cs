using System;
using UnityEngine;

public class Order : MonoBehaviour, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnExpired;
    public event EventHandler OnDestroyed;
    
    private RecipeSO _recipeSO;
    private float _timer;

    private void Start()
    {
        _timer = _recipeSO.countdownMax;
    }

    private void Update()
    {
        if (_timer <= 0) return;
        
        _timer -= Time.deltaTime;
        
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            NormalizedProgress = _timer / _recipeSO.countdownMax
        });
        
        if (_timer <= 0)
        {
            OnExpired?.Invoke(this, EventArgs.Empty);
        }
    }

    public RecipeSO GetRecipeSO()
    {
        return _recipeSO;
    }

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        if (_recipeSO == null)
            _recipeSO = recipeSO;
    }

    public void DestroySelf()
    {
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
