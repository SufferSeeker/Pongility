using System;
using UnityEngine;

public class DamageableTarget : MonoBehaviour
{
    #region Events
    public static event Action<MatchSide, DamageInfo> OnTargetDied;
    public event Action<int, int> OnHealthChanged;
    public event Action OnHitVisualRequested;
    public event Action<DamageInfo> OnDeathVisualRequested;
    #endregion

    #region Variables
    [Header("Target Settings")]
    [SerializeField] private MatchSide TargetSide;

    [Header("Health Settings")]
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int CurrentHealth;

    [Header("State")]
    [SerializeField] private bool IsDead;
    #endregion

    #region Unity Methods
    private void Awake()
    {

        CurrentHealth = MaxHealth;
        IsDead = false;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
    #endregion

    #region Getters
    public MatchSide GetTargetSide()
    {
        return TargetSide;
    }
    #endregion

    #region Health Methods
    public void TakeDamage(DamageInfo NewDamageInfo)
    {
        if (IsDead == true) return;

        CurrentHealth -= NewDamageInfo.DamageAmount;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        Debug.Log(TargetSide + " took " + NewDamageInfo.DamageAmount + " damage from " + NewDamageInfo.DamageOwnerSide + ". Current Health: " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die(NewDamageInfo);
            return;
        }

        OnHitVisualRequested?.Invoke();
    }

    public void Heal(int HealAmount)
    {
        if (IsDead == true) return;

        if (CurrentHealth >= MaxHealth)
        {
            Debug.Log(TargetSide + " is already at full health.");
            return;
        }

        CurrentHealth += HealAmount;

        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        Debug.Log(TargetSide + " healed " + HealAmount + ". Current Health: " + CurrentHealth);
    }

    public void RestoreFullHealth()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        Debug.Log(TargetSide + " health restored to full.");
    }
    #endregion

    #region Death Logic
    private void Die(DamageInfo DeathDamageInfo)
    {
        if (IsDead == true) return;

        IsDead = true;

        OnDeathVisualRequested?.Invoke(DeathDamageInfo);

        Debug.Log(TargetSide + " died. Killer: " + DeathDamageInfo.DamageOwnerSide);

        OnTargetDied?.Invoke(TargetSide, DeathDamageInfo);
    }
    #endregion
}