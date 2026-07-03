using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectReceiver : MonoBehaviour
{
    #region Variables
    [Header("References")]
    [SerializeField] private DamageableTarget DamageableTarget;

    [Header("State")]
    [SerializeField] private int ActiveStatusEffectCount;

    private List<ActiveStatusEffect> ActiveStatusEffects;
    private List<Coroutine> ActiveStatusEffectRoutines;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        DamageableTarget = GetComponent<DamageableTarget>();

        ActiveStatusEffects = new List<ActiveStatusEffect>();
        ActiveStatusEffectRoutines = new List<Coroutine>();

        UpdateActiveStatusEffectCount();
    }

    private void OnEnable()
    {
        MatchManager.OnRoundEndFreezeStarted += ClearAllStatusEffects;
        MatchManager.OnRoundCleanupRequested += ClearAllStatusEffects;
        MatchManager.OnMatchEnded += ClearAllStatusEffects;
    }

    private void OnDisable()
    {
        MatchManager.OnRoundEndFreezeStarted -= ClearAllStatusEffects;
        MatchManager.OnRoundCleanupRequested -= ClearAllStatusEffects;
        MatchManager.OnMatchEnded -= ClearAllStatusEffects;
    }
    #endregion

    #region Status Effect Methods
    public void ApplyStatusEffect(StatusEffectDefinition NewStatusEffectDefinition, MatchSide OwnerSide)
    {
        int ExistingStatusEffectIndex = GetStatusEffectIndex(NewStatusEffectDefinition);

        if (ExistingStatusEffectIndex >= 0)
        {
            ReapplyStatusEffect(ExistingStatusEffectIndex, OwnerSide);
            return;
        }

        AddNewStatusEffect(NewStatusEffectDefinition, OwnerSide);
    }

    private void AddNewStatusEffect(StatusEffectDefinition NewStatusEffectDefinition, MatchSide OwnerSide)
    {
        ActiveStatusEffect NewActiveStatusEffect = new ActiveStatusEffect(NewStatusEffectDefinition, OwnerSide);

        ActiveStatusEffects.Add(NewActiveStatusEffect);
        ActiveStatusEffectRoutines.Add(null);

        UpdateActiveStatusEffectCount();

        Debug.Log(gameObject.name + " received status effect: " + NewActiveStatusEffect.GetStatusEffectType());

        ApplyStatusEffectTick(NewActiveStatusEffect);

        if (ActiveStatusEffects.Contains(NewActiveStatusEffect) == false) return;

        if (NewActiveStatusEffect.HasRemainingTicks() == true)
        {
            int NewStatusEffectIndex = ActiveStatusEffects.IndexOf(NewActiveStatusEffect);
            ActiveStatusEffectRoutines[NewStatusEffectIndex] = StartCoroutine(StatusEffectRoutine(NewActiveStatusEffect));
            return;
        }

        RemoveStatusEffect(NewActiveStatusEffect);
    }

    private void ReapplyStatusEffect(int ExistingStatusEffectIndex, MatchSide OwnerSide)
    {
        ActiveStatusEffect ExistingStatusEffect = ActiveStatusEffects[ExistingStatusEffectIndex];

        ExistingStatusEffect.Reapply(OwnerSide);

        if (ActiveStatusEffectRoutines[ExistingStatusEffectIndex] != null)
        {
            StopCoroutine(ActiveStatusEffectRoutines[ExistingStatusEffectIndex]);
        }

        Debug.Log(gameObject.name + " reapplied status effect: " + ExistingStatusEffect.GetStatusEffectType() + " Stack: " + ExistingStatusEffect.GetCurrentStackCount());

        ApplyStatusEffectTick(ExistingStatusEffect);

        if (ActiveStatusEffects.Contains(ExistingStatusEffect) == false) return;

        if (ExistingStatusEffect.HasRemainingTicks() == true)
        {
            ActiveStatusEffectRoutines[ExistingStatusEffectIndex] = StartCoroutine(StatusEffectRoutine(ExistingStatusEffect));
            return;
        }

        RemoveStatusEffect(ExistingStatusEffect);
    }

    private IEnumerator StatusEffectRoutine(ActiveStatusEffect ActiveStatusEffect)
    {
        while (ActiveStatusEffect.HasRemainingTicks() == true)
        {
            yield return new WaitForSeconds(ActiveStatusEffect.GetTickInterval());

            ApplyStatusEffectTick(ActiveStatusEffect);

            if (ActiveStatusEffects.Contains(ActiveStatusEffect) == false)
            {
                yield break;
            }
        }

        RemoveStatusEffect(ActiveStatusEffect);
    }

    private void ApplyStatusEffectTick(ActiveStatusEffect ActiveStatusEffect)
    {
        DamageInfo TickDamageInfo = ActiveStatusEffect.CreateTickDamageInfo();

        DamageableTarget.TakeDamage(TickDamageInfo);

        if (ActiveStatusEffects.Contains(ActiveStatusEffect) == false) return;

        ActiveStatusEffect.ConsumeTick();

        Debug.Log(gameObject.name + " status effect tick: " + ActiveStatusEffect.GetStatusEffectType() + " Stack: " + ActiveStatusEffect.GetCurrentStackCount());
    }

    private int GetStatusEffectIndex(StatusEffectDefinition NewStatusEffectDefinition)
    {
        for (int i = 0; i < ActiveStatusEffects.Count; i++)
        {
            if (ActiveStatusEffects[i].HasSameStatusEffectType(NewStatusEffectDefinition) == true)
            {
                return i;
            }
        }

        return -1;
    }

    private void RemoveStatusEffect(ActiveStatusEffect ActiveStatusEffect)
    {
        int StatusEffectIndex = ActiveStatusEffects.IndexOf(ActiveStatusEffect);

        if (StatusEffectIndex < 0) return;

        Debug.Log(gameObject.name + " status effect ended: " + ActiveStatusEffect.GetStatusEffectType());

        ActiveStatusEffects.RemoveAt(StatusEffectIndex);
        ActiveStatusEffectRoutines.RemoveAt(StatusEffectIndex);

        UpdateActiveStatusEffectCount();
    }

    private void ClearAllStatusEffects()
    {
        if (ActiveStatusEffects.Count == 0) return;

        for (int i = 0; i < ActiveStatusEffectRoutines.Count; i++)
        {
            if (ActiveStatusEffectRoutines[i] != null)
            {
                StopCoroutine(ActiveStatusEffectRoutines[i]);
            }
        }

        ActiveStatusEffects.Clear();
        ActiveStatusEffectRoutines.Clear();

        UpdateActiveStatusEffectCount();

        Debug.Log(gameObject.name + " cleared all status effects.");
    }

    private void UpdateActiveStatusEffectCount()
    {
        ActiveStatusEffectCount = ActiveStatusEffects.Count;
    }
    #endregion
}