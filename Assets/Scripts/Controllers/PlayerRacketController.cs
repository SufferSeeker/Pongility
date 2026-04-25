using UnityEngine;

public class PlayerRacketController : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private MatchSide ControlledSide = MatchSide.Player1;

    [Header("Move Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private float HorizontalInput;
    [SerializeField] private bool CanMove = true;

    [Header("Limits")]
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;

    private void OnEnable()
    {
        if (ControlledSide == MatchSide.Player1)
        {
            InputManager.OnPlayer1Move += HandleMoveInput;
        }

        else if (ControlledSide == MatchSide.Player2)
        {
            InputManager.OnPlayer2Move += HandleMoveInput;
        }

        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        if (ControlledSide == MatchSide.Player1)
        {
            InputManager.OnPlayer1Move -= HandleMoveInput;
        }

        else if (ControlledSide == MatchSide.Player2)
        {
            InputManager.OnPlayer2Move -= HandleMoveInput;
        }

        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }

    private void Update()
    {
        if (!CanMove) return;

        Move();
        ClampPosition();
    }

    private void HandleMoveInput(float InputValue)
    {
        HorizontalInput = InputValue;
    }

    private void HandleMatchEnded()
    {
        CanMove = false;
        HorizontalInput = 0f;
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