using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeedFloat = 5f;
    [SerializeField] private GameInput gameInput;

    public bool IsWalking { get; private set; }

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        transform.position += moveDirection * (moveSpeedFloat * Time.deltaTime);
        
        IsWalking = moveDirection.magnitude > 0.1f;
        
        const float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }
}