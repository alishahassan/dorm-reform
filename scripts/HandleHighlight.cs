using UnityEngine;

public class HandleHighlight : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;
    public Color hoverColor = Color.green;

    public enum HandleType { SE, NW, NE, SW }
    public HandleType handleType;

    public RoomBuilder roomBuilder;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }
        else
        {
            Debug.LogError("HandleHighlight requires a MeshRenderer component.");
        }
    }

    void OnMouseEnter()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = hoverColor;
        }
    }

    void OnMouseExit()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }
    }

    void OnMouseDrag()
    {
        if (Event.current.button != 0)
        {
            return;
        }
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector3 movement = worldPosition - transform.position;

        if (roomBuilder != null)
        {
            AdjustFloorScale(movement);
        }
        else
        {
            Debug.LogError("Room Builder GameObject not assigned in HandleHighlight.");
        }

        transform.position = worldPosition;
    }

    void AdjustFloorScale(Vector3 movement)
    {
        if (roomBuilder == null)
        {
            Debug.LogError("Room Builder script not assigned in HandleHighlight.");
            return;
        }

        Vector3 scaleChange = Vector3.zero;

        switch (handleType)
        {
            case HandleType.SE:
                scaleChange = new Vector3(movement.x, 0, movement.z);
                break;
            case HandleType.NW:
                scaleChange = new Vector3(-movement.x, 0, -movement.z);
                break;
            case HandleType.NE:
                scaleChange = new Vector3(movement.x, 0, -movement.z);
                break;
            case HandleType.SW:
                scaleChange = new Vector3(-movement.x, 0, movement.z);
                break;
        }

        roomBuilder.AdjustFloorScale(scaleChange, handleType);
    }
}