using System;

[Serializable]
public struct DamageInfo
{
    public int DamageAmount;
    public MatchSide DamageOwnerSide;
    public DamageSourceType DamageSourceType;
    public DeathVisualType DeathVisualType;

    public DamageInfo(
        int damageAmount,
        MatchSide damageOwnerSide,
        DamageSourceType damageSourceType,
        DeathVisualType deathVisualType
    )
    {
        DamageAmount = damageAmount;
        DamageOwnerSide = damageOwnerSide;
        DamageSourceType = damageSourceType;
        DeathVisualType = deathVisualType;
    }
}