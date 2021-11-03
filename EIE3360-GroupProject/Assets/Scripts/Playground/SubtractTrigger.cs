using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtractTrigger : MonoBehaviour {

    private GameObject[] objectCount;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Clickable" && CheckCount())
            other.gameObject.tag = "Operation";
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Operation")
            other.gameObject.tag = "Clickable";
    }

    // Assign one and only one in this tag.
    bool CheckCount()
    {
        objectCount = GameObject.FindGameObjectsWithTag("Operation");
        if (objectCount.Length <= 2)
            return true;
        else
            return false;
    }

}
