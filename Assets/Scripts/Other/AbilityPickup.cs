using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    #region Variables
    [Header("Ability Settings")]
    [SerializeField] private AbilityDefinition AbilityDefinition;
    [SerializeField] private SpriteRenderer PickupSpriteRenderer;

    [Header("Player Inventories")]
    [SerializeField] private PlayerAbilityInventory Player1Inventory;
    [SerializeField] private PlayerAbilityInventory Player2Inventory;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        FindPickupReferences();
        FindPlayerInventories();
        UpdatePickupVisual();
    }

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        BallController Ball = Collision.GetComponent<BallController>();

        if (Ball == null) return;
        
        MatchSide LastHitSide = Ball.GetLastHitSide();

        if (LastHitSide == MatchSide.None)
        {
            Debug.Log("Ability could not be picked up because the ball has no last hit side.");
            return;
        }

        PlayerAbilityInventory TargetInventory = GetTargetInventory(LastHitSide);

        bool AbilityAdded = TargetInventory.TryAddAbility(AbilityDefinition);

        if (AbilityAdded == true)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Initialization
    public void Initialize(AbilityDefinition NewAbilityDefinition)
    {
        AbilityDefinition = NewAbilityDefinition;

        UpdatePickupVisual();
    }
    #endregion

    #region Reference Setup
    private void FindPickupReferences()
    {
        PickupSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Visual
    private void UpdatePickupVisual()
    {
        if (AbilityDefinition == null) return;
        if (PickupSpriteRenderer == null) return;

        PickupSpriteRenderer.sprite = AbilityDefinition.GetAbilityIcon();
    }
    #endregion

    #region Inventory Setup
    private void FindPlayerInventories()
    {
        PlayerAbilityInventory[] Inventories = FindObjectsByType<PlayerAbilityInventory>(FindObjectsSortMode.None);

        for (int i = 0; i < Inventories.Length; i++)
        {
            if (Inventories[i].GetPlayerSide() == MatchSide.Player1)
            {
                Player1Inventory = Inventories[i];
            }

            else if (Inventories[i].GetPlayerSide() == MatchSide.Player2)
            {
                Player2Inventory = Inventories[i];
            }
        }
    }

    private PlayerAbilityInventory GetTargetInventory(MatchSide Side)
    {
        if (Side == MatchSide.Player1)
        {
            return Player1Inventory;
        }

        if (Side == MatchSide.Player2)
        {
            return Player2Inventory;
        }

        return null;
    }
    #endregion
}