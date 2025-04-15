using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadVRScene()
    {
        StartCoroutine(LoadVRSceneRoutine());
    }

    private IEnumerator LoadVRSceneRoutine()
    {
        // Deinitialize XR before switching
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            yield return null; // Wait for XR to shut down
        }

        // Wait a short delay to prevent race conditions
        yield return new WaitForSeconds(0.5f);

        // Load the VR scene
        SceneManager.LoadScene("VR Scene");
        yield return null;

        // Reinitialize XR for VR
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Failed to initialize XR");
        }
        else
        {
            Debug.Log("XR Initialized");
        }
    }
}