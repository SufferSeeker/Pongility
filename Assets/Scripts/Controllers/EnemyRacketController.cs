using UnityEngine;

public class EnemyRacketController : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Transform Ball;

    [Header("Move Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private bool CanFollow = true;

    [Header("Limits")]
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Ball = GameObject.Find("Ball").transform;
    }

    private void OnEnable()
    {
        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void Update()
    {
        if (CanFollow == false) return;

        FollowBall();
        ClampPosition();
    }

    private void OnDisable()
    {
        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }
    #endregion

    #region Event
    private void HandleMatchEnded()
    {
        CanFollow = false;
    }
    #endregion

    #region Movement
    private void FollowBall()
    {
        Vector3 CurrentPosition = transform.position;
        float TargetX = Ball.position.x;

        CurrentPosition.x = Mathf.MoveTowards(CurrentPosition.x, TargetX, MovementSpeed * Time.deltaTime);

        transform.position = CurrentPosition;
    }

    private void ClampPosition()
    {
        Vector3 CurrentPosition = transform.position;

        CurrentPosition.x = Mathf.Clamp(CurrentPosition.x, MinX, MaxX);

        transform.position = CurrentPosition;
    }
    #endregion

    #region Public Methods
    public void SetCanFollow(bool CanFollowValue)
    {
        CanFollow = CanFollowValue;
    }
    #endregion
}