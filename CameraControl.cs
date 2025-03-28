using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float moveSpeed = 10f;

    private bool isDraggingHandle = false;

    private float fixedHeight = 25f; // The locked height

    void Update()
    {
        isDraggingHandle = CheckIfDraggingHandle();

        // WASD Movement (move camera along X and Z, keeping Y locked)
        if (!isDraggingHandle)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Get the camera's forward and right directions based on its current rotation
            Vector3 forward = transform.forward; // This is the local forward direction
            forward.y = 0; // Flatten the vector so it doesn't move vertically
            forward.Normalize(); // Normalize it to avoid faster movement diagonally

            Vector3 right = transform.right; // This is the local right direction
            right.y = 0; // Flatten the vector
            right.Normalize(); // Normalize it

            // Move the camera based on WASD input, relative to camera's rotation
            Vector3 moveDirection = forward * vertical + right * horizontal;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // Ensure the camera stays locked at Y = fixedHeight
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(currentPosition.x, fixedHeight, currentPosition.z);

        // Rotation (only if right mouse button is held and not dragging a handle)
        if (Input.GetMouseButton(1) && !isDraggingHandle) // 1 represents the right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }
    }

    // Function to check if a handle is being dragged
    private bool CheckIfDraggingHandle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name.StartsWith("Handle_")) // Check if clicked object is a handle
            {
                return true;
            }
        }

        return false; // No handle is being dragged
    }
}
