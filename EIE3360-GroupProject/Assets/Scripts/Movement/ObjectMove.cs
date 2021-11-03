using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour {

    public float movementSpeed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(-movementSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0f, -movementSpeed * Input.GetAxis("Vertical") * Time.deltaTime);
	}
}
