using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 40;
    public List<InventorySystem> items = new List<InventorySystem>();
    public InventoryUI ui;

    private void Awake()
    {
        
        for (int i = 0; i < inventorySize; i++)
            items.Add(null);
    }

    public void AddItem(string itemName, int amount, Sprite icon, bool isUsable, ItemEffectType effectType)
    {
        Debug.Log($"[Inventory] AddItem -> {itemName} x{amount} (usable: {isUsable})");

        
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null && items[i].itemName == itemName)
            {
                items[i].stack += amount;
                ui.UpdateUI(items);
                return;
            }
        }

      
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = new InventorySystem(itemName, amount, icon, isUsable, effectType);
                ui.UpdateUI(items);
                return;
            }
        }

        Debug.LogWarning("[Inventory] AddItem -> inventario pieno");
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= items.Count)
            return;

        InventorySystem item = items[index];
        if (item == null)
            return;

        if (!item.isUsable)
        {
            Debug.Log($"[Inventory] {item.itemName} non è usabile.");
            return;
        }

        Debug.Log("[Inventory] Uso l'oggetto: " + item.itemName);

        PlayerMovement player = FindObjectOfType<PlayerMovement>();

       
        switch (item.effectType)
        {
            case ItemEffectType.SpeedBoost:
                player.StartCoroutine(player.SpeedBoostRoutine());
                break;

            case ItemEffectType.JumpBoost:
                if (player != null)
                    player.ArmJumpBoost();
                break;

            default:
                Debug.Log("Nessun effetto applicato.");
                break;
        }

        
        item.stack--;
        if (item.stack <= 0)
            items[index] = null;

        ui.UpdateUI(items);
    }

    public InventorySystem GetItem(int index)
    {
        if (index >= 0 && index < items.Count)
            return items[index];

        return null;
    }

    public void SwapItems(int a, int b)
    {
        if (a < 0 || b < 0 || a >= items.Count || b >= items.Count)
            return;

        // se stessi slot, non fare nulla
        if (a == b)
            return;

        InventorySystem itemA = items[a];
        InventorySystem itemB = items[b];

        
        if (itemA == null && itemB == null)
            return;

        
        if (itemA != null && itemB != null && itemA.itemName == itemB.itemName)
        {
            int total = itemA.stack + itemB.stack;
            int max = itemA.maxStack; 

            if (total <= max)
            {
                
                itemB.stack = total;
                items[a] = null;
            }
            else
            {
                
                itemB.stack = max;
                itemA.stack = total - max;
            }

            ui.UpdateUI(items);
            return;
        }

        
        InventorySystem temp = items[a];
        items[a] = items[b];
        items[b] = temp;

        ui.UpdateUI(items);
    }

    public void SplitStack(int originIndex, int targetIndex)
    {
        Debug.Log($"[Inventory] SplitStack({originIndex} -> {targetIndex})");

        if (originIndex < 0 || targetIndex < 0 || originIndex >= items.Count || targetIndex >= items.Count)
            return;

        InventorySystem origin = items[originIndex];
        InventorySystem target = items[targetIndex];

        if (origin == null || origin.stack <= 1)
            return;

        int amountToMove = origin.stack / 2; 
        if (amountToMove <= 0) return;

       
        if (target == null)
        {
            items[targetIndex] = new InventorySystem(origin.itemName, amountToMove, origin.itemIcon, origin.isUsable, origin.effectType);
            origin.stack -= amountToMove;
        }
      
        else if (target.itemName == origin.itemName)
        {
            int space = target.maxStack - target.stack;
            if (space <= 0) return;

            int moved = Mathf.Min(amountToMove, space);
            target.stack += moved;
            origin.stack -= moved;
        }
        else
        {
            return;
        }

        ui.UpdateUI(items);
    }
}