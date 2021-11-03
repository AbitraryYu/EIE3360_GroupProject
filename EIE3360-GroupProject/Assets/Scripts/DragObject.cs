using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour {

    public string testString = "hi";

    private Vector3 mOffset;

    private float mZCoord;

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
        if (enabled == false) return;
        transform.position = GetMouseWorldPos() + mOffset;
        //Debug.Log( "Drag: " + transform.position);
    }
}
