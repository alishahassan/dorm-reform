using UnityEngine;
using System.Collections;
using  UnityEngine.SceneManagement;
using UnityEditor.MPE;

public class CopyObject : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}