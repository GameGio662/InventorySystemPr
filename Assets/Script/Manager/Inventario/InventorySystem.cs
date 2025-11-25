using UnityEngine;

[System.Serializable]
public class InventorySystem
{
    public string itemName;
    public int stack;
    public int maxStack = 64;
    public Sprite itemIcon;
    public bool isUsable;

    public ItemEffectType effectType;

    public InventorySystem(string _itemName, int _stack, Sprite _itemIcon, bool _isUsable, ItemEffectType _effectType)
    {
        itemName = _itemName;
        stack = _stack;
        itemIcon = _itemIcon;
        isUsable = _isUsable;
        effectType = _effectType;
    }
}