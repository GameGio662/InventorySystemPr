using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler
{

    public static InventorySlot hoveredSlot;
    public int slotIndex;
    public Image icon;
    public TMP_Text stackText;

    private Inventory inventory;
    private PointerEventData.InputButton dragButton;

    private void Awake()
    {
        if (icon == null) icon = transform.Find("Icon").GetComponent<Image>();
        if (stackText == null) stackText = transform.Find("StackText").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public void SetItem(InventorySystem item)
    {
        if (item != null)
        {
            icon.enabled = true;
            icon.sprite = item.itemIcon;
            stackText.text = item.stack > 1 ? item.stack.ToString() : "";
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        icon.enabled = false;
        icon.sprite = null;
        stackText.text = "";
    }

    // ========== CLICK ==========

    public void OnPointerClick(PointerEventData eventData)
    {
        // 1) Modalità "dividi stack": clic SINISTRO -> dividi tra origin e questo slot
        if (eventData.button == PointerEventData.InputButton.Left &&
            DragItemUI.Instance != null &&
            DragItemUI.Instance.isClickMoveMode &&
            DragItemUI.Instance.originSlot != null)
        {
            InventorySlot origin = DragItemUI.Instance.originSlot;

            if (origin.slotIndex != this.slotIndex)
            {
                inventory.SplitStack(origin.slotIndex, this.slotIndex);
            }

            DragItemUI.Instance.StopDrag();
            return;
        }

        // 2) Se QUALCOSA è draggato, non apriamo menu destro
        if (DragItemUI.Instance != null && DragItemUI.Instance.isDragging)
            return;

        // 3) Click destro -> apri menù Usa / Dividi
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (inventory == null)
            {
                Debug.LogError("[InventorySlot] inventory è NULL su OnPointerClick");
                return;
            }

            InventorySystem item = inventory.GetItem(slotIndex);
            if (item == null) return; 

            if (InventoryContextMenu.Instance == null)
            {
                Debug.LogError("[InventorySlot] InventoryContextMenu.Instance è NULL. Nessun context menu in scena?");
                return;
            }

            InventoryContextMenu.Instance.Show(this, eventData.position);
        }
    }
    // ========== DRAG (sinistro normale) ==========

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (DragItemUI.Instance.isClickMoveMode)
        {
            eventData.pointerDrag = null;
            return;
        }

        dragButton = eventData.button;


        if (dragButton != PointerEventData.InputButton.Left)
        {
            eventData.pointerDrag = null;
            return;
        }

        InventorySystem item = inventory.GetItem(slotIndex);
        if (item == null)
        {
            eventData.pointerDrag = null;
            return;
        }


        DragItemUI.Instance.StartDrag(item.itemIcon, this, clickMode: false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!DragItemUI.Instance.isDragging) return;

        DragItemUI.Instance.icon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
        if (dragButton != PointerEventData.InputButton.Left)
        {
            DragItemUI.Instance.StopDrag();
            return;
        }

        
        if (!DragItemUI.Instance.isDragging)
        {
            DragItemUI.Instance.StopDrag();
            return;
        }

        
        InventorySlot targetSlot = hoveredSlot;

        if (targetSlot != null && targetSlot.slotIndex != this.slotIndex)
        {
            inventory.SwapItems(this.slotIndex, targetSlot.slotIndex);
        }

        DragItemUI.Instance.StopDrag();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoveredSlot = this;
        // Debug.Log($"[InventorySlot] hover su slot {slotIndex}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hoveredSlot == this)
            hoveredSlot = null;
        // Debug.Log($"[InventorySlot] leave slot {slotIndex}");
    }
}