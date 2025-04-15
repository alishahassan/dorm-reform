using UnityEngine;
using UnityEngine.SceneManagement;

public class CleanUpObjects : MonoBehaviour
{
    public void CleanupVRObjects()
    {
        Transform roomCopies = GameObject.Find("Room Builder")?.transform;
        if (roomCopies != null)
        {
            foreach (Transform child in roomCopies)
            {
                if (child.CompareTag("object")) {
                    DontDestroyOnLoad(child);
                }
                else
                    Destroy(child.gameObject);
            }
            Destroy(roomCopies.gameObject);
        }
    }
}