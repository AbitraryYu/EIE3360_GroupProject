using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Parabox.CSG;
using Leap.Unity.Interaction;


public class Operation : NetworkBehaviour {

    // Gameobjects: The interactable
    public static GameObject[] gameObjects;
    public Transform SpawnPoint;
    public GameObject Pivot;

    // X,Y,Z offsets
    public float xOffset;
    public float yOffset;
    public float zOffset;

    // For scaling
    static protected int sign = 1;

    // Use this for initialization
    void Start () {
        //gameObject2 = GameObject.FindWithTag("Table");
        gameObjects = new GameObject[2];
    }
	
	// Update is called once per frame
	void Update () {
        gameObjects = GameObject.FindGameObjectsWithTag("Operation");
    }

    /*
        Known existing limitation:
            After boolean operation, the boolean gameobject's pivot is very different from its center. The gameobject will move when scaling.
            No solutions can be found to tacklet it yet.
            This is because when boolean gameobjects undergo scaling, it will scale according to the pivot. 
         */
    public void BoolOperation(GameObject gameObject1, GameObject gameObject2, int op)
    {
        GameObject composite;
        CSG_Model result = null;
        string myString = ""; // for abitrary strings

        switch (op)
        {
            case 0:
                // Perform boolean operation
                result = Boolean.Subtract(gameObjects[0], gameObjects[1]);
                break;
            case 1:
                result = Boolean.Union(gameObjects[0], gameObjects[1]);
                break;
            default:
                Debug.Log("Wrong Operation");
                break;
        }

        // Exception handling.
        if (result == null) return;

        // Create a gameObject to render the result
        // First of all, we need the define the following:
        // 1. Rigidbody
        // 2. Colliders (so that it can be grasped by Leap Motion)
        // 3. InteractionBehaviour script (enable Leap Motion to grab the obj)
        // 4. Mesh Filter
        // 5. Mesh Renderer (to see the object in screen)
        // 6. Assign tag Clickable (for further operations)
        composite = new GameObject();
        composite.name = StringRandom(myString);         // Create abitrary string
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.GetComponent<MeshFilter>().mesh.name = composite.name;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        composite.AddComponent<Rigidbody>().useGravity = false;
        composite.AddComponent<MeshCollider>().convex = true;
        composite.GetComponent<MeshCollider>().isTrigger = true;
        composite.AddComponent<InteractionBehaviour>();
        composite.AddComponent<NetworkIdentity>();
        composite.AddComponent<NetworkTransform>().syncSpin = true;
        composite.tag = "Clickable";

#if UNITY_EDITOR

        // Add it to prefab
        CreatePrefab(composite);

        // Create the mesh name
        // Must not comment these two lines, otherwise mesh filter can't be applied to prefab.
        AssetDatabase.CreateAsset(composite.GetComponent<MeshFilter>().mesh, "Assets/" + composite.name + ".fbx"  );
        AssetDatabase.SaveAssets();
#endif

    }

    public void Scaling(int op, GameObject gameObject1 = null, GameObject gameObject2 = null)
    {
        Debug.Log(sign);

        // If he stays longer, scaling multiplier increase.
        switch(op)
        {
            // General scaleup
            case 0:
                gameObject1.transform.localScale += Vector3.one * Time.deltaTime * sign;
                gameObject2.transform.localScale += Vector3.one * Time.deltaTime * sign;
                break;
            // x direction scale up/down
            case 1:
                gameObject1.transform.localScale += new Vector3(1,0,0) * Time.deltaTime * sign;
                gameObject2.transform.localScale += new Vector3(1, 0, 0) * Time.deltaTime * sign;
                break;
            // y direction scale up/down
            case 2:
                gameObject1.transform.localScale += new Vector3(0, 1, 0) * Time.deltaTime * sign;
                gameObject2.transform.localScale += new Vector3(0, 1, 0) * Time.deltaTime * sign;
                break;
            // z direction scale up/down
            case 3:
                gameObject1.transform.localScale += new Vector3(0, 0, 1) * Time.deltaTime * sign;
                gameObject2.transform.localScale += new Vector3(0, 0, 1) * Time.deltaTime * sign;
                break;
            default:
                break;
        }
    }

    protected int ScaleToggle(bool scaleup)
    {
        // Scale Up or down?
        return sign = (scaleup == true) ? 1 : -1;
    }


#if UNITY_EDITOR
    [MenuItem("Game/Boolean Prefab")]
    static void CreatePrefab(GameObject obj)
    {
        // Keep track of the currently selected GameObject

        // Set the path as within the Assets folder,
        // and name it as the GameObject's name with the .Prefab format
        string localPath = "Assets/" + obj.name + ".prefab";

        // Make sure the file name is unique, in case an exising Prefab has the same name
        localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

        //Check if the Prefab and/or name already exists at the path
        if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
        {
            //Create dialog to ask if User is sure they want to overwrite existing prefab
            if (EditorUtility.DisplayDialog("Are you sure?",
                    "The prefab already exists. Do you want to overwrite it?",
                    "Yes",
                    "No"))
            //If the user presses the yes button, create the Prefab
            {
                CreateNew(obj, localPath);
            }
        }
        //If the name doesn't exist, create the new Prefab
        else
        {
            Debug.Log(obj.name + " is not a prefab, will convert");
            CreateNew(obj, localPath);
        }
    }

    // Disable the menu item if no selection is in place
    [MenuItem("Game/Boolean Prefab", true)]
    static bool ValidateCreatePrefab()
    {
        return Selection.activeGameObject != null;
    }

    static void CreateNew(GameObject obj, string localPath)
    {
        //Create a new prefab at the path given
        Object prefab = PrefabUtility.CreatePrefab(localPath, obj);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }

    public void Apply(GameObject composite, Object union, Object subtract, bool isUnion)
    {
        if (isUnion)
            PrefabUtility.ReplacePrefab(composite, union);
        else
            PrefabUtility.ReplacePrefab(composite, subtract);
    }

#endif

    string StringRandom(string myString)
    {
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789"; //add the characters you want
        int charAmount = Random.Range(1, 4); //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++)
        {
            myString += glyphs[Random.Range(0, glyphs.Length)];
        }
        return myString;
    }

}
