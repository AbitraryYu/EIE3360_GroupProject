using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChar : MonoBehaviour {


	public Vector3 moveDir;
	public float moveSpeed;

	private CharacterController charController;

	// Use this for initialization
	void Start () {
		charController = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//移动
		InternalMove ();
		//
		LookRotation();
	}


	//
	void InternalMove()
	{
		if (moveDir != Vector3.zero) {
			if (charController.isGrounded) {
				moveDir = transform.TransformDirection (moveDir);
				moveDir *= moveSpeed;
			}

			moveDir.y -= 20.0f * Time.deltaTime;
			charController.Move (moveDir * Time.deltaTime);
		}
	}

	//朝向
	void LookRotation()
	{
		float yRot = GameLevel.instance.mainCarmera.transform.eulerAngles.y;

		Vector3 rot = transform.eulerAngles;
		rot.y = yRot;
		transform.eulerAngles = rot;
	}

}
