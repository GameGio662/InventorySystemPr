using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Inventory inventory;
    public KeyCode toggleKey = KeyCode.E;

    private InventorySlot[] slotScripts;
    private int selectedSlot = -1;

    private bool isOpen = false;

    private void Awake()
    {
        AutoAssignSlots();
    }

    private void Start()
    {
        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        if (inventory != null) inventory.ui = this;

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleInventory();
        }
    }


    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }



    private void AutoAssignSlots()
    {
        slotScripts = GetComponentsInChildren<InventorySlot>(true);
        for (int i = 0; i < slotScripts.Length; i++)
        {
            slotScripts[i].slotIndex = i;
            slotScripts[i].ui = this;

           
            if (slotScripts[i].icon == null)
                slotScripts[i].icon = slotScripts[i].GetComponentInChildren<UnityEngine.UI.Image>(true);
            if (slotScripts[i].stackText == null)
                slotScripts[i].stackText = slotScripts[i].GetComponentInChildren<TMPro.TMP_Text>(true);
        }

        Debug.Log("InventoryUI: AutoAssigned slots = " + slotScripts.Length);
        if (slotScripts.Length == 0) Debug.LogWarning("InventoryUI: no InventorySlot children found. Make sure slots are children of this GameObject (or its descendants).");
    }

    public void UpdateUI(List<InventorySystem> items)
    {
        if (slotScripts == null) AutoAssignSlots();

        for (int i = 0; i < slotScripts.Length; i++)
        {
            if (i < items.Count)
                slotScripts[i].SetItem(items[i].itemIcon, items[i].stack);
            else
                slotScripts[i].Clear();
        }
    }

    public void OnSlotClicked(int index)
    {
        if (inventory == null)
        {
            Debug.LogWarning("InventoryUI: Inventory reference missing.");
            return;
        }

        if (selectedSlot == -1)
        {
            if (index < inventory.items.Count) selectedSlot = index;
            return;
        }

        if (selectedSlot == index)
        {
            selectedSlot = -1;
            return;
        }

        if (selectedSlot < inventory.items.Count)
        {
            if (index < inventory.items.Count)
            {
                var temp = inventory.items[selectedSlot];
                inventory.items[selectedSlot] = inventory.items[index];
                inventory.items[index] = temp;
            }
            else
            {
                var moved = inventory.items[selectedSlot];
                inventory.items.RemoveAt(selectedSlot);
                inventory.items.Add(moved);
            }
        }

        selectedSlot = -1;
        UpdateUI(inventory.items);
    }
}