using UnityEngine;

public class PlayerRacketController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D Rigidbody;

    [Header("Move Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float HorizontalInput;

    private void OnEnable()
    {
        InputManager.OnMove += HandleMoveInput;
    }

    private void OnDisable()
    {
        InputManager.OnMove -= HandleMoveInput;
    }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Rigidbody.velocity = new Vector2 (HorizontalInput * MovementSpeed, 0f);
    }

    private void HandleMoveInput(float InputValue)
    {
        HorizontalInput = InputValue;
    }
}