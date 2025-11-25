using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryContextMenu : MonoBehaviour
{
    public static InventoryContextMenu Instance;

    [Header("UI")]
    public GameObject panel;
    public Button useButton;
    public Button splitButton;

    private InventorySlot currentSlot;
    private Inventory inventory;

    private void Awake()
    {
        Debug.Log("[ContextMenu] Awake chiamato su " + gameObject.name);

        Instance = this;
        inventory = FindObjectOfType<Inventory>();

        if (panel == null)
            Debug.LogError("[ContextMenu] ERRORE: Pannello non assegnato!");

        if (useButton == null)
            Debug.LogError("[ContextMenu] ERRORE: UseButton non assegnato!");

        if (splitButton == null)
            Debug.LogError("[ContextMenu] ERRORE: SplitButton non assegnato!");

        
        if (panel != null)
            panel.SetActive(false);

        if (useButton != null)
            useButton.onClick.AddListener(OnUseClicked);

        if (splitButton != null)
            splitButton.onClick.AddListener(OnSplitClicked);
    }

    
    public void Show(InventorySlot slot, Vector2 screenPos)
    {
        if (panel == null)
        {
            Debug.LogError("[ContextMenu] Show chiamato ma panel è NULL");
            return;
        }

        currentSlot = slot;
        panel.SetActive(true);
        panel.transform.position = screenPos;

        InventorySystem item = inventory.GetItem(slot.slotIndex);

        bool canUse = item != null && item.isUsable;
        bool canSplit = item != null && item.stack > 1;

        useButton.gameObject.SetActive(canUse);
        splitButton.gameObject.SetActive(canSplit);

        Debug.Log($"[ContextMenu] Aperto su slot {slot.slotIndex}, Usabile={canUse}, Divisibile={canSplit}");
    }

    public void Hide()
    {
        if (panel != null)
            panel.SetActive(false);

        currentSlot = null;
    }

    private void Update()
    {
        
        if (panel == null || !panel.activeSelf)
            return;

      
        if (Input.GetMouseButtonDown(0))
        {
            
            if (!IsPointerOverThisMenu())
            {
                Hide();
            }
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            if (!IsPointerOverThisMenu())
                Hide();
        }
    }

    private bool IsPointerOverThisMenu()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        foreach (var r in results)
        {
           
            if (r.gameObject.transform.IsChildOf(panel.transform))
                return true;
        }

        
        return false;
    }

    private void OnUseClicked()
    {
        Debug.Log("[ContextMenu] USE premuto");

        if (currentSlot == null || inventory == null)
        {
            Hide();
            return;
        }

        inventory.UseItem(currentSlot.slotIndex);
        Hide();
    }

    private void OnSplitClicked()
    {
        Debug.Log("[ContextMenu] SPLIT premuto");

        if (currentSlot == null || inventory == null)
        {
            Hide();
            return;
        }

        InventorySystem item = inventory.GetItem(currentSlot.slotIndex);

        if (item != null && item.stack > 1)
        {
           
            DragItemUI.Instance.StartDrag(item.itemIcon, currentSlot, clickMode: true);
        }

        Hide();
    }
}