using UnityEngine;

public class EnemyRacketController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D Rigidbody;

    [Header("References")]
    [SerializeField] private Transform Ball;

    [Header("Move Settings")]
    [SerializeField] private float MovementSpeed = 5f;

    [Header("Limits")]
    [SerializeField] private float MinX;
    [SerializeField] private float MaxX;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Ball = GameObject.Find("Ball").transform;
    }

    private void FixedUpdate()
    {
        FollowBall();
        ClampPosition();
    }

    private void FollowBall()
    {
        float TargetX = Ball.position.x;
        float CurrentX = Rigidbody.position.x;

        float Difference = TargetX - CurrentX;
        float HorizontalVelocity = Difference * MovementSpeed;

        if (Rigidbody.position.x <= MinX && HorizontalVelocity < 0f)
        {
            HorizontalVelocity = 0f;
        }

        if (Rigidbody.position.x >= MaxX && HorizontalVelocity > 0f)
        {
            HorizontalVelocity = 0f;
        }

        Rigidbody.velocity = new Vector2(HorizontalVelocity, 0f);
    }

    private void ClampPosition()
    {
        Vector2 CurrentPosition = Rigidbody.position;
        CurrentPosition.x = Mathf.Clamp(CurrentPosition.x, MinX, MaxX);
        Rigidbody.position = CurrentPosition;
    }
}