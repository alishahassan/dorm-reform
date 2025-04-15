using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using System;
using System.Collections; // Add this for List
using Newtonsoft.Json; // If using Newtonsoft.Json
using UnityEngine.Networking;

public class RoomLayoutSaver : MonoBehaviour
{
    public Button menuButton; // The hamburger menu button
    public GameObject menuPanel; // The panel containing the action buttons
    public Button saveButton;
    public Button loadButton;
    public Text statusText;
    public Button resetButton;
    public GameObject confirmationDialog; // The entire confirmation panel
    public Button confirmButton;
    public Button cancelButton;
    public Text confirmText; // The "Are you sure?" text
    public GameObject roomPlane;
    public GameObject furnitureParent; // Parent object containing all furniture as children
    public RoomDimensionValidator roomDimensions;
    public Text roomDimensionsText; // UI Text to display room dimensions
    private string fileExtension = ".room";
    private Vector3 defaultRoomScale = new Vector3(4f, 1f, 6f);

    private bool isMenuOpen = false;

    private float currentLength;
    private float currentWidth;

    void Start()
    {
        // Menu button listener
        menuButton.onClick.AddListener(ToggleMenu);

        // Action button listeners
        saveButton.onClick.AddListener(SaveRoomLayout);
        loadButton.onClick.AddListener(LoadRoomLayout);
        resetButton.onClick.AddListener(ShowConfirmationDialog);
        confirmButton.onClick.AddListener(ResetRoom);
        confirmButton.onClick.AddListener(HideConfirmationDialog);
        cancelButton.onClick.AddListener(HideConfirmationDialog);

        // Ensure confirmation UI elements start hidden
        confirmationDialog.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        confirmText.gameObject.SetActive(false);

        menuPanel.SetActive(false); // Initially hide the menu panel
    }

    void Update()
    {
        UpdateRoomDimensionsText(); // Update dimensions every frame (or less often if needed)
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuPanel.SetActive(isMenuOpen);
    }

    public void SaveRoomLayout()
    {
        string filePath = EditorUtility.SaveFilePanel("Save Room Layout", "", "RoomLayout", fileExtension.Substring(1));

        if (string.IsNullOrEmpty(filePath))
        {
            return; // Cancel the save operation
        }

        if (File.Exists(filePath))
        {
            if (!EditorUtility.DisplayDialog("Overwrite File?", "The file already exists. Do you want to overwrite it?", "Yes", "No"))
            {
                return; // Cancel the overwrite
            }
        }

        RoomData roomData = new RoomData();
        roomData.roomScale = new SerializableVector3(roomPlane.transform.localScale);
        roomData.furnitureData = new FurnitureData[furnitureParent.transform.childCount];
        for (int i = 0; i < furnitureParent.transform.childCount; i++)
        {
            Transform furniture = furnitureParent.transform.GetChild(i);
            roomData.furnitureData[i] = new FurnitureData();
            roomData.furnitureData[i].position = new SerializableVector3(furniture.position);
            roomData.furnitureData[i].rotation = new SerializableQuaternion(furniture.rotation);
            roomData.furnitureData[i].scale = new SerializableVector3(furniture.localScale);
            roomData.furnitureData[i].name = furniture.name;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        formatter.Serialize(file, roomData);
        file.Close();

        statusText.text = "Room layout saved successfully!";
        Invoke("ClearStatus", 5f);
    }

    public void LoadRoomLayout()
    {
        string filePath = EditorUtility.OpenFilePanel("Load Room Layout", "", fileExtension.Substring(1));

        if (string.IsNullOrEmpty(filePath))
        {
            return; // Cancel the load operation
        }

        if (!File.Exists(filePath))
        {
            statusText.text = "File not found.";
            Invoke("ClearStatus", 5f);
            return;
        }

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            RoomData roomData = (RoomData)formatter.Deserialize(file);
            file.Close();

            // Clear existing furniture
            foreach (Transform child in furnitureParent.transform)
            {
                Destroy(child.gameObject);
            }

            // Load room plane scale
            roomPlane.transform.localScale = roomData.roomScale.ToVector3();

            // Update current dimensions and display
            currentLength = roomPlane.transform.localScale.z;
            currentWidth = roomPlane.transform.localScale.x;
            UpdateRoomDimensionsText();

            // Load furniture
            if (roomData.furnitureData != null)
            {
                foreach (FurnitureData furniture in roomData.furnitureData)
                {
                    GameObject prefab = Resources.Load<GameObject>(furniture.name); // Load prefab from Resources folder
                    if (prefab != null)
                    {
                        GameObject furnitureInstance = Instantiate(prefab, furnitureParent.transform);
                        furnitureInstance.transform.position = furniture.position.ToVector3();
                        furnitureInstance.transform.rotation = furniture.rotation.ToQuaternion();
                        furnitureInstance.transform.localScale = furniture.scale.ToVector3();
                    }
                    else
                    {
                        Debug.LogError("Prefab not found: " + furniture.name);
                    }
                }
            }

            statusText.text = "Room layout loaded successfully!";
            Invoke("ClearStatus", 5f);
        }
        catch (Exception e)
        {
            statusText.text = "Error loading file: " + e.Message;
            Invoke("ClearStatus", 5f);
        }
    }

    void ShowConfirmationDialog()
    {
        confirmationDialog.SetActive(true); // Show the entire panel
        confirmButton.gameObject.SetActive(true); // Show Confirm button
        cancelButton.gameObject.SetActive(true); // Show Cancel button
        confirmText.gameObject.SetActive(true); // Show confirmation text
    }

    void HideConfirmationDialog()
    {
        confirmationDialog.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        confirmText.gameObject.SetActive(false);
    }

    void ResetRoom()
    {
        // Remove all furniture and decor
        foreach (Transform child in furnitureParent.transform)
        {
            Destroy(child.gameObject);
        }

        float currentHeight = roomPlane.transform.localScale.z;

        // Reset room dimensions to default (4m x 6m)
        roomPlane.transform.localScale = new Vector3(4f, 6f, currentHeight);
    
        // Update the displayed room dimensions
        UpdateRoomDimensionsText();

        // Show status message
        statusText.text = "Room reset to default!";
        Invoke("ClearStatus", 5f);
    }

    private void ClearStatus()
    {
        statusText.text = "";
    }

    void UpdateRoomDimensionsText()
    {
        if (roomDimensionsText != null && roomPlane != null)
        {
            currentLength = roomPlane.transform.localScale.y;
            currentWidth = roomPlane.transform.localScale.x;
            roomDimensionsText.text = string.Format("Length: {0:F2}m\nWidth: {1:F2}m", currentLength, currentWidth);
        }
    }
}

[System.Serializable]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public struct SerializableQuaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}

[System.Serializable]
public class RoomData
{
    public SerializableVector3 roomScale;
    public FurnitureData[] furnitureData;
}

[System.Serializable]
public class FurnitureData
{
    public SerializableVector3 position;
    public SerializableQuaternion rotation;
    public SerializableVector3 scale;
    public string name;
}