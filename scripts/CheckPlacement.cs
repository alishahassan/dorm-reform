using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CheckPlacement : MonoBehaviour
{
    BuildingManager buildingManager;
    void Start()
    {
        buildingManager = GameObject.Find("buildingmanager").GetComponent<buildingmanager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("object"))
        {
            buildingManager.canplace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        buildingManager.canplace = true;
    }
}
