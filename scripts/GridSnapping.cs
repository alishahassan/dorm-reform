using UnityEngine;

public class GridSnapping : MonoBehaviour
{
    public float snapSize = 0.5f;  // Size of the grid
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    public void SnapToGrid()
    {
        float x = Mathf.Round((transform.position.x - originalPosition.x) / snapSize) * snapSize + originalPosition.x;
        float z = Mathf.Round((transform.position.z - originalPosition.z) / snapSize) * snapSize + originalPosition.z;
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
