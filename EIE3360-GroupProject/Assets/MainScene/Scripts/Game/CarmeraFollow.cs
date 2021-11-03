using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarmeraFollow : MonoBehaviour {

	public  GameObject target;//跟随的目标对象
	public 	Vector3 offsetPos;//偏移位置


	private Vector3 carmerafinalPos;//

	private float sensitivity = 6.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//位置跟随
		carmerafinalPos = new Vector3 (target.transform.position.x + offsetPos.x, target.transform.position.y + offsetPos.y, target.transform.position.z + offsetPos.z);
		transform.position = carmerafinalPos;
		//更改朝向
		if (!GameLevel.instance.lockCursor && Input.GetMouseButton(0)) {
			LookRotation();
		}
	}

	void LookRotation()
	{
		float yRot = Input.GetAxis ("Mouse X") * sensitivity;
		float xRot = Input.GetAxis ("Mouse Y") * sensitivity;

		transform.Rotate (-xRot, yRot, 0);

		Vector3 rotation = transform.eulerAngles;
		rotation.z = 0;
		transform.eulerAngles = rotation;

	}
}
