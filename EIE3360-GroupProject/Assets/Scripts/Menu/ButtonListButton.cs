using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Parabox.CSG;

public class ButtonListButton : MonoBehaviour {

    public EngineManager engineManager;
    //public SelectBox selectBoxScript;

    [SerializeField]
    private Text myText;
    [SerializeField]
    private ButtonListControl buttonControl;

    private ClickOn[] ClickOnScripts;
    private GameObject[] Interactable;

    private string myTextString;

    void Start()
    {
    }

    public void SetText(string textString)
    {
        myTextString = textString;
        myText.text = textString;
    }

    public void OnClick()
    {
        Reset();
        DoOperation(myTextString);
    }

    /*
        A List of operations.
    */
    void DoOperation(string myTextString)
    {
        switch (myTextString)
        {
            // Toggle the status of the user from Moving anything.
            case "Move":
                if (FindScript())
                {
                    //// We use the last index as a stub to indicate if all interactables have drag or not.
                    //Debug.Log(myTextString + " is now at state " + ClickOnScripts[Interactable.Length - 1].drag +
                    //"\n Note that this is the last index of the interactable, this doesn't mean every interactable have the same state, if there are logical errors. ");
                }
                else
                    Debug.Log("Please create at least one interactable");
                break;
            case "Create":
                engineManager.create = !engineManager.create;
                break;
            case "Delete":
                engineManager.DeleteObjects();
                break;
            case "Subtract":
                BoolOperation(engineManager.selectableObjects[0], engineManager.selectableObjects[1]);
                break;
            case "Toggle":
                engineManager.toggle = !engineManager.toggle;
                break;
            default:
                Debug.Log("Invaild operation");
                break;
        }
    }

    /*
     Reset all states in the engine as well as in the interactable.
    */
    void Reset()
    {
        engineManager.create = false;
        engineManager.toggle = false;
        //if (ClickOnScripts.Length == 0) return;
        //// Uncheck the status of the drag for each interactable. This affects OnMouseDrag().
        //foreach (ClickOn Clickon in ClickOnScripts)
        //{
        //    Clickon.drag = false;
        //}
    }

    /*
        FindScript: Attach all ClickOn scripts to the GUI such that it can control whether it is enabled or not.
                    If there isn't anything yet. Return false.
        return: True / False.
    */
    public bool FindScript()
    {
        // Find all the interactables by using the Tag system. Return GameObject[]
        Interactable = GameObject.FindGameObjectsWithTag("Clickable");

        // If there aren't anything created just yet.
        if (Interactable.Length == 0) return false;

        // Create memory locations for ClickOnScripts.
        ClickOnScripts = new ClickOn[Interactable.Length];

        // Attach all ClickOn scripts from the instantiated interactables.
        for (int i = 0; i < Interactable.Length; i++)
            ClickOnScripts[i] = Interactable[i].GetComponent<ClickOn>();

        // Invert the status of the drag for each interactable. This affects OnMouseDrag().
        foreach (ClickOn Clickon in ClickOnScripts)
        {
            Clickon.drag = !Clickon.drag;
        }

        return true;
    }

    public void BoolOperation(GameObject gameObject1, GameObject gameObject2)
    {
        GameObject composite;

        // Perform boolean operation
        CSG_Model result = Boolean.Subtract(gameObject1, gameObject2);

        // Create a gameObject to render the result
        composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        composite.AddComponent<Rigidbody>().useGravity = false;
        composite.transform.position = gameObject1.transform.position + new Vector3(0, 1, 0);
    }
}
