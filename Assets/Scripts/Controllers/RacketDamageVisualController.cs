using UnityEngine;
using System.Collections;

[RequireComponent(typeof(DamageableTarget))]
public class RacketDamageVisualController : MonoBehaviour
{
    [System.Serializable]
    private struct DeathVisualTriggerBinding
    {
        public DeathVisualType DeathVisualType;
        public string TriggerName;
    }

    #region Variables
    [Header("References")]
    [SerializeField] private Animator RacketAnimator;
    [SerializeField] private SpriteRenderer RacketSpriteRenderer;
    [SerializeField] private DamageableTarget DamageableTarget;

    [Header("Death Animation Triggers")]
    [SerializeField] private DeathVisualTriggerBinding[] DeathVisualTriggerBindings;

    [Header("Hit Flash Settings")]
    [SerializeField] private Material HitFlashMaterial;
    [SerializeField] private int HitFlashCount = 2;
    [SerializeField] private float HitFlashOnDuration = 0.06f;
    [SerializeField] private float HitFlashOffDuration = 0.04f;

    [Header("State")]
    [SerializeField] private Color OriginalColor;
    [SerializeField] private Material OriginalMaterial;
    private Coroutine HitFlashRoutine;

    [Header("Debug")]
    [SerializeField] private bool LogMissingAnimatorMessages = true;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        RacketAnimator = GetComponent<Animator>();
        RacketSpriteRenderer = GetComponent<SpriteRenderer>();
        DamageableTarget = GetComponent<DamageableTarget>();

        OriginalColor = RacketSpriteRenderer.color;
        OriginalMaterial = RacketSpriteRenderer.sharedMaterial;
    }

    private void OnEnable()
    {
        DamageableTarget.OnHitVisualRequested += HandleHitVisualRequested;
        DamageableTarget.OnDeathVisualRequested += HandleDeathVisualRequested;
        MatchManager.OnRoundCleanupRequested += ResetVisuals;
    }

    private void OnDisable()
    {
        DamageableTarget.OnHitVisualRequested -= HandleHitVisualRequested;
        DamageableTarget.OnDeathVisualRequested -= HandleDeathVisualRequested;
        MatchManager.OnRoundCleanupRequested -= ResetVisuals;
    }
    #endregion

    #region Event Handlers
    private void HandleHitVisualRequested()
    {
        PlayHitFlash();
    }

    private void HandleDeathVisualRequested(DamageInfo NewDamageInfo)
    {
        StopHitFlash();

        PlayDeathVisual(NewDamageInfo.DeathVisualType);
    }
    #endregion

    #region Hit Flash
    private void PlayHitFlash()
    {
        if (HitFlashRoutine != null)
        {
            StopCoroutine(HitFlashRoutine);
            RestoreOriginalVisuals();
        }

        HitFlashRoutine = StartCoroutine(HitFlashRoutineMethod());
    }

    private IEnumerator HitFlashRoutineMethod()
    {
        for (int i = 0; i < HitFlashCount; i++)
        {
            RacketSpriteRenderer.sharedMaterial = HitFlashMaterial;

            yield return new WaitForSeconds(HitFlashOnDuration);

            RestoreOriginalVisuals();

            if (i < HitFlashCount - 1)
            {
                yield return new WaitForSeconds(HitFlashOffDuration);
            }
        }

        HitFlashRoutine = null;
    }

    private void StopHitFlash()
    {
        if (HitFlashRoutine != null)
        {
            StopCoroutine(HitFlashRoutine);
            HitFlashRoutine = null;
        }

        RestoreOriginalVisuals();
    }

    private void RestoreOriginalVisuals()
    {
        RacketSpriteRenderer.sharedMaterial = OriginalMaterial;
        RacketSpriteRenderer.color = OriginalColor;
    }
    #endregion

    #region Death Visuals
    private void PlayDeathVisual(DeathVisualType RequestedDeathVisualType)
    {
        for (int i = 0; i < DeathVisualTriggerBindings.Length; i++)
        {
            if (DeathVisualTriggerBindings[i].DeathVisualType == RequestedDeathVisualType)
            {
                PlayAnimatorTrigger(DeathVisualTriggerBindings[i].TriggerName);
                return;
            }
        }

        Debug.Log(gameObject.name + " has no death visual trigger binding for: " + RequestedDeathVisualType);
    }
    #endregion

    #region Reset
    private void ResetVisuals()
    {
        StopHitFlash();

        RacketAnimator.Rebind();
        RacketAnimator.Update(0f);

        RestoreOriginalVisuals();
    }
    #endregion

    #region Helper Methods
    private void PlayAnimatorTrigger(string TriggerName)
    {
        if (HasAnimatorTrigger(TriggerName) == false)
        {
            if (LogMissingAnimatorMessages == true)
            {
                Debug.Log(gameObject.name + " requested visual trigger, but Animator trigger was not found: " + TriggerName);
            }

            return;
        }

        RacketAnimator.SetTrigger(TriggerName);
    }

    private bool HasAnimatorTrigger(string TriggerName)
    {
        AnimatorControllerParameter[] Parameters = RacketAnimator.parameters;

        for (int i = 0; i < Parameters.Length; i++)
        {
            if (Parameters[i].type == AnimatorControllerParameterType.Trigger && Parameters[i].name == TriggerName)
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}