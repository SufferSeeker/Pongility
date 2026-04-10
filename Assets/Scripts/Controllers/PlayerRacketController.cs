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

//using System.Collections;
//using UnityEngine;

//public class BallController : MonoBehaviour
//{
//    [SerializeField] private Rigidbody2D Rigidbody2D;

//    [Header("Ball Settings")]
//    [SerializeField] private float BallSpeed = 4f;
//    [SerializeField] private Vector2 MoveDirection;

//    [Header("Reset Settings")]
//    [SerializeField] private float RestartDelay = 1f;
//    [SerializeField] private Vector3 StartPosition;
//    [SerializeField] private bool IsMatchFinished;

//    private void OnEnable()
//    {
//        GoalZone.OnGoalScored += HandleGoalScored;
//        MatchManager.OnMatchEnded += HandleMatchEnded;
//    }

//    private void OnDisable()
//    {
//        GoalZone.OnGoalScored -= HandleGoalScored;
//        MatchManager.OnMatchEnded -= HandleMatchEnded;
//    }

//    private void Awake()
//    {
//        Rigidbody2D = GetComponent<Rigidbody2D>();

//        StartPosition = transform.position;

//        IsMatchFinished = false;
//    }

//    private void Start()
//    {
//        StartCoroutine(StartBallRoutine());
//    }

//    private void FixedUpdate()
//    {
//        Move();
//    }

//    private void HandleGoalScored(MatchSide ScoringSide)
//    {
//        if (IsMatchFinished) return;

//        StartCoroutine(ResetBallRoutine());
//    }

//    private void HandleMatchEnded()
//    {
//        Rigidbody2D.velocity = Vector2.zero;
//        MoveDirection = Vector2.zero;
//        IsMatchFinished = true;
//        StopAllCoroutines();
//    }

//    private IEnumerator StartBallRoutine()
//    {
//        Rigidbody2D.velocity = Vector2.zero;
//        MoveDirection = Vector2.zero;

//        yield return new WaitForSeconds(RestartDelay);

//        SetRandomStartDirection();
//    }

//    private IEnumerator ResetBallRoutine()
//    {
//        Rigidbody2D.velocity = Vector2.zero;
//        MoveDirection = Vector2.zero;
//        transform.position = StartPosition;

//        yield return new WaitForSeconds(RestartDelay);

//        SetRandomStartDirection();
//    }

//    private void SetRandomStartDirection()
//    {
//        float RandomDirection = Random.value;

//        if (RandomDirection < 0.5f)
//        {
//            MoveDirection = Vector2.up.normalized;
//        }

//        else
//        {
//            MoveDirection = Vector2.down.normalized;
//        }
//    }

//    private void Move()
//    {
//        Vector2 NewPosition = Rigidbody2D.position + MoveDirection * BallSpeed * Time.fixedDeltaTime;
//        Rigidbody2D.MovePosition(NewPosition);
//    }

//    private void OnCollisionEnter2D(Collision2D Collision)
//    {
//        RacketSideIdentifier RacketSideIdentifier = Collision.gameObject.GetComponent<RacketSideIdentifier>();

//        if (RacketSideIdentifier != null)
//        {
//            float HitOffset = transform.position.x - Collision.transform.position.x;
//            MatchSide RacketSide = RacketSideIdentifier.GetRacketSide();

//            if (RacketSide == MatchSide.Player1)
//            {
//                Vector2 NewDirection = new Vector2(HitOffset, 1f).normalized;
//                MoveDirection = NewDirection;
//            }

//            else if (RacketSide == MatchSide.Player2)
//            {
//                Vector2 NewDirection = new Vector2(HitOffset, -1f).normalized;
//                MoveDirection = NewDirection;
//            }

//            return;
//        }

//        if (Collision.gameObject.CompareTag("Wall"))
//        {
//            MoveDirection = new Vector2(-MoveDirection.x, MoveDirection.y).normalized;
//        }
//    }
//}