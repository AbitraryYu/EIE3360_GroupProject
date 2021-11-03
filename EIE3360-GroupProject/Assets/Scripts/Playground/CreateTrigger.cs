using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CreateTrigger : NetworkBehaviour {

    // The object you want to clone
    public GameObject Clone;
	public Transform SpawnPoint;

    //public float yHeight = 3f;
    public float scale = 0.01f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
		
		if(isLocalPlayer)
			return;
		
		if(Input.GetKey(KeyCode.K))
		{
			CmdCreateObjects();
		}
    }

    private void OnTriggerEnter(Collider other)
    {
		if(isLocalPlayer)
			return;
		
        if (other.tag == "Player")
        {
            CmdCreateObjects();
            Debug.Log("created");
        }

        Debug.Log("entered");

    }



	[Command]
    void CmdCreateObjects()
    {
         //GameObject obj = Instantiate(Clone, transform.position + new Vector3(0, yHeight,0), Quaternion.identity) as GameObject;
         GameObject obj = Instantiate(Clone, SpawnPoint);
		 obj.transform.localScale = Vector3.one * scale;
		 NetworkServer.Spawn(obj);
		 Debug.Log("Spawn");
    }
}
