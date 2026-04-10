using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float BallSpeed = 4f;
    [SerializeField] private Vector2 MoveDirection;

    [Header("Reset Settings")]
    [SerializeField] private float RestartDelay = 1f;
    [SerializeField] private Vector3 StartPosition;
    [SerializeField] private bool IsMatchFinished;

    [Header("Clamp Settings")]
    [SerializeField] private bool UseHorizontalClamp = true;
    [SerializeField] private float MinX = -3.5f;
    [SerializeField] private float MaxX = 3.5f;
    [SerializeField] private float GizmoHeight = 12f;

    private void OnEnable()
    {
        GoalZone.OnGoalScored += HandleGoalScored;
        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        GoalZone.OnGoalScored -= HandleGoalScored;
        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }

    private void Awake()
    {
        StartPosition = transform.position;
        IsMatchFinished = false;
    }

    private void Start()
    {
        StartCoroutine(StartBallRoutine());
    }

    private void Update()
    {
        Move();
    }

    private void HandleGoalScored(MatchSide ScoringSide)
    {
        if (IsMatchFinished)
        {
            return;
        }

        StartCoroutine(ResetBallRoutine());
    }

    private void HandleMatchEnded()
    {
        IsMatchFinished = true;
        StopAllCoroutines();
        MoveDirection = Vector2.zero;
    }

    private IEnumerator StartBallRoutine()
    {
        MoveDirection = Vector2.zero;

        yield return new WaitForSeconds(RestartDelay);

        SetRandomStartDirection();
    }

    private IEnumerator ResetBallRoutine()
    {
        MoveDirection = Vector2.zero;
        transform.position = StartPosition;

        yield return new WaitForSeconds(RestartDelay);

        SetRandomStartDirection();
    }

    private void SetRandomStartDirection()
    {
        float RandomDirection = Random.value;

        if (RandomDirection < 0.5f)
        {
            MoveDirection = Vector2.up.normalized;
        }
        else
        {
            MoveDirection = Vector2.down.normalized;
        }
    }

    private void Move()
    {
        Vector3 Movement = (Vector3)(MoveDirection * BallSpeed * Time.deltaTime);
        transform.position += Movement;

        ClampBallPosition();
    }

    private void ClampBallPosition()
    {
        if (UseHorizontalClamp == false)
        {
            return;
        }

        Vector3 CurrentPosition = transform.position;

        if (CurrentPosition.x < MinX)
        {
            CurrentPosition.x = MinX;
            transform.position = CurrentPosition;

            if (MoveDirection.x < 0f)
            {
                MoveDirection = new Vector2(-MoveDirection.x, MoveDirection.y).normalized;
            }
        }

        else if (CurrentPosition.x > MaxX)
        {
            CurrentPosition.x = MaxX;
            transform.position = CurrentPosition;

            if (MoveDirection.x > 0f)
            {
                MoveDirection = new Vector2(-MoveDirection.x, MoveDirection.y).normalized;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        RacketSideIdentifier RacketSideIdentifier = Collision.gameObject.GetComponent<RacketSideIdentifier>();

        if (RacketSideIdentifier != null)
        {
            float HitOffset = transform.position.x - Collision.transform.position.x;
            MatchSide RacketSide = RacketSideIdentifier.GetRacketSide();

            if (RacketSide == MatchSide.Player1)
            {
                Vector2 NewDirection = new Vector2(HitOffset, 1f).normalized;
                MoveDirection = NewDirection;
            }
            else if (RacketSide == MatchSide.Player2)
            {
                Vector2 NewDirection = new Vector2(HitOffset, -1f).normalized;
                MoveDirection = NewDirection;
            }

            return;
        }

        if (Collision.gameObject.CompareTag("Wall"))
        {
            MoveDirection = new Vector2(-MoveDirection.x, MoveDirection.y).normalized;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (UseHorizontalClamp == false)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        Vector3 LeftTop = new Vector3(MinX, transform.position.y + GizmoHeight * 0.5f, 0f);
        Vector3 LeftBottom = new Vector3(MinX, transform.position.y - GizmoHeight * 0.5f, 0f);

        Vector3 RightTop = new Vector3(MaxX, transform.position.y + GizmoHeight * 0.5f, 0f);
        Vector3 RightBottom = new Vector3(MaxX, transform.position.y - GizmoHeight * 0.5f, 0f);

        Gizmos.DrawLine(LeftTop, LeftBottom);
        Gizmos.DrawLine(RightTop, RightBottom);
    }
}