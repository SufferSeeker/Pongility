using UnityEngine;

public class BallController : MonoBehaviour
{
    #region Variables
    [Header("Core References")]
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;

    [Header("Ball Settings")]
    [SerializeField] private float BallSpeed = 4f;

    [Header("Dynamic Speed Settings")]
    [SerializeField] private float DynamicSpeedStartDelay = 30f;
    [SerializeField] private float DynamicSpeedDuration = 60f;
    [SerializeField] private float DynamicMaxSpeed = 9f;

    [Header("Reset Settings")]
    [SerializeField] private Vector3 StartPosition;

    [Header("Clamp Settings")]
    [SerializeField] private bool UseHorizontalClamp = true;
    [SerializeField] private float MinX = -3.5f;
    [SerializeField] private float MaxX = 3.5f;
    [SerializeField] private float GizmoHeight = 12f;

    [Header("Runtime State")]
    [SerializeField] private Vector2 MoveDirection;
    [SerializeField] private MatchSide LastHitSide = MatchSide.None;
    [SerializeField] private bool IsMatchFinished;
    [SerializeField] private float DefaultBallSpeed;
    [SerializeField] private float CurrentBallSpeed;
    [SerializeField] private float DynamicSpeedTimer;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        SelectedMatchSettings = FindFirstObjectByType<SelectedMatchSettings>();

        StartPosition = transform.position;
        IsMatchFinished = false;

        DefaultBallSpeed = BallSpeed;
        CurrentBallSpeed = DefaultBallSpeed;
        DynamicSpeedTimer = 0f;
    }

    private void OnEnable()
    {
        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void Update()
    {
        UpdateDynamicBallSpeed();

        Move();
    }

    private void OnDisable()
    {
        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }
    #endregion

    #region Round Control
    public MatchSide GetLastHitSide()
    {
        return LastHitSide;
    }

    public void StopBallForRound()
    {
        if (IsMatchFinished == true) return;

        StopBallMovement();
        ResetDynamicSpeedProgress();
    }

    public void ResetBallForRound()
    {
        if (IsMatchFinished == true) return;

        StopBallMovement();
        ResetDynamicSpeedProgress();

        transform.position = StartPosition;
        LastHitSide = MatchSide.None;
    }

    public void LaunchBall()
    {
        if (IsMatchFinished == true) return;

        SetRandomStartDirection();
    }

    private void HandleMatchEnded()
    {
        IsMatchFinished = true;
        StopBallMovement();
    }

    private void StopBallMovement()
    {
        StopAllCoroutines();
        MoveDirection = Vector2.zero;
    }
    #endregion

    #region Movement
    private void Move()
    {
        float MovementAmount = CurrentBallSpeed * Time.deltaTime;

        Vector3 Movement = (Vector3)(MoveDirection * MovementAmount);

        transform.position = transform.position + Movement;

        ClampBallPosition();
    }

    private void SetRandomStartDirection()
    {
        float RandomDirection = Random.value;

        if (RandomDirection < 0.5f)
        {
            MoveDirection = Vector2.up;
        }

        else
        {
            MoveDirection = Vector2.down;
        }
    }

    private void ClampBallPosition()
    {
        if (UseHorizontalClamp == false) return;   

        Vector3 CurrentPosition = transform.position;

        if (CurrentPosition.x < MinX)
        {
            CurrentPosition.x = MinX;
            transform.position = CurrentPosition;

            if (MoveDirection.x < 0f)
            {
                BounceHorizontally();
            }

            return;
        }

        if (CurrentPosition.x > MaxX)
        {
            CurrentPosition.x = MaxX;
            transform.position = CurrentPosition;

            if (MoveDirection.x > 0f)
            {
                BounceHorizontally();
            }
        }
    }

    private void BounceHorizontally()
    {
        MoveDirection = new Vector2(-MoveDirection.x, MoveDirection.y).normalized;
    }
    #endregion

    #region Dynamic Speed
    private void UpdateDynamicBallSpeed()
    {
        if (SelectedMatchSettings == null) return;
        if (SelectedMatchSettings.BallSpeedMode != BallSpeedMode.Dynamic) return;
        if (MoveDirection == Vector2.zero) return;
        if (IsMatchFinished == true) return;

        DynamicSpeedTimer += Time.deltaTime;

        if (DynamicSpeedTimer <= DynamicSpeedStartDelay)
        {
            CurrentBallSpeed = DefaultBallSpeed;
            return;
        }

        float AccelerationTime = DynamicSpeedTimer - DynamicSpeedStartDelay;
        float AccelerationProgress = AccelerationTime / DynamicSpeedDuration;

        if (AccelerationProgress > 1f)
        {
            AccelerationProgress = 1f;
        }

        CurrentBallSpeed = Mathf.Lerp(DefaultBallSpeed, DynamicMaxSpeed, AccelerationProgress);
    }

    private void ResetDynamicSpeedProgress()
    {
        DynamicSpeedTimer = 0f;
        CurrentBallSpeed = DefaultBallSpeed;
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (HandleRacketCollision(Collision) == true) return;

        if (HandleWallCollision(Collision) == true) return;
    }

    private bool HandleRacketCollision(Collision2D Collision)
    {
        RacketSideIdentifier RacketSideIdentifier = Collision.gameObject.GetComponent<RacketSideIdentifier>();

        if (RacketSideIdentifier == null) return false;
        
        float HitOffset = transform.position.x - Collision.transform.position.x;
        MatchSide RacketSide = RacketSideIdentifier.GetRacketSide();
        
        LastHitSide = RacketSide;

        if (RacketSide == MatchSide.Player1)
        {
            MoveDirection = new Vector2(HitOffset, 1f).normalized;
        }

        else if (RacketSide == MatchSide.Player2)
        {
            MoveDirection = new Vector2(HitOffset, -1f).normalized;
        }

        return true;
    }

    private bool HandleWallCollision(Collision2D Collision)
    {
        if (Collision.gameObject.CompareTag("Wall") == false) return false;
        
        BounceHorizontally();

        return true;
    }
    #endregion

    #region Gizmos
    private void OnDrawGizmosSelected()
    {
        if (UseHorizontalClamp == false) return;

        Gizmos.color = Color.yellow;

        Vector3 LeftTop = new Vector3(MinX, transform.position.y + GizmoHeight * 0.5f, 0f);
        Vector3 LeftBottom = new Vector3(MinX, transform.position.y - GizmoHeight * 0.5f, 0f);

        Vector3 RightTop = new Vector3(MaxX, transform.position.y + GizmoHeight * 0.5f, 0f);
        Vector3 RightBottom = new Vector3(MaxX, transform.position.y - GizmoHeight * 0.5f, 0f);

        Gizmos.DrawLine(LeftTop, LeftBottom);
        Gizmos.DrawLine(RightTop, RightBottom);
    }
    #endregion
}