using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class VRSceneInitializer : MonoBehaviour
{
    public Transform targetParent;
    private List<GameObject> copiedObjects = new List<GameObject>();
    void Start()
    {
        StartCoroutine(CopyRoomObjects());
    }

    IEnumerator CopyRoomObjects()
    {
        yield return null;
        GameObject[] objectsToCopy = null;

        objectsToCopy = GameObject.FindGameObjectsWithTag("Room Element");
        if (objectsToCopy != null)
        {
            foreach (GameObject obj in objectsToCopy)
            {
                if (obj != null)
                {
                    GameObject copy = Instantiate(obj, targetParent);
                    copiedObjects.Add(copy);

                    DestroyImmediate(copy.GetComponent<RoomDimensionValidator>());
                    DestroyImmediate(copy.GetComponent<FloorScaler>());
                }
            }
        }
        else
        {
            Debug.LogError("No Room Elements found to copy!");
        }
    }

    public void CleanupCopiedObjects()
    {
        foreach (GameObject obj in copiedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        copiedObjects.Clear();
    }
}
