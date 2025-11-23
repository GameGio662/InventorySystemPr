using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 6f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;


    public float pickupDistance = 3f;
    public LayerMask pickupLayer;
    public Inventory inventory;
    private Camera cam;
    private bool canPickup = true;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        Cursor.lockState = CursorLockMode.Locked;


        cam = Camera.main;
    }

    void Update()
    {
        //HandleMouseLook();
        if (!canPickup) return;
        if (Input.GetKeyDown(KeyCode.F)) TryPickup();
    }


    void FixedUpdate()
    {
        HandleMovement();
    }


    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = (transform.forward * z + transform.right * x).normalized;

        Vector3 targetVelocity = direction * moveSpeed;
        Vector3 velocityChange = targetVelocity - rb.velocity;
        velocityChange.y = 0; 

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void TryPickup()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, pickupLayer))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null && inventory != null)
            {
                inventory.AddItem(item.itemName, 1, item.itemIcon);
                Destroy(item.gameObject);
                StartCoroutine(PickupCooldown());
            }
        }
    }

    IEnumerator PickupCooldown()
    {
        canPickup = false;
        yield return new WaitForSeconds(0.15f);
        canPickup = true;
    }

}
