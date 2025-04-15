using UnityEngine;
using UnityEngine.UI;

public class RoomDimensionValidator : MonoBehaviour
{
    public InputField lengthInput;
    public InputField widthInput;
    public Text lengthRangeText;
    public Text widthRangeText;
    public Text errorMessage;
    public GameObject floor;  // Renamed from roomPlane for clarity
    public GameObject wallNorth;
    public GameObject wallSouth;
    public GameObject wallEast;
    public GameObject wallWest;

    public float wallHeight = 3f;  // Default wall height (adjust as needed)
    public float wallThickness = 0.2f; // Default wall thickness (adjust as needed)

    private const float MinLength = 2f;
    private const float MaxLength = 8f;
    private const float MinWidth = 2f;
    private const float MaxWidth = 8f;
    public Vector3 defaultRoomScale = new Vector3(4f, 1f, 6f);

    void Start()
    {
        lengthRangeText.text = $"Length Range: ({MinLength}m - {MaxLength}m)";
        widthRangeText.text = $"Width Range: ({MinWidth}m - {MaxWidth}m)";

        lengthInput.onEndEdit.AddListener(delegate { ValidateInput(lengthInput, MinLength, MaxLength); });
        widthInput.onEndEdit.AddListener(delegate { ValidateInput(widthInput, MinWidth, MaxWidth); });

        // Initialize wall heights
        SetWallHeight();
    }

    public void ValidateInput(InputField inputField, float min, float max)
    {
        if (float.TryParse(inputField.text, out float value))
        {
            if (value < min || value > max)
            {
                errorMessage.text = $"Error: Value must be between {min}m and {max}m.";
                inputField.text = "";
            }
            else
            {
                errorMessage.text = "";
                UpdateRoom(); // Combined floor and wall update
            }
        }
        else
        {
            errorMessage.text = "Error: Please enter a valid number.";
            inputField.text = "";
        }
    }

    private void UpdateRoom()
    {
    if (float.TryParse(lengthInput.text, out float length) &&
        float.TryParse(widthInput.text, out float width))
        {
        // Update Floor Scale
        floor.transform.localScale = new Vector3(width, 1f, length);

        // Update Wall Positions & Scaling
        float halfLength = length / 2f;
        float halfWidth = width / 2f;

        if (wallNorth)
        {
            wallNorth.transform.localScale = new Vector3(width, wallHeight, wallThickness);
            wallNorth.transform.localPosition = new Vector3(0f, wallHeight / 2f, halfLength);
        }

        if (wallEast)
        {
            wallEast.transform.localScale = new Vector3(length, wallHeight, wallThickness);
            wallEast.transform.localPosition = new Vector3(halfWidth, wallHeight / 2f, 0f);
            wallEast.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }

        if (wallWest)
        {
            wallWest.transform.localScale = new Vector3(length, wallHeight, wallThickness);
            wallWest.transform.localPosition = new Vector3(-halfWidth, wallHeight / 2f, 0f);
            wallWest.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }   
    }

    public void ResetDimensions()
    {
        lengthInput.text = defaultRoomScale.x.ToString();
        widthInput.text = defaultRoomScale.z.ToString();
        UpdateRoom(); // Combined floor and wall update
    }

    void SetWallHeight()
    {
        // Set the initial wall height
        wallNorth.transform.localScale = new Vector3(wallNorth.transform.localScale.x, wallHeight, wallNorth.transform.localScale.z);
        wallSouth.transform.localScale = new Vector3(wallSouth.transform.localScale.x, wallHeight, wallSouth.transform.localScale.z);
        wallEast.transform.localScale = new Vector3(wallEast.transform.localScale.x, wallHeight, wallEast.transform.localScale.z);
        wallWest.transform.localScale = new Vector3(wallWest.transform.localScale.x, wallHeight, wallWest.transform.localScale.z);
    }
}