using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTrigger : MonoBehaviour {

    private BoxCollider box;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Clickable")
        {
            box = other.GetComponent<BoxCollider>();
            box.isTrigger = !box.isTrigger;
        }

    }
}
