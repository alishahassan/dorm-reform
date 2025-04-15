using UnityEngine;
using UnityEngine.UI;

public class RegistrationUI : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text messageText;
    public BackendService backendService; // Assign in Inspector

    public void OnRegisterButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        backendService.RegisterUser(username, password,
            (successMessage) => { messageText.text = successMessage; }, // onSuccess
            (errorMessage) => { messageText.text = errorMessage; }      // onError
        );
    }
}
