using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour {

	public static GameLevel instance;//

	public Camera mainCarmera;  //主摄像机
	public MainChar mainChar;   //主角


	public bool lockCursor = false;//锁定光标

	private Transform birthPosTran;//出生位置

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
		//1.出生位置
		birthPosTran = this.transform.Find("BrithPos").transform;
		//2.创建MainChar
		CreateMainChar();
		//3.获取MainCarmera
		mainCarmera = CreateMainCarmera();

	}
	
	// Update is called once per frame
	void Update () {
		//
		MainCharMove ();
		//
		if (Input.GetKeyDown(KeyCode.F2)) {
			if (lockCursor == false) {
				lockCursor = true;
			} else {
				lockCursor = false;
			}
		}

		//
		AdjustCamFov();
		//
		ClickModels();
	}


	//主摄影机
	Camera CreateMainCarmera()
	{
		Camera carm;
		carm = Camera.main;

		carm.cullingMask = ~(1<<5);//除去UI层
		carm.fieldOfView = 48;
		carm.depth = -1;
		carm.renderingPath = RenderingPath.DeferredShading;
		carm.allowHDR = true;

		carm.transform.position = Vector3.zero;
		carm.transform.rotation = Quaternion.Euler (Vector3.zero);

		CarmeraFollow carmFollow =  carm.gameObject.AddComponent<CarmeraFollow> ();
		carmFollow.target = mainChar.transform.gameObject;
		carmFollow.offsetPos = new Vector3 (0, 1.65f, 0);

		return carm;
	}

	//调整畸变
	void AdjustCamFov()
	{
		if (mainCarmera != null) {
			if (Input.GetKey(KeyCode.Q)) {
				mainCarmera.fieldOfView += 1;
			}
			else if (Input.GetKey(KeyCode.E)) {
				mainCarmera.fieldOfView -= 1;
			}
		}
	}


	//主角
	void CreateMainChar()
	{
		GameObject mainCharObj = new GameObject ();

		mainCharObj.name = "@MainChar";
		mainCharObj.transform.parent = this.transform;
		mainCharObj.transform.position = birthPosTran.position;
		mainCharObj.transform.rotation = Quaternion.identity;

		CharacterController charController = mainCharObj.AddComponent<CharacterController> ();
		charController.radius = 0.35f;
		charController.height = 1.65f;
		charController.skinWidth = 0.01f;
		charController.slopeLimit = 20.0f;
		charController.center = new Vector3(0,0.825f,0);

		mainChar = mainCharObj.AddComponent<MainChar> ();
	}

	//主角移动
	void MainCharMove()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		Vector3 moveDir = new Vector3 (horizontal, 0, vertical);

		if (mainChar != null) {
			mainChar.moveDir = moveDir;
			mainChar.moveSpeed = 2.0f;
		}
	}

	//点击模型
	void ClickModels()
	{
//		if (Input.GetMouseButtonDown(0)) {
//			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
//			RaycastHit hitInfo;
//			if (Physics.Raycast(ray,out hitInfo)) {
//				Debug.DrawLine (ray.origin, hitInfo.point,Color.red);
//				GameObject hitObj = hitInfo.collider.gameObject;
//				Debug.Log ("点击到" + hitObj.name);
//				if (hitObj.GetComponent<MaterialBaseManage>() != null) {
//					int currentMaterialsNum = 0;
//					currentMaterialsNum++;
//					Material replaceMat = hitObj.GetComponent<MaterialBaseManage> ().materiallist[currentMaterialsNum];
//					hitObj.GetComponent<MaterialBaseManage> ().ChangeMaterial (replaceMat);
//				}
//			}
//		}
	}
}
