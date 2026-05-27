using UnityEngine;

public class AbilityDeflect : MonoBehaviour, IUsableAbility
{

    #region Variables
    [Header("Core References")]
    [SerializeField] private Animator DeflectAnimator;
    [SerializeField] private Collider2D DeflectCollider;

    [Header("Animation Settings")]
    [SerializeField] private string ActiveTriggerName = "Active";

    [Header("State")]
    [SerializeField] private MatchSide OwnerSide;
    [SerializeField] private bool IsActive;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        DeflectAnimator = GetComponent<Animator>();
        DeflectCollider = GetComponent<Collider2D>();

        IsActive = false;

        DeflectCollider.enabled = false;
    }

    private void OnEnable()
    {
        MatchManager.OnRoundEndFreezeStarted += DisableDeflect;
        MatchManager.OnMatchEnded += DisableDeflect;
    }

    private void OnDisable()
    {
        MatchManager.OnRoundEndFreezeStarted -= DisableDeflect;
        MatchManager.OnMatchEnded -= DisableDeflect;
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (IsActive == false) return;

        IDeflectable Deflectable = Collision.GetComponent<IDeflectable>();

        if (Deflectable == null) return;

        if (Deflectable.GetOwnerSide() == OwnerSide) return;

        Deflectable.Deflect(OwnerSide);
    }
    #endregion

    #region Initialization
    public void Initialize(Vector2 Direction, MatchSide NewOwnerSide, Transform CastParent)
    {
        OwnerSide = NewOwnerSide;

        transform.SetParent(CastParent, true);
        transform.localPosition = Vector3.zero;

        IsActive = true;

        DeflectCollider.enabled = true;

        SetDeflectRotation(Direction);

        PlayAnimationTrigger(ActiveTriggerName);
    }
    #endregion

    #region Rotation
    private void SetDeflectRotation(Vector2 Direction)
    {
        if (Direction == Vector2.up)
        {
            transform.localRotation = Quaternion.identity;
        }

        else if (Direction == Vector2.down)
        {
            transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
        }
    }
    #endregion

    #region State Control
    private void DisableDeflect()
    {
        IsActive = false;

        DeflectCollider.enabled = false;
    }
    #endregion

    #region Helper Methods
    private void PlayAnimationTrigger(string TriggerName)
    {
        DeflectAnimator.SetTrigger(TriggerName);
    }
    #endregion
}