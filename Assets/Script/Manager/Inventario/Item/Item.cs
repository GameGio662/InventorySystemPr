using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;
    public float pickupDistance = 3f;
    public LayerMask pickupLayer;

    private Camera playerCamera;

    private void Start()
    {
        if (string.IsNullOrEmpty(itemName))
            itemName = gameObject.name.Replace("(Clone)", "");
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, pickupDistance, pickupLayer))
            {
                Item targetItem = hit.collider.GetComponent<Item>();
                if (targetItem != null)
                {
                    Inventory inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
                    if (inventory != null)
                    {
                        inventory.AddItem(targetItem.itemName, 1, targetItem.itemIcon);
                        Destroy(targetItem.gameObject);
                    }
                }
            }
        }
    }
}
