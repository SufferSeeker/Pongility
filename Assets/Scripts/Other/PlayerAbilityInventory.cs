using System;
using UnityEngine;

public class PlayerAbilityInventory : MonoBehaviour
{
    #region Variables
    [Header("Player Settings")]
    [SerializeField] private MatchSide PlayerSide;

    [Header("Ability Slots")]
    [SerializeField] private AbilityDefinition[] AbilitySlots = new AbilityDefinition[3];

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

    #region Ability Usage
    public void UseAbilityAtSlot(int SlotIndex)
    {
        if (CanUseAbilities == false) return;

        if (SlotIndex < 0)
        {
            Debug.Log(PlayerSide + " ability slot index is below zero.");
            return;
        }

        if (SlotIndex >= AbilitySlots.Length)
        {
            Debug.Log(PlayerSide + " ability slot index is out of range.");
            return;
        }

        AbilityDefinition AbilityToUse = AbilitySlots[SlotIndex];

        if (AbilityToUse == null)
        {
            Debug.Log(PlayerSide + " ability slot " + (SlotIndex + 1) + " is empty.");
            return;
        }

        SpawnAbility(AbilityToUse);

        Debug.Log(PlayerSide + " used " + AbilityToUse.GetAbilityName() + " from slot " + (SlotIndex + 1));

        AbilitySlots[SlotIndex] = null;

        OnInventoryChanged?.Invoke();
    }

    private void SpawnAbility(AbilityDefinition SelectedAbility)
    {
        GameObject AbilityPrefab = SelectedAbility.GetAbilityPrefab();

        Transform SpawnPoint = GetSpawnPoint(SelectedAbility.GetSpawnPointType());

        GameObject SpawnedAbility = Instantiate(AbilityPrefab, SpawnPoint.position, Quaternion.identity);

        AbilityLifetime AbilityLifetime = SpawnedAbility.GetComponent<AbilityLifetime>();

        AbilityLifetime.Initialize(SelectedAbility.GetActiveLifeTime());

        IUsableAbility UsableAbility = SpawnedAbility.GetComponent<IUsableAbility>();

        UsableAbility.Initialize(GetAbilityDirection(), PlayerSide, SpawnPoint);
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
            InputManager.OnPlayer1AbilitySlotInput += UseAbilityAtSlot;
        }

        else if (PlayerSide == MatchSide.Player2)
        {
            InputManager.OnPlayer2AbilitySlotInput += UseAbilityAtSlot;
        }
    }

    private void UnsubscribeInputEvents()
    {
        if (PlayerSide == MatchSide.Player1)
        {
            InputManager.OnPlayer1AbilitySlotInput -= UseAbilityAtSlot;
        }

        else if (PlayerSide == MatchSide.Player2)
        {
            InputManager.OnPlayer2AbilitySlotInput -= UseAbilityAtSlot;
        }
    }
    #endregion

    #region Getters
    public MatchSide GetPlayerSide()
    {
        return PlayerSide;
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