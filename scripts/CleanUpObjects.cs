using UnityEngine;
using UnityEngine.SceneManagement;

public class CleanUpObjects : MonoBehaviour
{
    public void CleanupVRObjects()
    {
        // Destroy by Parent
        Transform roomCopies = GameObject.Find("Room Builder")?.transform; // Find the parent object
        if (roomCopies != null)
        {
            foreach (Transform child in roomCopies)
            {
                if (child.CompareTag("object")) {
                    DontDestroyOnLoad(child);
                }
                else
                    Destroy(child.gameObject); // Destroy each child
            }
            Destroy(roomCopies.gameObject); // Destroy the parent too
        }
    }
}
