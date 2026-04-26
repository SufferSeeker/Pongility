using UnityEngine;

public class AbilityFireball : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 6f;
    [SerializeField] private Vector2 MoveDirection;

    [Header("Damage Settings")]
    [SerializeField] private int DamageAmount = 20;

    private MatchSide OwnerSide;

    private void Update()
    {
        transform.Translate(MoveDirection * MovementSpeed * Time.deltaTime);
    }

    public void Initialize(Vector2 Direction, MatchSide NewOwnerSide)
    {
        MoveDirection = Direction.normalized;
        OwnerSide = NewOwnerSide;
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        DamageableTarget Target = Collision.GetComponent<DamageableTarget>();

        if (Target == null)
        {
            return;
        }

        if (Target.GetTargetSide() == OwnerSide)
        {
            return;
        }

        Target.TakeDamage(DamageAmount, OwnerSide);

        Destroy(gameObject);
    }
}