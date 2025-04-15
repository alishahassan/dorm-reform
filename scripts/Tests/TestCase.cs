using UnityEngine;
using UnityEngine.SceneManagement;
using NUnit.Framework;

public class CleanUpObjectsTests // Rename the class to follow testing conventions
{
    [Test] // Mark this method as a test
    public void ClearRoomTest() // Rename the method to indicate it's a test
    {
        // Arrange: Set up the scene for the test
        var roomBuilderGO = new GameObject("Room Builder");
        var cleanUpScript = roomBuilderGO.AddComponent<CleanUpObjects>();

        var chairGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        chairGO.name = "Chair";
        chairGO.tag = "object";
        chairGO.transform.SetParent(roomBuilderGO.transform);

        var tableGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
        tableGO.name = "Table";
        tableGO.transform.SetParent(roomBuilderGO.transform);

        // Act: Perform the action you want to test
        cleanUpScript.CleanupVRObjects(new int[] { 1, 2 });

        // Assert: Check the expected outcome
        Assert.IsNull(GameObject.Find("Room Builder"), "Room Builder should be destroyed.");
        Assert.IsNotNull(GameObject.Find("Chair"), "Chair (tagged) should exist.");
        Assert.IsNull(GameObject.Find("Table"), "Table (untagged) should be destroyed.");

        // Clean up (important for tests to avoid interference)
        Object.DestroyImmediate(roomBuilderGO);
        Object.DestroyImmediate(chairGO);
        Object.DestroyImmediate(tableGO);
    }
}