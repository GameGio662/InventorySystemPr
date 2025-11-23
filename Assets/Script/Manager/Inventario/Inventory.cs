using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySystem> items = new List<InventorySystem>();
    public InventoryUI ui;

    public void AddItem(string itemName, int amount, Sprite icon)
    {
        if (string.IsNullOrEmpty(itemName)) return;
        string clean = itemName.Replace("(Clone)", "").Trim();

        InventorySystem existing = items.Find(x => x.itemName.ToLower() == clean.ToLower());

        if (existing != null)
        {
            int newStack = existing.stack + amount;
            if (newStack <= existing.maxStack)
            {
                existing.stack = newStack;
            }
            else
            {
                int overflow = newStack - existing.maxStack;
                existing.stack = existing.maxStack;
                if (items.Count < 27)
                    items.Add(new InventorySystem(clean, overflow, icon));
            }
        }
        else
        {
            if (items.Count < 27)
                items.Add(new InventorySystem(clean, amount, icon));
        }

        if (ui != null) ui.UpdateUI(items);
        else Debug.LogWarning("Inventory: UI reference is null. Assign InventoryUI in inspector.");
    }
}
