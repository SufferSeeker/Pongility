using UnityEngine;

public class AbilityPoison : MonoBehaviour, IUsableAbility, IDeflectable
{
    #region Variables
    [Header("References")]
    [SerializeField] private Animator PoisonAnimator;
    [SerializeField] private Collider2D PoisonCollider;

    [Header("Status Effect Settings")]
    [SerializeField] private StatusEffectDefinition PoisonStatusEffectDefinition;

    [Header("Animation Settings")]
    [SerializeField] private string CastTriggerName = "Cast";
    [SerializeField] private string ForwardTriggerName = "Forward";
    [SerializeField] private string ExplodeTriggerName = "Explode";

    [Header("Movement Settings")]
    [SerializeField] private float MovementSpeed = 6f;
    [SerializeField] private Vector2 MoveDirection;

    [Header("State")]
    [SerializeField] private MatchSide OwnerSide;
    [SerializeField] private bool CanMove;
    [SerializeField] private bool HasImpacted;
    [SerializeField] private bool IsFrozenByRoundEnd;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        PoisonAnimator = GetComponent<Animator>();
        PoisonCollider = GetComponent<Collider2D>();

        CanMove = false;
        HasImpacted = false;
        IsFrozenByRoundEnd = false;

        PoisonCollider.enabled = false;
    }

    private void OnEnable()
    {
        MatchManager.OnRoundEndFreezeStarted += FreezeForRoundEnd;
        MatchManager.OnMatchEnded += FreezeForRoundEnd;
    }

    private void Update()
    {
        if (CanMove == false) return;

        MovePoison();
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

        if (CanApplyPoison(Target) == false) return;

        StatusEffectReceiver TargetStatusEffectReceiver = Collision.GetComponent<StatusEffectReceiver>();

        if (TargetStatusEffectReceiver == null) return;

        StartImpact();

        TargetStatusEffectReceiver.ApplyStatusEffect(PoisonStatusEffectDefinition, OwnerSide);
    }
    #endregion

    #region Initialization
    public void Initialize(Vector2 Direction, MatchSide NewOwnerSide, Transform CastParent)
    {
        MoveDirection = Direction.normalized;
        OwnerSide = NewOwnerSide;

        SetPoisonRotation();

        transform.SetParent(CastParent, true);
        transform.localPosition = Vector3.zero;

        PlayAnimationTrigger(CastTriggerName);
    }

    private void SetPoisonRotation()
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
    private void MovePoison()
    {
        transform.position += (Vector3)(MovementSpeed * Time.deltaTime * MoveDirection);
    }
    #endregion

    #region Impact
    private bool CanApplyPoison(DamageableTarget Target)
    {
        if (Target == null) return false;

        if (Target.GetTargetSide() == OwnerSide) return false;

        return true;
    }

    private void StartImpact()
    {
        HasImpacted = true;
        CanMove = false;

        PoisonCollider.enabled = false;

        PlayAnimationTrigger(ExplodeTriggerName);
    }
    #endregion

    #region Deflect
    public MatchSide GetOwnerSide()
    {
        return OwnerSide;
    }

    public void Deflect(MatchSide NewOwnerSide)
    {
        if (HasImpacted == true) return;
        if (IsFrozenByRoundEnd == true) return;

        OwnerSide = NewOwnerSide;

        MoveDirection = -MoveDirection;

        SetPoisonRotation();
    }
    #endregion

    #region Round Events
    private void FreezeForRoundEnd()
    {
        if (IsFrozenByRoundEnd == true) return;

        CanMove = false;

        PoisonCollider.enabled = false;

        if (HasImpacted == true) return;

        IsFrozenByRoundEnd = true;

        PoisonAnimator.speed = 0f;
    }
    #endregion

    #region Animation Event Methods
    public void StartForward()
    {
        if (HasImpacted == true) return;
        if (IsFrozenByRoundEnd == true) return;

        transform.SetParent(null, true);

        PlayAnimationTrigger(ForwardTriggerName);

        PoisonCollider.enabled = true;

        CanMove = true;
    }

    public void DestroyPoison()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Helper Methods
    private void PlayAnimationTrigger(string TriggerName)
    {
        PoisonAnimator.SetTrigger(TriggerName);
    }
    #endregion
}