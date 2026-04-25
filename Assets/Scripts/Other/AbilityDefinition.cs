using UnityEngine;

public enum AbilityTargetType
{
    Self,
    Opponent,
    Ball,
    Environment
}

public enum AbilityDeliveryType
{
    Instant,
    Projectile,
    SpawnedObject,
    WorldEffect
}

public enum AbilitySpawnPointType
{
    RacketCenter,
    RacketFront,
    RacketBehind,
    RacketLeft,
    RacketRight,
    WorldCenter,
    RandomWorldPoint
}

[CreateAssetMenu(fileName = "New Ability", menuName = "Pongility/Ability Definition")]
public class AbilityDefinition : ScriptableObject
{
    [Header("General Information")]
    [SerializeField] private string AbilityName;
    [SerializeField] private Sprite AbilityIcon;
    [TextArea]
    [SerializeField] private string AbilityDescription;

    [Header("Ability Classification")]
    [SerializeField] private AbilityTargetType TargetType;
    [SerializeField] private AbilityDeliveryType DeliveryType;

    [Header("Projectile Settings")]
    [SerializeField] private GameObject ProjectilePrefab;
    
    [Header("Casting Settings")]
    [SerializeField] private float CastTime;

    [Header("Spawn Settings")]
    [SerializeField] private AbilitySpawnPointType SpawnPointType;

    public string GetAbilityName()
    {
        return AbilityName;
    }

    public Sprite GetAbilityIcon()
    {
        return AbilityIcon;
    }

    public string GetAbilityDescription()
    {
        return AbilityDescription;
    }

    public AbilityTargetType GetTargetType()
    {
        return TargetType;
    }

    public AbilityDeliveryType GetDeliveryType()
    {
        return DeliveryType;
    }

    public GameObject GetProjectilePrefab()
    {
        return ProjectilePrefab;
    }

    public float GetCastTime()
    {
        return CastTime;
    }

    public AbilitySpawnPointType GetSpawnPointType()
    {
        return SpawnPointType;
    }
}