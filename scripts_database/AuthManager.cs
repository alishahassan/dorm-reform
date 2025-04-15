using Firebase.Auth;
using UnityEngine;
using TMPro; // Import for TMP Input Fields

public class AuthManager : MonoBehaviour
{
    // UI references
    public TMP_InputField emailInputField;  // TMP_InputField for email
    public TMP_InputField passwordInputField; // TMP_InputField for password
    public TextMeshProUGUI statusText; // TMP_Text for status display

    // Firebase authentication reference
    private FirebaseAuth auth;

    void Start()
    {
        // Initialize Firebase Authentication
        auth = FirebaseAuth.DefaultInstance;
    }

    // Login functionality
    public void Login()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // Asynchronously sign in with email and password
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                // Handle cancellation of login
                Debug.LogError("Login canceled.");
                statusText.text = "Login canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                // Handle login failure
                Debug.LogError("Login failed: " + task.Exception);
                statusText.text = "Login failed: " + task.Exception.Message;
                return;
            }

            // Successful login
            FirebaseUser newUser = task.Result.User;
            Debug.Log("Logged in as: " + newUser.Email);
            statusText.text = "Logged in as: " + newUser.Email;

            // Optionally, load the main menu or another scene here
            // For example: SceneManager.LoadScene("MainMenu");
        });
    }

    // Create new account functionality
    public void CreateAccount()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        // Asynchronously create a new user with email and password
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                // Handle cancellation of account creation
                Debug.LogError("Account creation canceled.");
                statusText.text = "Account creation canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                // Handle account creation failure
                Debug.LogError("Account creation failed: " + task.Exception);
                statusText.text = "Account creation failed: " + task.Exception.Message;
                return;
            }

            // Successful account creation
            FirebaseUser newUser = task.Result.User;
            Debug.Log("Account created for: " + newUser.Email);
            statusText.text = "Account created for: " + newUser.Email;

            // Optionally, log the user in immediately after account creation
            // Login();
        });
    }
}
