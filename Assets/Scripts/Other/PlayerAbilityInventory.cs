using System;
using UnityEngine;

public class PlayerAbilityInventory : MonoBehaviour
{
    #region Variables
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

    [Header("State")]
    [SerializeField] private bool CanUseAbilities = true;
    #endregion

    #region Events
    public event Action OnInventoryChanged;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        FindSpawnPointReferences();
    }

    private void OnEnable()
    {
        SubscribeInputEvents();

        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        UnsubscribeInputEvents();

        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }
    #endregion

    #region Event Methods
    private void HandleMatchEnded()
    {
        CanUseAbilities = false;
    }
    #endregion

    #region Inventory Methods
    public bool TryAddAbility(AbilityDefinition NewAbility)
    {
        for (int i = 0; i < AbilitySlots.Length; i++)
        {
            if (AbilitySlots[i] == null)
            {
                AbilitySlots[i] = NewAbility;

                OnInventoryChanged?.Invoke();

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
    #endregion

    #region Slot Selection
    private void SelectPreviousSlot()
    {
        if (CanUseAbilities == false) return;

        SelectedSlotIndex--;

        if (SelectedSlotIndex < 0)
        {
            SelectedSlotIndex = AbilitySlots.Length - 1;
        }

        OnInventoryChanged?.Invoke();
    }

    private void SelectNextSlot()
    {
        if (CanUseAbilities == false) return;

        SelectedSlotIndex++;

        if (SelectedSlotIndex >= AbilitySlots.Length)
        {
            SelectedSlotIndex = 0;
        }

        OnInventoryChanged?.Invoke();
    }
    #endregion

    #region Ability Usage
    private void UseSelectedAbility()
    {
        if (CanUseAbilities == false) return;

        AbilityDefinition SelectedAbility = AbilitySlots[SelectedSlotIndex];

        if (SelectedAbility == null)
        {
            Debug.Log(PlayerSide + " selected ability slot is empty.");
            return;
        }

        SpawnAbility(SelectedAbility);

        Debug.Log(PlayerSide + " used " + SelectedAbility.GetAbilityName());

        AbilitySlots[SelectedSlotIndex] = null;

        OnInventoryChanged?.Invoke();
    }

    private void SpawnAbility(AbilityDefinition SelectedAbility)
    {
        GameObject AbilityPrefab = SelectedAbility.GetAbilityPrefab();
        Transform SpawnPoint = GetSpawnPoint(SelectedAbility.GetSpawnPointType());

        GameObject SpawnedAbility = Instantiate(AbilityPrefab, SpawnPoint.position, Quaternion.identity);

        AbilityFireball Fireball = SpawnedAbility.GetComponent<AbilityFireball>();

        if (Fireball != null)
        {
            Fireball.Initialize(GetAbilityDirection(), PlayerSide, SpawnPoint);
        }
    }
    #endregion

    #region Reference Setup
    private void FindSpawnPointReferences()
    {
        Transform RacketTransform = GetRacketTransform();

        RacketCenter = RacketTransform.Find("Racket Center");
        RacketFront = RacketTransform.Find("Racket Front");
        RacketBehind = RacketTransform.Find("Racket Behind");
        RacketLeft = RacketTransform.Find("Racket Left");
        RacketRight = RacketTransform.Find("Racket Right");
    }

    private Transform GetRacketTransform()
    {
        if (PlayerSide == MatchSide.Player1)
        {
            return GameObject.Find("Player Racket 1").transform;
        }

        if (PlayerSide == MatchSide.Player2)
        {
            return GameObject.Find("Player Racket 2").transform;
        }

        return null;
    }
    #endregion

    #region Input Subscription
    private void SubscribeInputEvents()
    {
        if (PlayerSide == MatchSide.Player1)
        {
            InputManager.OnPlayer1PreviousAbilitySlot += SelectPreviousSlot;
            InputManager.OnPlayer1NextAbilitySlot += SelectNextSlot;
            InputManager.OnPlayer1UseSelectedAbility += UseSelectedAbility;
        }

        else if (PlayerSide == MatchSide.Player2)
        {
            InputManager.OnPlayer2PreviousAbilitySlot += SelectPreviousSlot;
            InputManager.OnPlayer2NextAbilitySlot += SelectNextSlot;
            InputManager.OnPlayer2UseSelectedAbility += UseSelectedAbility;
        }
    }

    private void UnsubscribeInputEvents()
    {
        if (PlayerSide == MatchSide.Player1)
        {
            InputManager.OnPlayer1PreviousAbilitySlot -= SelectPreviousSlot;
            InputManager.OnPlayer1NextAbilitySlot -= SelectNextSlot;
            InputManager.OnPlayer1UseSelectedAbility -= UseSelectedAbility;
        }

        else if (PlayerSide == MatchSide.Player2)
        {
            InputManager.OnPlayer2PreviousAbilitySlot -= SelectPreviousSlot;
            InputManager.OnPlayer2NextAbilitySlot -= SelectNextSlot;
            InputManager.OnPlayer2UseSelectedAbility -= UseSelectedAbility;
        }
    }
    #endregion

    #region Getters
    public MatchSide GetPlayerSide()
    {
        return PlayerSide;
    }

    public int GetSelectedSlotIndex()
    {
        return SelectedSlotIndex;
    }
    #endregion

    #region Helper Methods
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
    #endregion

    #region Public Methods
    public void SetCanUseAbilities(bool CanUseAbilitiesValue)
    {
        CanUseAbilities = CanUseAbilitiesValue;
    }
    #endregion
}