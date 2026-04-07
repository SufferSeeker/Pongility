using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D Rigidbody;

    [Header("Ball Settings")]
    [SerializeField] private float BallSpeed = 4f;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitialMovement();
    }

    private void FixedUpdate()
    {

    }

    private void InitialMovement()
    {
        Rigidbody.velocity = Vector2.down.normalized * BallSpeed;
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.CompareTag("Player"))
        {
            float HitOffset = (transform.position.x - Collision.transform.position.x);

            Vector2 NewDirection = new Vector2(HitOffset, 1f).normalized;

            Rigidbody.velocity = NewDirection * BallSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.gameObject.CompareTag("Wall"))
        {
            Vector2 CurrentVelocity = Rigidbody.velocity;

            Rigidbody.velocity = new Vector2(-CurrentVelocity.x, CurrentVelocity.y);
        }
    }
}