using UnityEngine;

public class AbilityFireball : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private string CastTriggerName = "Cast";
    [SerializeField] private string ForwardTriggerName = "Forward";
    [SerializeField] private string ExplodeTriggerName = "Explode";

    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 6f;
    [SerializeField] private Vector2 MoveDirection;

    [Header("Damage Settings")]
    [SerializeField] private int DamageAmount = 20;

    private MatchSide OwnerSide;

    private Animator FireballAnimator;
    private Collider2D FireballCollider;

    private bool CanMove;
    private bool HasImpacted;

    private void Awake()
    {
        FireballAnimator = GetComponent<Animator>();
        FireballCollider = GetComponent<Collider2D>();

        CanMove = false;
        HasImpacted = false;

        FireballCollider.enabled = false;
    }

    private void Update()
    {
        if (CanMove == false) return;

        transform.position += (Vector3)(MovementSpeed * Time.deltaTime * MoveDirection);
    }

    public void Initialize(Vector2 Direction, MatchSide NewOwnerSide, Transform CastParent)
    {
        MoveDirection = Direction.normalized;
        OwnerSide = NewOwnerSide;

        SetFireballRotation();

        transform.SetParent(CastParent, true);
        transform.localPosition = Vector3.zero;

        PlayAnimationTrigger(CastTriggerName);
    }

    private void SetFireballRotation()
    {
        if (MoveDirection == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else if (MoveDirection == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }

    public void StartForward()
    {
        if (HasImpacted == true) return;

        transform.SetParent(null, true);

        PlayAnimationTrigger(ForwardTriggerName);

        FireballCollider.enabled = true;

        CanMove = true;
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (HasImpacted == true) return;

        DamageableTarget Target = Collision.GetComponent<DamageableTarget>();

        if (Target == null) return;

        if (Target.GetTargetSide() == OwnerSide) return;

        Target.TakeDamage(DamageAmount, OwnerSide);

        StartImpact();
    }

    private void StartImpact()
    {
        HasImpacted = true;
        CanMove = false;

        FireballCollider.enabled = false;

        PlayAnimationTrigger(ExplodeTriggerName);
    }

    public void DestroyFireball()
    {
        Destroy(gameObject);
    }

    private void PlayAnimationTrigger(string TriggerName)
    {
        FireballAnimator.SetTrigger(TriggerName);
    }
}