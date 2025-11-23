using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;
    public InventoryUI ui;
    public Image icon;
    public TMP_Text stackText;

    public void SetItem(Sprite itemIcon, int amount)
    {
        if (icon == null || stackText == null) return;
        if (itemIcon != null)
        {
            icon.sprite = itemIcon;
            icon.enabled = true;
            stackText.text = amount > 1 ? amount.ToString() : "";
        }
        else
        {
            Clear();
        }
    }

    public void Clear()
    {
        if (icon == null || stackText == null) return;
        icon.sprite = null;
        icon.enabled = false;
        stackText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ui != null) ui.OnSlotClicked(slotIndex);
    }
}