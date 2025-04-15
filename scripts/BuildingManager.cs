using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Unity.Mathematics;

public class BuildingManager : MonoBehaviour
{
        public GameObject[] objects;
        public GameObject pendingObject;
        private Vector3 pos;

        private RaycastHit hit;
         [SerializeField] private LayerMask layerMask;
         public GameObject furnitureParent;

         public bool canplace = true;
         
         public float mouseWheelRotation;

    void Update()
    {
        if(pendingObject != null)
        {
            pendingObject.transform.position = pos;
            if(Input.GetMouseButtonDown(0) && canplace)
            {
                PlaceObject();
            }
        }
    }

    public void PlaceObject()
    {
        pendingObject = null;
    }

    private void FixedUpdate()
    {
        furnitureParent = GameObject.Find("Room Builder/Furniture");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
            // rotateFromMouseWheel();
            //pendingObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.point);
        }
    }

    public void SelectObject(int index)
    {
        pendingObject = Instantiate(objects[index], pos, transform.rotation, furnitureParent.transform); // place new object will go to
    }
}
