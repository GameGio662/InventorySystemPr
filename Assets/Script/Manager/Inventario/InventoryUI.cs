using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> slots;      
    public GameObject inventoryPanel;  
    public KeyCode toggleKey = KeyCode.E;

    private bool isOpen = false;

    private void Start()
    {
        inventoryPanel.SetActive(isOpen);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);

            if (isOpen)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void UpdateUI(List<InventorySystem> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Image icon = slots[i].Find("Icon").GetComponent<Image>();
            TMP_Text stackTxt = slots[i].Find("StackText").GetComponent<TMP_Text>();

            if (i < items.Count)
            {
                icon.sprite = items[i].itemIcon;
                icon.enabled = true;

                stackTxt.text = items[i].stack > 1 ? items[i].stack.ToString() : "";
            }
            else
            {
                icon.enabled = false;
                stackTxt.text = "";
            }
        }
    }
}
