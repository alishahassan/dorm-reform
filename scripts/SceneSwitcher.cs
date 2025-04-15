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
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("VR Scene");
        yield return null;

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