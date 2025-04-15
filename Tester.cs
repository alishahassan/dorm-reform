using UnityEngine;
using UnityEngine.SceneManagement;

public void Test()
{
    int[] roomCopies = {chair, table};
    CleanUpObjects.CleanUpVRObjects(roomCopies);
    if (roomCopies != null) {
        printf("Room not cleared!");
    }
    else {
        printf("Room cleared successfully!")
    }
}
