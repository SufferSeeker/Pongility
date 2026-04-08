using UnityEngine;

public class EnemyRacketController1 : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform Ball;

    [Header("Move Settings")]
    [SerializeField] private float MovementSpeed = 5f;

    [Header("Limits")]
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;

    private void Start()
    {
        Ball = GameObject.Find("Ball").transform;
    }

    private void Update()
    {
        FollowBall();
        ClampPosition();
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