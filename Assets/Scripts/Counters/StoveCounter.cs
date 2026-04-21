using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }
    
    [SerializeField] private FryingRecipeSO[] _fryingRecipesSOArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipesSOArray;
    
    private State _state;
    private FryingRecipeSO _fryingRecipeSO;
    private float _fryingTimer;
    private BurningRecipeSO _burningRecipeSO;
    private float _burningTimer;

    private void Start()
    {
        _state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    _fryingTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        NormalizedProgress = _fryingTimer / _fryingRecipeSO.fryingTimerMax
                    });
                    
                    if (_fryingTimer > _fryingRecipeSO.fryingTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_fryingRecipeSO.output, this);
                        
                        _burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
                        _burningTimer = 0f;
                        
                        _state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { state = _state });
                    }
                    break;
                case State.Fried:
                    _burningTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        NormalizedProgress = _burningTimer / _burningRecipeSO.burningTimerMax
                    });
                    
                    if (_burningTimer > _burningRecipeSO.burningTimerMax)
                    {
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(_burningRecipeSO.output, this);
                        
                        _state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { state = _state });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            NormalizedProgress = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject() && player.HasKitchenObject())
        {
            if (HasRecipeWithInput(player.GetKitchenObject().KitchenObjectSO))
            {
                player.GetKitchenObject().SetParent(this);
                
                _fryingTimer = 0f;
                _fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().KitchenObjectSO);
                
                _state = State.Frying;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { state = _state });
            }
        }
        else if (HasKitchenObject() && !player.HasKitchenObject())
        {
            GetKitchenObject().SetParent(player);
            
            _state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { state = _state });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                NormalizedProgress = 0f
            });
        }
        else if (HasKitchenObject() && player.HasKitchenObject())
        {
            if (!player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) return;
            if (!plateKitchenObject.TryAddIngredient(GetKitchenObject().KitchenObjectSO)) return;
            GetKitchenObject().DestroySelf();
                
            _state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs() { state = _state });
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                NormalizedProgress = 0f
            });
        }
    }

    public bool IsFried()
    {
        return _state == State.Fried;
    }
    
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputKitchenObject(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null)
        {
            return fryingRecipeSO.output;
        }

        return null;
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in _fryingRecipesSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }
        
        return null;
    }
    
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in _burningRecipesSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }
        
        return null;
    }
}
