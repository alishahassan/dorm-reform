using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    public GameObject floor;

    public void AdjustFloorScale(Vector3 scaleChange, HandleHighlight.HandleType handleType)
    {
        if (floor != null)
        {
            Vector3 newFloorScale = new Vector3(
                floor.transform.localScale.x + scaleChange.x,
                floor.transform.localScale.y,
                floor.transform.localScale.z + scaleChange.z
            );

            floor.transform.localScale = newFloorScale;

            AdjustFloorPosition(scaleChange, handleType);

            AdjustRoomBuilderScale(newFloorScale);
        }
        else
        {
            Debug.LogError("Floor GameObject not assigned in RoomBuilder.");
        }
    }

    void AdjustRoomBuilderScale(Vector3 floorScale)
    {
        transform.localScale = new Vector3(
            floorScale.x,
            transform.localScale.y,
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

        floor.transform.localPosition = new Vector3(
            floor.transform.localPosition.x + positionOffset.x,
            floor.transform.localPosition.y,
            floor.transform.localPosition.z + positionOffset.z
        );
    }

    void AdjustRoomBuilderPosition(Vector3 scaleChange, HandleHighlight.HandleType handleType)
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

    transform.localPosition = new Vector3(
        transform.localPosition.x + positionOffset.x,
        transform.localPosition.y,
        transform.localPosition.z + positionOffset.z
    );
    }
}