using System;
using UnityEngine;

public abstract class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;
    
    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }
    
    [SerializeField] private Transform counterTopPoint;
    
    private KitchenObject _kitchenObject;
    
    public abstract void Interact(Player player);

    public virtual void InteractAlternate(Player player) {}
    
    public Transform GetCounterTopPoint()
    {
        return counterTopPoint;
    }

    public KitchenObject GetKitchenObject()
    {
        return _kitchenObject;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (_kitchenObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return _kitchenObject != null;
    }
}
