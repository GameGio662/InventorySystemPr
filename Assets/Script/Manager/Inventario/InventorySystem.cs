using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystem
{
    public string itemName;
    public int stack;
    public Sprite itemIcon;

    public InventorySystem(string _itemName, int _stack, Sprite _itemIcon)
    {
        itemName = _itemName;
        stack = _stack;
        itemIcon = _itemIcon;
    }

    public bool IsDeplete()
    {
        return stack <= 0;
    }
}
