using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 20;
    public List<InventorySystem> items = new List<InventorySystem>();


    public void AddItem(string itemName, int amount, Sprite icon)
    {
        string cleanName = itemName.Replace("(Clone)", "").Trim();
        InventorySystem existingItem = items.Find(i => i.itemName.ToLower() == cleanName.ToLower());
        if (existingItem != null)
        {
            existingItem.stack += amount;
            return;
        }
        if (items.Count < inventorySize)
        {
            items.Add(new InventorySystem(cleanName, amount, icon));
        }
    }


    public void RemoveItem(string itemName, int amount)
    {
        InventorySystem item = items.Find(i => i.itemName.ToLower() == itemName.ToLower());
        if (item != null)
        {
            item.stack -= amount;
            if (item.stack <= 0)
                items.Remove(item);
        }
    }


    public bool HasItem(string itemName, int amount)
    {
        InventorySystem item = items.Find(i => i.itemName.ToLower() == itemName.ToLower());
        return item != null && item.stack >= amount;
    }


    public void PrintInventory()
    {
        foreach (var item in items)
            Debug.Log(item.itemName + " x" + item.stack);
    }
}

