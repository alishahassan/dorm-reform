using UnityEngine;

public class FloorScaler : MonoBehaviour
{
    public GameObject floor;
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

        // Store initial wall positions
        if (wallNorth != null) initialWallNorthPos = wallNorth.transform.position;
        if (wallSouth != null) initialWallSouthPos = wallSouth.transform.position;
        if (wallEast != null) initialWallEastPos = wallEast.transform.position;
        if (wallWest != null) initialWallWestPos = wallWest.transform.position;
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
                float snappedScaleY = Mathf.Round(initialScale.y * scaleFactor / gridSize) * gridSize;
                floor.transform.localScale = new Vector3(initialScale.x, snappedScaleY, initialScale.z);

                // Adjust wall positions for Y-axis scaling (along Z-axis now)
                if (wallNorth != null)
                {
                    wallNorth.transform.position = new Vector3(initialWallNorthPos.x, initialWallNorthPos.y, initialWallNorthPos.z + (snappedScaleY - initialScale.y) / 2f);
                }
                if (wallSouth != null)
                {
                    wallSouth.transform.position = new Vector3(initialWallSouthPos.x, initialWallSouthPos.y, initialWallSouthPos.z - (snappedScaleY - initialScale.y) / 2f);
                }
            }
            else
            {
                float snappedScaleX = Mathf.Round(initialScale.x * scaleFactor / gridSize) * gridSize;
                floor.transform.localScale = new Vector3(snappedScaleX, initialScale.y, initialScale.z);

                // Adjust wall positions for X-axis scaling (remains the same)
                if (wallEast != null)
                {
                    wallEast.transform.position = new Vector3(initialWallEastPos.x + (snappedScaleX - initialScale.x) / 2f, initialWallEastPos.y, initialWallEastPos.z);
                }
                if (wallWest != null)
                {
                    wallWest.transform.position = new Vector3(initialWallWestPos.x - (snappedScaleX - initialScale.x) / 2f, initialWallWestPos.y, initialWallWestPos.z);
                }
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
