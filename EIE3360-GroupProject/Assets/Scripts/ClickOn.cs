using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOn : MonoBehaviour {

    [SerializeField]
    private Material red;
    [SerializeField]
    private Material green;

    private MeshRenderer myRend;

    [HideInInspector]
    public bool currentlySelected = false;

    // Disable dragging function in the OnClickDrag()
    public bool drag;

    private Vector3 mOffset;

    private float mZCoord;

    // Use this for initialization
    void Start() {
        //myRend = GetComponent<MeshRenderer>();
        // Add itself as a selectable object.
        //Camera.main.gameObject.GetComponent<EngineManager>().selectableObjects.Add(this.gameObject);
        //ClickMe();
    }

    public void ClickMe()
    {
        if (currentlySelected == false)
        {
            myRend.material = red;
        }
        else
        {
            myRend.material = green;
        }
    }

    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();
        //Debug.Log("Ini: " + transform.position);

    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinate (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if (drag == false) return;
        transform.position = GetMouseWorldPos() + mOffset;
        //Debug.Log("Drag: " + transform.position);
    }
}
