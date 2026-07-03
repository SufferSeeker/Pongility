using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect Definition", menuName = "Pongility/Status Effect Definition")]
public class StatusEffectDefinition : ScriptableObject
{
    #region Variables
    [Header("Effect Settings")]
    [SerializeField] private StatusEffectType StatusEffectType;
    [SerializeField] private DamageSourceType DamageSourceType;
    [SerializeField] private DeathVisualType DeathVisualType;

    [Header("Tick Settings")]
    [SerializeField, Min(0)] private int DamagePerTick = 1;
    [SerializeField, Min(1)] private int TickCount = 1;
    [SerializeField, Min(0.1f)] private float TickInterval = 1f;

    [Header("Stack Settings")]
    [SerializeField, Min(1)] private int MaxStackCount = 1;
    #endregion

    #region Getters
    public StatusEffectType GetStatusEffectType()
    {
        return StatusEffectType;
    }

    public DamageSourceType GetDamageSourceType()
    {
        return DamageSourceType;
    }

    public DeathVisualType GetDeathVisualType()
    {
        return DeathVisualType;
    }

    public int GetDamagePerTick()
    {
        return DamagePerTick;
    }

    public int GetTickCount()
    {
        return TickCount;
    }

    public float GetTickInterval()
    {
        return TickInterval;
    }

    public int GetMaxStackCount()
    {
        return MaxStackCount;
    }
    #endregion
}