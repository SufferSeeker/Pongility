using UnityEngine;

public class PlayerAbilityInventory : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private MatchSide PlayerSide;

    [Header("Ability Slots")]
    [SerializeField] private AbilityDefinition[] AbilitySlots = new AbilityDefinition[3];
    [SerializeField] private int SelectedSlotIndex;

    [Header("Spawn Points")]
    [SerializeField] private Transform RacketCenter;
    [SerializeField] private Transform RacketFront;
    [SerializeField] private Transform RacketBehind;
    [SerializeField] private Transform RacketLeft;
    [SerializeField] private Transform RacketRight;

    private void OnEnable()
    {
        InputManager.OnPlayer1PreviousAbilitySlot += SelectPreviousSlot;
        InputManager.OnPlayer1NextAbilitySlot += SelectNextSlot;
        InputManager.OnPlayer1UseSelectedAbility += UseSelectedAbility;
    }

    private void OnDisable()
    {
        InputManager.OnPlayer1PreviousAbilitySlot -= SelectPreviousSlot;
        InputManager.OnPlayer1NextAbilitySlot -= SelectNextSlot;
        InputManager.OnPlayer1UseSelectedAbility -= UseSelectedAbility;
    }

    public MatchSide GetPlayerSide()
    {
        return PlayerSide;
    }

    public bool TryAddAbility(AbilityDefinition NewAbility)
    {
        for (int i = 0; i < AbilitySlots.Length; i++)
        {
            if (AbilitySlots[i] == null)
            {
                AbilitySlots[i] = NewAbility;

                Debug.Log(NewAbility.GetAbilityName() + " added to " + PlayerSide + " slot " + (i + 1));

                return true;
            }
        }

        Debug.Log(PlayerSide + " ability slots are full.");

        return false;
    }

    public AbilityDefinition GetAbilityInSlot(int SlotIndex)
    {
        return AbilitySlots[SlotIndex];
    }

    private void SelectPreviousSlot()
    {
        SelectedSlotIndex--;

        if (SelectedSlotIndex < 0)
        {
            SelectedSlotIndex = AbilitySlots.Length - 1;
        }
    }

    private void SelectNextSlot()
    {
        SelectedSlotIndex++;

        if (SelectedSlotIndex >= AbilitySlots.Length)
        {
            SelectedSlotIndex = 0;
        }
    }

    public int GetSelectedSlotIndex()
    {
        return SelectedSlotIndex;
    }

    private void UseSelectedAbility()
    {
        AbilityDefinition SelectedAbility = AbilitySlots[SelectedSlotIndex];

        if (SelectedAbility == null)
        {
            Debug.Log(PlayerSide + " selected ability slot is empty.");
            return;
        }

        SpawnAbility(SelectedAbility);

        Debug.Log(PlayerSide + " used " + SelectedAbility.GetAbilityName());

        AbilitySlots[SelectedSlotIndex] = null;
    }

    private void SpawnAbility(AbilityDefinition SelectedAbility)
    {
        GameObject ProjectilePrefab = SelectedAbility.GetProjectilePrefab();

        if (ProjectilePrefab == null)
        {
            Debug.Log(SelectedAbility.GetAbilityName() + " has no projectile prefab.");
            return;
        }

        Transform SpawnPoint = GetSpawnPoint(SelectedAbility.GetSpawnPointType());

        if (SpawnPoint == null)
        {
            Debug.Log("Spawn point is missing for " + SelectedAbility.GetAbilityName());
            return;
        }

        GameObject SpawnedAbility = Instantiate(ProjectilePrefab, SpawnPoint.position, Quaternion.identity);

        AbilityFireball Fireball = SpawnedAbility.GetComponent<AbilityFireball>();

        if (Fireball != null)
        {
            Fireball.Initialize(GetAbilityDirection());
        }
    }

    private Transform GetSpawnPoint(AbilitySpawnPointType SpawnPointType)
    {
        if (SpawnPointType == AbilitySpawnPointType.RacketCenter)
        {
            return RacketCenter;
        }

        if (SpawnPointType == AbilitySpawnPointType.RacketFront)
        {
            return RacketFront;
        }

        if (SpawnPointType == AbilitySpawnPointType.RacketBehind)
        {
            return RacketBehind;
        }

        if (SpawnPointType == AbilitySpawnPointType.RacketLeft)
        {
            return RacketLeft;
        }

        if (SpawnPointType == AbilitySpawnPointType.RacketRight)
        {
            return RacketRight;
        }

        return RacketCenter;
    }

    private Vector2 GetAbilityDirection()
    {
        if (PlayerSide == MatchSide.Player1)
        {
            return Vector2.up;
        }

        if (PlayerSide == MatchSide.Player2)
        {
            return Vector2.down;
        }

        return Vector2.zero;
    }
}