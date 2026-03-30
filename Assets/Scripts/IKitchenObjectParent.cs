using UnityEngine;

public interface IKitchenObjectParent
{
    Transform GetCounterTopPoint();
    
    KitchenObject GetKitchenObject();
    
    void SetKitchenObject(KitchenObject kitchenObject);

    void ClearKitchenObject();

    bool HasKitchenObject();
}
