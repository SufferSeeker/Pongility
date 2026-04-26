using System;
using UnityEngine;

public class DamageableTarget : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private MatchSide TargetSide;

    [Header("Health Settings")]
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int CurrentHealth;

    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public MatchSide GetTargetSide()
    {
        return TargetSide;
    }

    public void TakeDamage(int DamageAmount, MatchSide DamageOwnerSide)
    {
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
        CurrentHealth += HealAmount;

        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        Debug.Log(TargetSide + " healed " + HealAmount + ". Current Health: " + CurrentHealth);
    }

    private void Die(MatchSide DamageOwnerSide)
    {
        Debug.Log(TargetSide + " died. Killer: " + DamageOwnerSide);
    }
}