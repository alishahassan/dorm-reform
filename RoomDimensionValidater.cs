using UnityEngine;
using UnityEngine.UI;

public class RoomDimensionValidator : MonoBehaviour
{
    public InputField lengthInput;
    public InputField widthInput;
    public Text lengthRangeText;
    public Text widthRangeText;
    public Text errorMessage;
    public GameObject roomPlane;

    private const float MinLength = 8f;
    private const float MaxLength = 20f;
    private const float MinWidth = 8f;
    private const float MaxWidth = 20f;
    public Vector3 defaultRoomScale = new Vector3(12f, 1f, 15f);

    void Start()
    {
        lengthRangeText.text = $"Length Range: ({MinLength}ft - {MaxLength}ft)";
        widthRangeText.text = $"Width Range: ({MinWidth}ft - {MaxWidth}ft)";

        lengthInput.onEndEdit.AddListener(delegate { ValidateInput(lengthInput, MinLength, MaxLength); });
        widthInput.onEndEdit.AddListener(delegate { ValidateInput(widthInput, MinWidth, MaxWidth); });
    }

    public void ValidateInput(InputField inputField, float min, float max)
    {
        if (float.TryParse(inputField.text, out float value))
        {
            if (value < min || value > max)
            {
                errorMessage.text = $"Error: Value must be between {min}ft and {max}ft.";
                inputField.text = "";
            }
            else
            {
                errorMessage.text = "";
                UpdateRoomPlane();
            }
        }
        else
        {
            errorMessage.text = "Error: Please enter a valid number.";
            inputField.text = "";
        }
    }

    private void UpdateRoomPlane()
    {
        if (float.TryParse(lengthInput.text, out float length) &&
            float.TryParse(widthInput.text, out float width))
        {
            roomPlane.transform.localScale = new Vector3(length, 1, width);
        }
    }

    public void ResetDimensions()
    {
        lengthInput.text = defaultRoomScale.x.ToString();
        widthInput.text = defaultRoomScale.z.ToString();
        UpdateRoomPlane();
    }
}
