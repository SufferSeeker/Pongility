using UnityEngine;

public class BallController1 : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private float BallSpeed = 4f;
    [SerializeField] private Vector2 MoveDirection;

    private void Start()
    {
        InitialMovement();
    }

    private void Update()
    {
        Move();
    }

    private void InitialMovement()
    {
        MoveDirection = Vector2.down.normalized;
    }

    private void Move()
    {
        Vector3 Movement = (Vector3)(MoveDirection * BallSpeed * Time.deltaTime);
        transform.position += Movement;
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.CompareTag("Player"))
        {
            float HitOffset = transform.position.x - Collision.transform.position.x;
            Vector2 NewDirection = new Vector2(HitOffset, 1f).normalized;

            MoveDirection = NewDirection;
        }

        if (Collision.gameObject.CompareTag("Enemy"))
        {
            float HitOffset = transform.position.x - Collision.transform.position.x;
            Vector2 NewDirection = new Vector2(HitOffset, -1f).normalized;

            MoveDirection = NewDirection;
        }

        if (Collision.gameObject.CompareTag("Wall"))
        {
            MoveDirection = new Vector2(-MoveDirection.x, MoveDirection.y).normalized;
        }
    }
}