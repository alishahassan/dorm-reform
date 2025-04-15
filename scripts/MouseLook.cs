using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform camera = null;
    public CharacterController player = null;
    public float currentCamera = 0.0f;
    public float mouseSense = 3.5f;
    public float speed = 5.0f;

    void Start()
    {
        player = GetComponent<CharacterController>();
        camera = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
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

    if (Cursor.lockState != CursorLockMode.Locked) return;
        Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        currentCamera = currentCamera - mouseDirection.y * mouseSense;
        if(currentCamera < -90)
        {
            currentCamera = -90;
        }
        else if(currentCamera > 90)
        {
            currentCamera = 90;
        }
        camera.localEulerAngles = currentCamera * Vector3.right;
        transform.Rotate(mouseDirection.x * Vector3.up * mouseSense);
        Vector3 cameraForward = camera.forward * Input.GetAxisRaw("Vertical");
        Vector3 cameraRight = camera.right * Input.GetAxisRaw("Horizontal");
        cameraRight.y = 0;
        cameraForward.y = 0;

        Vector3 movingDir = cameraForward + cameraRight;
        Vector3 pace = new Vector3(movingDir.x * speed, 0,  movingDir.z * speed);

        player.Move(pace * Time.deltaTime);
    }
}