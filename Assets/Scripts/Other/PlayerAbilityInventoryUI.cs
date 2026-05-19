using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityInventoryUI : MonoBehaviour
{
    #region Variables
    [Header("Player Settings")]
    [SerializeField] private MatchSide PlayerSide;

    [Header("References")]
    [SerializeField] private PlayerAbilityInventory Inventory;

    [Header("Slot Icons")]
    [SerializeField] private Image[] AbilityImages = new Image[3];

    [Header("Selected Slot Images")]
    [SerializeField] private Image[] SelectedAbilityImages = new Image[3];
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Inventory = FindInventoryBySide();

        FindAbilityImages();
        FindSelectedAbilityImages();
    }

    private void OnEnable()
    {
        Inventory.OnInventoryChanged += RefreshUI;
    }

    private void Start()
    {
        RefreshUI();
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChanged -= RefreshUI;
    }
    #endregion

    #region UI Logic
    private void RefreshUI()
    {
        for (int i = 0; i < AbilityImages.Length; i++)
        {
            UpdateAbilityIcon(i);
            UpdateSelectedSlotImage(i);
        }
    }

    private void UpdateAbilityIcon(int SlotIndex)
    {
        AbilityDefinition CurrentAbility = Inventory.GetAbilityInSlot(SlotIndex);

        if (CurrentAbility == null)
        {
            AbilityImages[SlotIndex].gameObject.SetActive(false);
            return;
        }


        AbilityImages[SlotIndex].gameObject.SetActive(true);
        AbilityImages[SlotIndex].sprite = CurrentAbility.GetAbilityIcon();
    }

    private void UpdateSelectedSlotImage(int SlotIndex)
    {
        if (Inventory.GetSelectedSlotIndex() == SlotIndex)
        {
            SelectedAbilityImages[SlotIndex].gameObject.SetActive(true);
        }

        else
        {
            SelectedAbilityImages[SlotIndex].gameObject.SetActive(false);
        }
    }
    #endregion

    #region Reference Setup
    private PlayerAbilityInventory FindInventoryBySide()
    {
        PlayerAbilityInventory[] Inventories = FindObjectsByType<PlayerAbilityInventory>(FindObjectsSortMode.None);

        for (int i = 0; i < Inventories.Length; i++)
        {
            if (Inventories[i].GetPlayerSide() == PlayerSide)
            {
                return Inventories[i];
            }
        }

        return null;
    }

    private void FindAbilityImages()
    {
        Transform AbilitySlotsParent = transform.Find("Ability Slots");

        for (int i = 0; i < AbilityImages.Length; i++)
        {
            Transform AbilitySlot = AbilitySlotsParent.Find("Ability Slot " + (i + 1));
            AbilityImages[i] = AbilitySlot.Find("Ability " + (i + 1)).GetComponent<Image>();
        }
    }

    private void FindSelectedAbilityImages()
    {
        Transform AbilitySlotsParent = transform.Find("Ability Slots");

        for (int i = 0; i < SelectedAbilityImages.Length; i++)
        {
            Transform AbilitySlot = AbilitySlotsParent.Find("Ability Slot " + (i + 1));
            SelectedAbilityImages[i] = AbilitySlot.Find("Selected Ability " + (i + 1)).GetComponent<Image>();
        }
    }
    #endregion
}