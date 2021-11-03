using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class ProBoolean : MonoBehaviour
{
    GameObject composite;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize two new meshes in the scene
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 1.3f;

        // Perform boolean operation
        CSG_Model result = Boolean.Union(cube, sphere);

        // Create a gameObject to render the result
        composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        composite.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
