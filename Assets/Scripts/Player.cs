using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickupSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    
    [SerializeField] private Transform kitchenObjectHoldPoint;

    public bool IsWalking { get; private set; }
    
    private Vector3 _lastDirection;
    private BaseCounter _selectedCounter;
    
    private KitchenObject _kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of Player in your scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void OnDisable()
    {
        gameInput.OnInteractAction -= GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction -= GameInput_OnInteractAlternateAction;
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    
    public Transform GetCounterTopPoint()
    {
        return kitchenObjectHoldPoint;
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
            OnPickupSomething?.Invoke(this, EventArgs.Empty);
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
    
    private void GameInput_OnInteractAction(object sender, EventArgs eventArgs)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
    }
    
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs eventArgs)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = 0.7f;
        float playerHeight = 2f;
        Vector3 playerHeightPosition = transform.position + Vector3.up * playerHeight;
        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast
        (
            transform.position, 
            playerHeightPosition, 
            playerRadius, 
            moveDirection,
            moveDistance
        );

        if (!canMove)
        {
            Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
            canMove = moveDirection.x != 0f && !Physics.CapsuleCast
            (
                transform.position, 
                playerHeightPosition, 
                playerRadius, 
                moveDirectionX,
                moveDistance
            );

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
                canMove = moveDirection.z != 0f && !Physics.CapsuleCast
                (
                    transform.position, 
                    playerHeightPosition, 
                    playerRadius, 
                    moveDirectionZ,
                    moveDistance
                );
                
                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
            }
        }
        
        if (canMove)
        {
            transform.position += moveDirection * moveDistance;
        }
        
        IsWalking = moveDirection.magnitude > 0.1f;
        
        const float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        
        if (moveDirection != Vector3.zero)
            _lastDirection = moveDirection;
        
        float interactDistance = 2f;
        bool isBlockedByCounter = Physics.Raycast
            (
                transform.position, 
                _lastDirection, 
                out RaycastHit hit, 
                interactDistance, 
                counterLayerMask
            );
        
        if (isBlockedByCounter)
        {
            if (hit.collider.TryGetComponent(out BaseCounter counter))
            {
                if (_selectedCounter != counter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter counter)
    {
        _selectedCounter = counter;
        var eventArgs = new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = _selectedCounter
        };
        OnSelectedCounterChanged?.Invoke(this, eventArgs);
    }
}