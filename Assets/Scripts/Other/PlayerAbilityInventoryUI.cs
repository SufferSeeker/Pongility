using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityInventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAbilityInventory Inventory;

    [Header("Slot Icons")]
    [SerializeField] private Image[] AbilityImages;

    [Header("Selected Slot Images")]
    [SerializeField] private Image[] SelectedAbilityImages;

    private void Update()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        for (int i = 0; i < AbilityImages.Length; i++)
        {
            AbilityDefinition CurrentAbility = Inventory.GetAbilityInSlot(i);

            if (CurrentAbility == null)
            {
                AbilityImages[i].gameObject.SetActive(false);
            }

            else
            {
                AbilityImages[i].gameObject.SetActive(true);
                AbilityImages[i].sprite = CurrentAbility.GetAbilityIcon();
            }

            if (Inventory.GetSelectedSlotIndex() == i)
            {
                SelectedAbilityImages[i].gameObject.SetActive(true);
            }

            else
            {
                SelectedAbilityImages[i].gameObject.SetActive(false);
            }
        }
    }
}