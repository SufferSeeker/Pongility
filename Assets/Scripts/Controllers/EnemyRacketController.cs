using UnityEngine;

public class EnemyRacketController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform Ball;

    [Header("Move Settings")]
    [SerializeField] private float MovementSpeed = 5f;
    [SerializeField] private bool CanFollow = true;

    [Header("Limits")]
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;

    private void OnEnable()
    {
        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }
    
    private void Start()
    {
        Ball = GameObject.Find("Ball").transform;
    }

    private void Update()
    {
        if (!CanFollow) return;

        FollowBall();
        ClampPosition();
    }

    private void HandleMatchEnded()
    {
        CanFollow = false;
    }

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
}