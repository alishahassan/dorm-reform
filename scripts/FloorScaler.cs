using UnityEngine;

public class FloorScaler : MonoBehaviour
{
    public GameObject floor;
    public GameObject ceiling;
    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;
    public float gridSize = 1f;
    public bool scaleOnYAxis = false;
    private Vector3 initialMousePosition;
    private Vector3 initialScale;
    private bool isDragging = false;

    // Store initial wall positions
    private Vector3 initialWallNorthPos;
    private Vector3 initialWallSouthPos;
    private Vector3 initialWallEastPos;
    private Vector3 initialWallWestPos;

    void OnMouseDown()
    {
        isDragging = true;
        initialMousePosition = Input.mousePosition;
        initialScale = floor.transform.localScale;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - initialMousePosition;
            float scaleFactor = 1 + (scaleOnYAxis ? mouseDelta.y / 100f : mouseDelta.x / 100f);

            if (scaleOnYAxis)
            {
                float newScaleY = initialScale.y * scaleFactor;
                newScaleY = Mathf.Clamp(Mathf.Round(newScaleY / gridSize) * gridSize, 2f, 8f); // Snap to grid & clamp range
                floor.transform.localScale = new Vector3(initialScale.x, newScaleY, initialScale.z);
                ceiling.transform.localScale = new Vector3(initialScale.x, newScaleY, initialScale.z);
            }
            else
            {
                float newScaleX = initialScale.x * scaleFactor;
                newScaleX = Mathf.Clamp(Mathf.Round(newScaleX / gridSize) * gridSize, 2f, 8f);
                floor.transform.localScale = new Vector3(newScaleX, initialScale.y, initialScale.z);
                ceiling.transform.localScale = new Vector3(newScaleX, initialScale.y, initialScale.z);
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}