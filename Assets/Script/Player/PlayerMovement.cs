using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 6f;
    public float mouseSensitivity = 2f;
    public Transform playerCamera;

    Rigidbody rb;
    float pitch = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
        playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);
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
}
