using UnityEngine;

public enum ItemEffectType
{
    None,
    SpeedBoost,
    JumpBoost
}

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    [Header("Gameplay")]
    public bool isUsable = false;

    public ItemEffectType effectType = ItemEffectType.None;   // <--- NUOVO

    private void Start()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = gameObject.name.Replace("(Clone)", "").Trim();
    }
}