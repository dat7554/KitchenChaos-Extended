using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }
    
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than 1 instance of GameManager in your scene.");
        }
        Instance = this;
        
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
    }

    private void Start()
    {
        _playerInputActions.Player.Interact.performed += Interact_Performed;
        _playerInputActions.Player.InteractAlternate.performed += InteractAlternate_Performed;
        _playerInputActions.Player.Pause.performed += Pause_OnPerformed;
    }

    private void OnDestroy()
    {
        _playerInputActions.Player.Interact.performed -= Interact_Performed;
        _playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_Performed;
        _playerInputActions.Player.Pause.performed -= Pause_OnPerformed;
        
        _playerInputActions.Dispose();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }
    
    private void Interact_Performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void InteractAlternate_Performed(InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void Pause_OnPerformed(InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }
}