using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class selection : MonoBehaviour
{
   
   public GameObject selectedObject;
   public TextMeshProUGUI objNameText;
   private buildingmanager buildingManager;

   public GameObject objUi;
   
   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildingManager = GameObject.Find("buildingmanager").GetComponent<buildingmanager>();
        // objUi = GameObject.Find("Large/Edit Panel/object");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000)) {
                if(hit.collider.gameObject.CompareTag("object")) {
                    Select(hit.collider.gameObject);
                }
            }
        }
        if(Input.GetMouseButtonDown(1) && selectedObject != null) {
            Deselect();
        }
        
    }

    private void Select(GameObject obj) {
        if(obj == selectedObject) return;
        if(selectedObject != null) Deselect();
        Outline outline = obj.GetComponent<Outline>();
        if(outline == null) obj.AddComponent<Outline>();
        else outline.enabled = true;
        objNameText.text = obj.name;
        objUi.SetActive(true);
        selectedObject = obj;
    }

    private void Deselect() {
        // return;
        objUi.SetActive(false);
        selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
    }
    
    public void Move() {
        buildingManager.pendingObject = selectedObject;
    }

    public void Delete() {
        GameObject objToDestroy = selectedObject;
        Deselect();
        Destroy(objToDestroy);
    }
}
