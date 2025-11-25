using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraLook : MonoBehaviour
{
    public float sensitivity = 150f;
    public float smoothing = 5f;

    public Transform playerBody;

    private Vector2 smoothVelocity;
    private Vector2 currentLook;

    public bool canLook = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (canLook)
        {
            

            Vector2 mouseInput = new Vector2(
                Input.GetAxisRaw("Mouse X"),
                Input.GetAxisRaw("Mouse Y")
            );

            mouseInput *= sensitivity * Time.deltaTime;

            smoothVelocity = Vector2.Lerp(smoothVelocity, mouseInput, 1f / smoothing);
            currentLook += smoothVelocity;

            currentLook.y = Mathf.Clamp(currentLook.y, -90f, 90f);

            playerBody.localRotation = Quaternion.Euler(0f, currentLook.x, 0f);
            Camera.main.transform.localRotation = Quaternion.Euler(-currentLook.y, 0f, 0f);
        }
    }

}
