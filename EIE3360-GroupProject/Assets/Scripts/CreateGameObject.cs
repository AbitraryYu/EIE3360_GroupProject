using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGameObject : MonoBehaviour {

    // Get all gameobjects for full utilzation.
    GameObject[] allObjects;



    // Use this for initialization
    void Start () {
        // Create a plane, sphere and cube in the Scene
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(0, 0.5f, 0);

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(0, 1.5f, 0);

        GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        capsule.transform.position = new Vector3(2, 1, 0);

        GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        cylinder.transform.position = new Vector3(-2, 1, 0);
    }
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Manually create a new sphere
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(0, 1.0f, 0);

            // Add rigidbody physics
            Rigidbody rigidbody = sphere.AddComponent<Rigidbody>();
            rigidbody.useGravity = true;
        }
       
        if (Input.GetKeyDown(KeyCode.C))
        {
            Check();
        }

        
    }

    void Check()
    {
        // Get all gameobjects for full utilzation.
        allObjects = FindObjectsOfType<GameObject>();


        foreach (GameObject go in allObjects)
        {
            //if (go.activeInHierarchy)
            //{
            //    Debug.Log(go + " is an active object");
            //    Debug.Log("Current location of x " + go + " is " + go.transform.position.x);
            //    Debug.Log("Current location of y " + go + " is " + go.transform.position.y);
            //    Debug.Log("Current location of z " + go + " is " + go.transform.position.z);
            //}

            // If the object is out of the screen, destroy.
            if (go.transform.position.y < -100 || go.transform.position.z < -100)
                Destroy(go);
        }
    }

    void OutOfBoundsDestroy()
    {
        // If the object is out of the screen, destroy.
        if (transform.position.y < -100 || transform.position.z < -100)
            Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
        enabled = false;
    }

}
