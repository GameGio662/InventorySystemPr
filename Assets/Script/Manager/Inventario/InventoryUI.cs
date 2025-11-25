using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Riferimenti")]
    public GameObject inventoryPanel;
    public InventorySlot slotPrefab;
    public Transform slotsParent;
    public PlayerMovement player;

    [Header("Dati inventario")]
    public Inventory playerInventory;
    public List<InventorySlot> slotList = new List<InventorySlot>();

    private bool inventoryOpen = false;

    void Start()
    {
        Debug.Log("[InventoryUI] Start");

        
        if (slotsParent != null && slotPrefab != null && slotList.Count == 0)
        {
            GenerateSlots(playerInventory.inventorySize);
            UpdateUI(playerInventory.items);
        }

       
        inventoryOpen = false;

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
            Debug.Log("[InventoryUI] inventoryPanel.SetActive(false) in Start");
        }
        else
        {
            Debug.LogError("[InventoryUI] inventoryPanel NON assegnato!");
        }

       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (player != null)
            player.canLook = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("[InventoryUI] Premi E");
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen;
        Debug.Log("[InventoryUI] ToggleInventory -> inventoryOpen = " + inventoryOpen);

        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(inventoryOpen);
            Debug.Log("[InventoryUI] inventoryPanel activeSelf = " + inventoryPanel.activeSelf);
        }

      
        if (!inventoryOpen && InventoryContextMenu.Instance != null)
        {
            InventoryContextMenu.Instance.Hide();
        }

        if (inventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (player != null)
                player.canLook = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (player != null)
                player.canLook = true;
        }
    }

    public void GenerateSlots(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            InventorySlot newSlot = Instantiate(slotPrefab, slotsParent);
            newSlot.gameObject.SetActive(true);
            newSlot.slotIndex = i;
            slotList.Add(newSlot);
        }
    }

    public void UpdateUI(List<InventorySystem> items)
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (i < items.Count)
                slotList[i].SetItem(items[i]);
            else
                slotList[i].ClearSlot();
        }
    }
}