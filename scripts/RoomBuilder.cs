using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    public GameObject floor;

    public void AdjustFloorScale(Vector3 scaleChange, HandleHighlight.HandleType handleType)
    {
        if (floor != null)
        {
            // Adjust the floor's scale directly based on scaleChange
            Vector3 newFloorScale = new Vector3(
                floor.transform.localScale.x + scaleChange.x,
                floor.transform.localScale.y, // Keep the floor's Y scale the same
                floor.transform.localScale.z + scaleChange.z
            );

            floor.transform.localScale = newFloorScale;

            // Adjust the floor's position based on the change in scale
            AdjustFloorPosition(scaleChange, handleType);

            // Adjust the RoomBuilder's scale to match the floor
            AdjustRoomBuilderScale(newFloorScale);
        }
        else
        {
            Debug.LogError("Floor GameObject not assigned in RoomBuilder.");
        }
    }

    // Adjust the RoomBuilder's scale to match the floor's new scale
    void AdjustRoomBuilderScale(Vector3 floorScale)
    {
        transform.localScale = new Vector3(
            floorScale.x,
            transform.localScale.y, // Keep the RoomBuilder's Y scale the same
            floorScale.z
        );
    }

    void AdjustFloorPosition(Vector3 scaleChange, HandleHighlight.HandleType handleType)
    {
        Vector3 positionOffset = Vector3.zero;

        switch (handleType)
        {
            case HandleHighlight.HandleType.SE:
                positionOffset = new Vector3(scaleChange.x / 2, 0, scaleChange.z / 2);
                break;
            case HandleHighlight.HandleType.NW:
                positionOffset = new Vector3(-scaleChange.x / 2, 0, -scaleChange.z / 2);
                break;
            case HandleHighlight.HandleType.NE:
                positionOffset = new Vector3(scaleChange.x / 2, 0, -scaleChange.z / 2);
                break;
            case HandleHighlight.HandleType.SW:
                positionOffset = new Vector3(-scaleChange.x / 2, 0, scaleChange.z / 2);
                break;
        }

        // Adjust the floor's position based on the change in RoomBuilder's scale
        floor.transform.localPosition = new Vector3(
            floor.transform.localPosition.x + positionOffset.x,
            floor.transform.localPosition.y,
            floor.transform.localPosition.z + positionOffset.z
        );
    }

    void AdjustRoomBuilderPosition(Vector3 scaleChange, HandleHighlight.HandleType handleType)
    {
    // Adjust the RoomBuilder's position based on the change in scale
    Vector3 positionOffset = Vector3.zero;

    switch (handleType)
    {
        case HandleHighlight.HandleType.SE:
            positionOffset = new Vector3(scaleChange.x / 2, 0, scaleChange.z / 2);
            break;
        case HandleHighlight.HandleType.NW:
            positionOffset = new Vector3(-scaleChange.x / 2, 0, -scaleChange.z / 2);
            break;
        case HandleHighlight.HandleType.NE:
            positionOffset = new Vector3(scaleChange.x / 2, 0, -scaleChange.z / 2);
            break;
        case HandleHighlight.HandleType.SW:
            positionOffset = new Vector3(-scaleChange.x / 2, 0, scaleChange.z / 2);
            break;
    }

    // Adjust the RoomBuilder's position based on the change in scale
    transform.localPosition = new Vector3(
        transform.localPosition.x + positionOffset.x,
        transform.localPosition.y,
        transform.localPosition.z + positionOffset.z
    );
    }
}