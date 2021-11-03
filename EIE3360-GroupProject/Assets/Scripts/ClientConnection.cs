using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientConnection : MonoBehaviour {

	public string IpAddress = "192.168.1.145";
	public string Port = "7777";

	// Use this for initialization
	void Start () {
		
		NetworkManager.singleton.networkAddress = IpAddress;
        NetworkManager.singleton.networkPort = int.Parse(Port);
		NetworkManager.singleton.StartClient();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
