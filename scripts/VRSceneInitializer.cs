using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class VRSceneInitializer : MonoBehaviour
{
    public Transform targetParent;
    private List<GameObject> copiedObjects = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                if (obj != null) // Safety check
                {
                    GameObject copy = Instantiate(obj, targetParent); // Instantiate a copy
                    copiedObjects.Add(copy); // Add to the list

                    // Remove any scripts you don't want on the copy
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
            if (obj != null) // Safety check
            {
                Destroy(obj);
            }
        }
        copiedObjects.Clear(); // Clear the list
    }
}
