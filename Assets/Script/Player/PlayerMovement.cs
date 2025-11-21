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

    private Camera cam;
    private bool canPickup = true;

    Rigidbody rb;
    float pitch = 0f;

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

        if (Input.GetKeyDown(KeyCode.F))
        {
            TryPickup();
        }
    }


    void FixedUpdate()
    {
        HandleMovement();
    }

    //void HandleMouseLook()
    //{
    //    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
    //    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

    //    transform.Rotate(Vector3.up * mouseX);

    //    pitch -= mouseY;
    //    pitch = Mathf.Clamp(pitch, -85f, 85f);
    //    playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);
    //}

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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupDistance, pickupLayer))
        {
            Item item = hit.collider.GetComponent<Item>();
            if (item != null)
            {
                Inventory inv = GetComponent<Inventory>();
                inv.AddItem(item.itemName, 1, item.itemIcon);

                Destroy(item.gameObject);
                StartCoroutine(PickupCooldown());
            }
        }
    }

    System.Collections.IEnumerator PickupCooldown()
    {
        canPickup = false;
        yield return new WaitForSeconds(0.15f);
        canPickup = true;
    }

}
