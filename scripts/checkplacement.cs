using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class checkplacement : MonoBehaviour
{
    buildingmanager buildingManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildingManager = GameObject.Find("buildingmanager").GetComponent<buildingmanager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("object")) { //"stackable_object" = canplace
            buildingManager.canplace = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        buildingManager.canplace = true;
    }
}
