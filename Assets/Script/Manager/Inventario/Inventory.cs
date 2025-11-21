using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySystem> items = new List<InventorySystem>();
    public InventoryUI ui;

    public void AddItem(string itemName, int amount, Sprite icon)
    {
        InventorySystem existing = items.Find(x => x.itemName == itemName);

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
                {
                    items.Add(new InventorySystem(itemName, overflow, icon));
                }
            }
        }
        else
        {
            if (items.Count < 27)
            {
                items.Add(new InventorySystem(itemName, amount, icon));
            }
        }

        ui.UpdateUI(items);
    }
}

