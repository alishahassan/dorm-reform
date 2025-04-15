using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float moveSpeed = 10f;
    public GameObject floor;
    public float cameraPadding = 2f;
    public float zoomSpeed = 5f;
    public float minCameraHeight = 5f;
    public float maxCameraHeight = 20f;

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
            HandleNormalCameraMovement();
        }
        else
        {
            HandleScalingCamera();
        }
    }

    void HandleNormalCameraMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        Vector3 moveDirection = forward * vertical + right * horizontal;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if (Input.GetMouseButton(1) && !isDraggingHandle)
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }

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
            Bounds floorBounds = floor.GetComponent<Renderer>().bounds;
            float floorLength = floorBounds.size.z;
            float floorWidth = floorBounds.size.x;
            float maxFloorDimension = Mathf.Max(floorLength, floorWidth) + cameraPadding;

            float idealCameraHeight = maxFloorDimension / (2f * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));

            transform.position = new Vector3(floor.transform.position.x, Mathf.Clamp(transform.position.y, idealCameraHeight, maxCameraHeight), floor.transform.position.z);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);

            float verticalInput = Input.GetAxis("Vertical");
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
            originalCameraPosition = transform.position;
            originalCameraRotation = transform.rotation;

            HandleScalingCamera();
        }
        else
        {
            transform.position = originalCameraPosition;
            transform.rotation = originalCameraRotation;
        }
    }

    private bool CheckIfDraggingHandle()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("DragHandle"))
            {
                return true;
            }
        }
        return false;
    }
}