using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineManager : MonoBehaviour {

    [SerializeField]
    private LayerMask clickableLayer;

    public List<GameObject> selectedObjects;

    [HideInInspector]
    public List<GameObject> selectableObjects;

    public Vector3 mousePos1;
    public Vector3 mousePos2;

    // The object you want to clone
    public GameObject Clone;

    // Booleans to determine the action.
    public bool toggle = false;
    public bool create = false;

    private Vector3 mOffset;

    private float mZCoord;

    void Awake()
    {
        selectedObjects = new List<GameObject>();
        selectableObjects = new List<GameObject>();
    }

    // Use this for initialization
    void Start() {
        selectedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update() {

        // Right Click -- Deselect all
        if (Input.GetMouseButtonDown(1))
        {
            ClearSelection();
        }

        // Left Click -- Select
        if (Input.GetMouseButtonDown(0))
        {
            // Convert the position of the mouse to a 3d Viewport coordinate
            mousePos1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            // Apply the following method functions according to user's selection in the menu.
            // Please look at ButtonListButton.cs , the Update() function, to see how the states are toggled.
            if (create == true)
                CreateObjects();
            else if (toggle == true)
                ToggleObjects();
        }

        // Left Click Release -- Select multiple
        if (Input.GetMouseButtonUp(0) && toggle == true)
        {
            mousePos2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (mousePos1 != mousePos2)
            {
                SelectObjects();
            }
        }
    }

    void ToggleObjects()
    {
        RaycastHit rayHit;

        //A ray is casted from the point where the mouse is clicked.
        //Stores the result on rayHit
        //Only Hit stuff on clickableLayer(Layer Group)
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, Mathf.Infinity, clickableLayer))
        {
            // Get a ClickOn reference from the rayhit object.
            ClickOn clickOnScript = rayHit.collider.GetComponent<ClickOn>();

            if (Input.GetKey("left ctrl"))
            {
                // Multiple selection
                if (clickOnScript.currentlySelected == false)
                {
                    selectedObjects.Add(rayHit.collider.gameObject);
                    clickOnScript.currentlySelected = true;
                    clickOnScript.ClickMe();
                }
                else
                {
                    selectedObjects.Remove(rayHit.collider.gameObject);
                    clickOnScript.currentlySelected = false;
                    clickOnScript.ClickMe();
                }
            }
            else
            {
                // Return to single toggling, remove all previously toggled objects from left ctrl
                ClearSelection();


                selectedObjects.Add(rayHit.collider.gameObject);
                clickOnScript.currentlySelected = true;

                // Run the ClickMe() in ClickOn.cs
                clickOnScript.ClickMe();
            }



        }
    }

    public void DeleteObjects()
    {
        // If the selectedObjects has no memory allocated/ no objects to delete.
        if (selectedObjects == null)
        {
            Debug.Log("There is nothing to delete.");
            return;
        }

        // Search for currently selected objects.
        foreach (GameObject go in selectedObjects)
        {
            // Loop to delete them one by one.
            Destroy(go);
            // Also update the list to prevent NullExpection.
            selectableObjects.Remove(go);
        }
    }
    
    void CreateObjects()
    {
        RaycastHit rayHit;

        //A ray is casted from the point where the mouse is clicked.
        //Stores the result on rayHit
        //Only Hit stuff on clickableLayer(Layer Group)
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit))
        {
            GameObject obj = Instantiate(Clone, new Vector3(rayHit.point.x, rayHit.point.y, rayHit.point.z), Quaternion.identity) as GameObject;
        }
    }


    void SelectObjects()
    {
        List<GameObject> remObjects = new List<GameObject>();

        // For Computer Selection only, left for debugging.
        if (Input.GetKey("left ctrl") == false)
        {
            ClearSelection();
        }

        // Create a rectangle based on mousePos1 and mousePos2
        Rect selectRect = new Rect(mousePos1.x, mousePos1.y, mousePos2.x - mousePos1.x, mousePos2.y - mousePos1.y);

        foreach (GameObject selectObject in selectableObjects)
        {
            if (selectableObjects != null)
            {
                // This will allow the user to drag the window from left to right OR right to left
                // If objects fall into the rectangle
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(selectObject.transform.position), true))
                {
                    selectedObjects.Add(selectObject);
                    selectObject.GetComponent<ClickOn>().currentlySelected = true;
                    selectObject.GetComponent<ClickOn>().ClickMe();
                }
            }
            else
            {
                remObjects.Add(selectObject);
            }
        }

        // NOTE: You cannot do List removal in iterations, we'll put it after the foreach loop
        // If previously selected objects are NOT in the rectangle, remove it.
        if (remObjects.Count > 0)
        {
            foreach (GameObject rem in remObjects)
            {
                selectableObjects.Remove(rem);
            }

            remObjects.Clear();
        }
    }

    void ClearSelection()
    {
        // Remove all previously selected GameObjects
        foreach (GameObject obj in selectedObjects)
        {
            if (obj != null)
            {
                obj.GetComponent<ClickOn>().currentlySelected = false;
                obj.GetComponent<ClickOn>().ClickMe();
            }
        }

        // Clean the List
        selectedObjects.Clear();
    }
}
