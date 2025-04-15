using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLoginScene()
    {
        SceneManager.LoadScene("Login"); // Replace with your actual login scene name
    }

    public void LoadCreateAccountScene()
    {
        SceneManager.LoadScene("createAccount"); // Replace with your actual create account scene name
    }
}
