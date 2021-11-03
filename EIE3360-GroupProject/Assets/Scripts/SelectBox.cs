using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBox : MonoBehaviour {

    [SerializeField]
    private RectTransform selectSquareImage;

    Vector3 startPos;
    Vector3 endPos;

	// Use this for initialization
	void Start () {
        selectSquareImage.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
        // Left Mouse Clicked
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            // Locate the current mouse position using raycast
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                startPos = hit.point;
            }
        }

        // Left Mouse Lifted
        if(Input.GetMouseButtonUp(0))
        {
            selectSquareImage.gameObject.SetActive(false);
        }

        // Left Mouse On Hold
        if (Input.GetMouseButton(0))
        {
            // The green square appears
            if(!selectSquareImage.gameObject.activeInHierarchy)
            {
                selectSquareImage.gameObject.SetActive(true);
            }

            endPos = Input.mousePosition;

            Vector3 squareStart = Camera.main.WorldToScreenPoint(startPos);
            squareStart.z = 0f;


            float sizeX = Mathf.Abs(squareStart.x - endPos.x);
            float sizeY = Mathf.Abs(squareStart.y - endPos.y);

            selectSquareImage.sizeDelta = new Vector2(sizeX, sizeY);

            Vector3 centre = (squareStart + endPos) / 2f;

            selectSquareImage.position = centre;
        }

	}
}
