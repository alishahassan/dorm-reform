using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;



public class buildingmanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        public GameObject[] objects;
        public GameObject pendingObject;
        private Vector3 pos;

        private RaycastHit hit;
         [SerializeField] private LayerMask layerMask;
         public GameObject furnitureParent; // "Furniture" object that allows you to reset room

         public bool canplace = true;


    // Update is called once per frame
    void Update()
    {
        if(pendingObject != null) {
            pendingObject.transform.position = pos;

            if(Input.GetMouseButtonDown(0) && canplace) {
                PlaceObject();
            }

        }
    }

    public void PlaceObject() {
        pendingObject = null; // object being placed will no longer move with mouse
    }

    private void FixedUpdate()
    {
        furnitureParent = GameObject.Find("Room Builder/Furniture");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Raycast from mouse position on screen -> world
        //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadDefaultValue());

        if(Physics.Raycast(ray, out hit, 1000, layerMask)) {
            pos = hit.point;
        }
        
    }


    public void SelectObject(int index) {
        pendingObject = Instantiate(objects[index], pos, transform.rotation, furnitureParent.transform); // place new object will go to
    }

    
}
