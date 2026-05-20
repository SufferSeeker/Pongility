using UnityEngine;

public class AbilityFireball : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private Animator FireballAnimator;
    [SerializeField] private Collider2D FireballCollider;

    [Header("Animation Settings")]
    [SerializeField] private string CastTriggerName = "Cast";
    [SerializeField] private string ForwardTriggerName = "Forward";
    [SerializeField] private string ExplodeTriggerName = "Explode";

    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 6f;
    [SerializeField] private Vector2 MoveDirection;

    [Header("Damage Settings")]
    [SerializeField] private int DamageAmount = 20;

    [Header("State")]
    [SerializeField] private MatchSide OwnerSide;
    [SerializeField] private bool CanMove;
    [SerializeField] private bool HasImpacted;
    [SerializeField] private bool IsFrozenByRoundEnd;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        FireballAnimator = GetComponent<Animator>();
        FireballCollider = GetComponent<Collider2D>();

        CanMove = false;
        HasImpacted = false;
        IsFrozenByRoundEnd = false;

        FireballCollider.enabled = false;
    }

    private void OnEnable()
    {
        MatchManager.OnRoundEndFreezeStarted += FreezeForRoundEnd;
        MatchManager.OnMatchEnded += FreezeForRoundEnd;
    }

    private void Update()
    {
        if (CanMove == false) return;

        MoveFireball();
    }

    private void OnDisable()
    {
        MatchManager.OnRoundEndFreezeStarted -= FreezeForRoundEnd;
        MatchManager.OnMatchEnded -= FreezeForRoundEnd;
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (HasImpacted == true) return;
        if (IsFrozenByRoundEnd == true) return;

        DamageableTarget Target = Collision.GetComponent<DamageableTarget>();

        if (CanDamageTarget(Target) == false) return;

        Target.TakeDamage(DamageAmount, OwnerSide);

        StartImpact();
    }
    #endregion

    #region Initialization
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
        if (MoveDirection.y > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        else if (MoveDirection.y < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }
    #endregion

    #region Movement
    private void MoveFireball()
    {
        transform.position += (Vector3)(MovementSpeed * Time.deltaTime * MoveDirection);
    }
    #endregion

    #region Impact
    private bool CanDamageTarget(DamageableTarget Target)
    {
        if (Target == null) return false;

        if (Target.GetTargetSide() == OwnerSide) return false;

        return true;
    }

    private void StartImpact()
    {
        HasImpacted = true;
        CanMove = false;

        FireballCollider.enabled = false;

        PlayAnimationTrigger(ExplodeTriggerName);
    }
    #endregion

    #region Round Events
    private void FreezeForRoundEnd()
    {
        if (IsFrozenByRoundEnd == true) return;

        IsFrozenByRoundEnd = true;
        CanMove = false;

        FireballCollider.enabled = false;

        FireballAnimator.speed = 0f;
    }
    #endregion

    #region Animation Event Methods
    public void StartForward()
    {
        if (HasImpacted == true) return;
        if (IsFrozenByRoundEnd == true) return;

        transform.SetParent(null, true);

        PlayAnimationTrigger(ForwardTriggerName);

        FireballCollider.enabled = true;

        CanMove = true;
    }

    public void DestroyFireball()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Helper Methods
    private void PlayAnimationTrigger(string TriggerName)
    {
        FireballAnimator.SetTrigger(TriggerName);
    }
    #endregion
}