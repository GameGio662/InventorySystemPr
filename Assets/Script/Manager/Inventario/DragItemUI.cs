
using UnityEngine;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI Instance;

    public Image icon;
    public bool isDragging = false;
    public bool isClickMoveMode = false;   
    public InventorySlot originSlot;

    private void Awake()
    {
        Instance = this;
        icon.enabled = false;
    }

    private void Update()
    {
        if (isDragging)
            icon.transform.position = Input.mousePosition;
    }

  
    public void StartDrag(Sprite sprite, InventorySlot slot, bool clickMode = false)
    {
        isClickMoveMode = clickMode;
        isDragging = true;
        originSlot = slot;

        icon.sprite = sprite;
        icon.enabled = true;
    }

    public void StopDrag()
    {
        isDragging = false;
        isClickMoveMode = false;
        originSlot = null;
        icon.enabled = false;
    }
}