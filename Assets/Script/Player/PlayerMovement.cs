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
    public bool canPickup = true;

    public bool canLook = true;



    [Header("Power Up")]
    public float boostedSpeed = 12f;
    public float boostDuration = 2f;
    private bool speedBoostActive = false;

    [Header("Jump")]
    public float jumpForce = 8f;      
    public bool jumpBoostArmed = false; 

    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main;
    }

    void Update()
    {
        if (canLook)
        {
            HandleMouseLook();
            HandleMovement();

        }

        
        if (jumpBoostArmed && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpBoostArmed = false;
        }

        

        if (Input.GetKeyDown(KeyCode.F))
            TryPickup();
    }




    void HandleMouseLook()
    {
     

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.localEulerAngles += new Vector3(-mouseY, 0, 0);
    }


    void HandleMovement()
    {
        if (!canLook) return;

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
                inventory.AddItem(item.itemName, 1, item.itemIcon, item.isUsable, item.effectType);
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


    public IEnumerator SpeedBoostRoutine()
    {
        if (speedBoostActive)
            yield break; 

        speedBoostActive = true;

        float originalSpeed = moveSpeed;
        moveSpeed = boostedSpeed;

        yield return new WaitForSeconds(boostDuration);

        moveSpeed = originalSpeed;
        speedBoostActive = false;
    }

    public void ArmJumpBoost()
    {
        jumpBoostArmed = true;
        Debug.Log("JumpBoost ARMATO: premi SPAZIO per saltare.");
    }

}
