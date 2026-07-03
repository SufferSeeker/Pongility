public class ActiveStatusEffect
{
    #region Variables
    private StatusEffectDefinition StatusEffectDefinition;
    private MatchSide OwnerSide;
    private int CurrentStackCount;
    private int RemainingTickCount;
    #endregion

    #region Constructor
    public ActiveStatusEffect(StatusEffectDefinition NewStatusEffectDefinition, MatchSide NewOwnerSide)
    {
        StatusEffectDefinition = NewStatusEffectDefinition;
        OwnerSide = NewOwnerSide;

        CurrentStackCount = 1;
        RemainingTickCount = StatusEffectDefinition.GetTickCount();
    }
    #endregion

    #region Getters
    public StatusEffectType GetStatusEffectType()
    {
        return StatusEffectDefinition.GetStatusEffectType();
    }

    public float GetTickInterval()
    {
        return StatusEffectDefinition.GetTickInterval();
    }

    public int GetRemainingTickCount()
    {
        return RemainingTickCount;
    }

    public int GetCurrentStackCount()
    {
        return CurrentStackCount;
    }
    #endregion

    #region Status Effect Logic
    public bool HasSameStatusEffectType(StatusEffectDefinition OtherStatusEffectDefinition)
    {
        if (StatusEffectDefinition.GetStatusEffectType() == OtherStatusEffectDefinition.GetStatusEffectType())
        {
            return true;
        }

        return false;
    }

    public void Reapply(MatchSide NewOwnerSide)
    {
        OwnerSide = NewOwnerSide;

        if (CurrentStackCount < StatusEffectDefinition.GetMaxStackCount())
        {
            CurrentStackCount++;
        }

        RemainingTickCount = StatusEffectDefinition.GetTickCount();
    }

    public DamageInfo CreateTickDamageInfo()
    {
        int TickDamageAmount = StatusEffectDefinition.GetDamagePerTick() * CurrentStackCount;

        DamageInfo NewDamageInfo = new DamageInfo(
            TickDamageAmount,
            OwnerSide,
            StatusEffectDefinition.GetDamageSourceType(),
            StatusEffectDefinition.GetDeathVisualType()
        );

        return NewDamageInfo;
    }

    public void ConsumeTick()
    {
        RemainingTickCount--;

        if (RemainingTickCount < 0)
        {
            RemainingTickCount = 0;
        }
    }

    public bool HasRemainingTicks()
    {
        if (RemainingTickCount > 0)
        {
            return true;
        }

        return false;
    }
    #endregion
}