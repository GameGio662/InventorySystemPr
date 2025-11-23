using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    private void Start()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = gameObject.name.Replace("(Clone)", "").Trim();
    }
}