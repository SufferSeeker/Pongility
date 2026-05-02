using System;
using UnityEngine;

public class DamageableTarget : MonoBehaviour
{
    public static event Action<MatchSide, MatchSide> OnTargetDied;

    [Header("Target Settings")]
    [SerializeField] private MatchSide TargetSide;

    [Header("Health Settings")]
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int CurrentHealth;

    [Header("State")]
    [SerializeField] private bool IsDead;

    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public MatchSide GetTargetSide()
    {
        return TargetSide;
    }

    public void TakeDamage(int DamageAmount, MatchSide DamageOwnerSide)
    {
        if (IsDead) return;
        
        CurrentHealth -= DamageAmount;

        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        Debug.Log(TargetSide + " took " + DamageAmount + " damage from " + DamageOwnerSide + ". Current Health: " + CurrentHealth);

        if (CurrentHealth <= 0)
        {
            Die(DamageOwnerSide);
        }
    }

    public void Heal(int HealAmount)
    {
        if (IsDead) return;

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

    private void Die(MatchSide DamageOwnerSide)
    {
        if (IsDead) return;

        IsDead = true;

        Debug.Log(TargetSide + " died. Killer: " + DamageOwnerSide);

        OnTargetDied?.Invoke(TargetSide, DamageOwnerSide);
    }
}