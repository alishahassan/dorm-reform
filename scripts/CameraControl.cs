using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float moveSpeed = 10f;
    public GameObject floor; // Assign your Floor GameObject
    public float cameraPadding = 2f; // Extra space around the floor
    public float zoomSpeed = 5f;
    public float minCameraHeight = 5f; // Minimum camera height
    public float maxCameraHeight = 20f; // Maximum camera height

    public bool isScalingMode = false;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    private bool isDraggingHandle = false;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        isDraggingHandle = CheckIfDraggingHandle();

        if (!isScalingMode)
        {
            // Normal Camera Controls (WASD, Right-Click Rotation, and Dolly Zoom)
            HandleNormalCameraMovement();
        }
        else
        {
            // Scaling Mode Camera (Locked Bird's-Eye View with Dolly Zoom)
            HandleScalingCamera();
        }
    }

    void HandleNormalCameraMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get the camera's forward and right directions based on its current rotation
        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDirection = forward * vertical + right * horizontal;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Rotation (only if right mouse button is held and not dragging a handle)
        if (Input.GetMouseButton(1) && !isDraggingHandle)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }

        // Dolly Zoom with mouse wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float heightChange = scroll * zoomSpeed;
            float newCameraHeight = Mathf.Clamp(transform.position.y - heightChange, minCameraHeight, maxCameraHeight);
            transform.position = new Vector3(transform.position.x, newCameraHeight, transform.position.z);
        }
    }

    void HandleScalingCamera()
    {
        if (floor != null)
        {
            // Calculate floor bounds
            Bounds floorBounds = floor.GetComponent<Renderer>().bounds;
            float floorLength = floorBounds.size.z;
            float floorWidth = floorBounds.size.x;
            float maxFloorDimension = Mathf.Max(floorLength, floorWidth) + cameraPadding;

            // Calculate ideal camera height (used as a reference)
            float idealCameraHeight = maxFloorDimension / (2f * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));

            // Position camera above floor
            transform.position = new Vector3(floor.transform.position.x, Mathf.Clamp(transform.position.y, idealCameraHeight, maxCameraHeight), floor.transform.position.z);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Look straight down

            // Dolly Zoom with W and S keys
            float verticalInput = Input.GetAxis("Vertical"); // Returns 1 for W, -1 for S, 0 for nothing
            if (verticalInput != 0f)
            {
                float heightChange = verticalInput * zoomSpeed * Time.deltaTime;
                float newCameraHeight = Mathf.Clamp(transform.position.y - heightChange, idealCameraHeight, maxCameraHeight);
                transform.position = new Vector3(floor.transform.position.x, newCameraHeight, floor.transform.position.z);
            }
        }
    }

    public void ToggleScalingMode()
    {
        isScalingMode = !isScalingMode;

        if (isScalingMode)
        {
            // Save original camera state
            originalCameraPosition = transform.position;
            originalCameraRotation = transform.rotation;

            // Move to scaling camera position
            HandleScalingCamera();
        }
        else
        {
            // Restore original camera state
            transform.position = originalCameraPosition;
            transform.rotation = originalCameraRotation;
        }
    }

    // Function to check if a handle is being dragged
    private bool CheckIfDraggingHandle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("DragHandle")) // Use a tag for handles
            {
                return true;
            }
        }

        return false;
    }
}