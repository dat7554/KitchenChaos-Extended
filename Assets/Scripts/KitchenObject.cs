using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    
    public KitchenObjectSO KitchenObjectSO => kitchenObjectSO;
    
    private IKitchenObjectParent _parent;

    public IKitchenObjectParent GetParent()
    {
        return _parent;
    }

    public void SetParent(IKitchenObjectParent parent)
    {
        if (_parent != null)
        {
            _parent.ClearKitchenObject();
        }
        
        _parent = parent;
        if (parent.HasKitchenObject())
        {
            Debug.LogError("Parent already has a KitchenObject");
        }
        parent.SetKitchenObject(this);
        
        transform.parent = parent.GetCounterTopPoint();
        transform.localPosition = Vector3.zero;
    }

    public void DestroySelf()
    {
        _parent.ClearKitchenObject();
        Destroy(gameObject);
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        Transform objectTransform = Instantiate(kitchenObjectSO.Prefab);
        KitchenObject kitchenObject = objectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetParent(parent);
        return kitchenObject;
    }
}
