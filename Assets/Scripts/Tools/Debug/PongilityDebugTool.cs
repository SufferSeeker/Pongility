using UnityEngine;

public class PongilityDebugTool : MonoBehaviour
{
    #region Variables
    [Header("Debug Values")]
    [SerializeField] private AbilityDefinition SelectedAbility;

    [Header("Runtime References")]
    [SerializeField] private AbilitySpawner AbilitySpawner;
    [SerializeField] private PlayerAbilityInventory Player1Inventory;
    [SerializeField] private PlayerAbilityInventory Player2Inventory;
    [SerializeField] private MatchManager MatchManager;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        FindReferences();
    }
    #endregion

    #region Ability Debug Methods
    public void SpawnSelectedAbilityPickup()
    {
        if (Application.isPlaying == false) return;

        if (SelectedAbility == null)
        {
            Debug.Log("No selected ability assigned.");
            return;
        }

        AbilitySpawner.SpawnSpecificAbilityForDebug(SelectedAbility);
    }

    public void GiveSelectedAbilityToPlayer1()
    {
        if (Application.isPlaying == false) return;

        if (SelectedAbility == null)
        {
            Debug.Log("No selected ability assigned.");
            return;
        }

        Player1Inventory.TryAddAbility(SelectedAbility);
    }

    public void GiveSelectedAbilityToPlayer2()
    {
        if (Application.isPlaying == false) return;

        if (SelectedAbility == null)
        {
            Debug.Log("No selected ability assigned.");
            return;
        }

        Player2Inventory.TryAddAbility(SelectedAbility);
    }

    public void ClearAbilityObjects()
    {
        if (Application.isPlaying == false) return;

        MatchManager.DebugClearAbilityObjects();
    }
    #endregion

    #region Reference Setup
    private void FindReferences()
    {
        AbilitySpawner = FindFirstObjectByType<AbilitySpawner>();
        MatchManager = FindFirstObjectByType<MatchManager>();

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
    #endregion
}