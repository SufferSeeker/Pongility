using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    [Header("Ability Settings")]
    [SerializeField] private AbilityDefinition AbilityDefinition;

    [Header("Player Inventories")]
    [SerializeField] private PlayerAbilityInventory Player1Inventory;
    [SerializeField] private PlayerAbilityInventory Player2Inventory;

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        BallController BallController = Collision.GetComponent<BallController>();

        if (BallController == null)
        {
            return;
        }

        MatchSide LastHitSide = BallController.GetLastHitSide();

        if (LastHitSide == MatchSide.None)
        {
            Debug.Log("Ability could not be picked up because the ball has no last hit side.");
            return;
        }

        PlayerAbilityInventory TargetInventory = GetTargetInventory(LastHitSide);

        if (TargetInventory == null)
        {
            Debug.Log("No inventory found for " + LastHitSide);
            return;
        }

        bool AbilityAdded = TargetInventory.TryAddAbility(AbilityDefinition);

        if (AbilityAdded == true)
        {
            Destroy(gameObject);
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
}