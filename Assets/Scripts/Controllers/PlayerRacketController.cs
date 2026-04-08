using UnityEngine;

public class PlayerRacketController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float HorizontalInput;

    [Header("Limits")]
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;

    private void OnEnable()
    {
        InputManager.OnMove += HandleMoveInput;
    }

    private void OnDisable()
    {
        InputManager.OnMove -= HandleMoveInput;
    }

    private void Update()
    {
        Move();
        ClampPosition();
    }

    private void HandleMoveInput(float InputValue)
    {
        HorizontalInput = InputValue;
    }

    private void Move()
    {
        Vector3 CurrentPosition = transform.position;
        float HorizontalMovement = HorizontalInput * MovementSpeed * Time.deltaTime;

        CurrentPosition.x += HorizontalMovement;
        transform.position = CurrentPosition;
    }

    private void ClampPosition()
    {
        Vector3 CurrentPosition = transform.position;
        CurrentPosition.x = Mathf.Clamp(CurrentPosition.x, MinX, MaxX);
        transform.position = CurrentPosition;
    }
}