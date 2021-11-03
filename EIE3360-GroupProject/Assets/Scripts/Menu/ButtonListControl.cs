using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListControl : MonoBehaviour {



    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private string[] stringArray;

    private List<GameObject> buttons;

    void Start()
    {

        buttons = new List<GameObject>();

        if (buttons.Count > 0)
        {
            foreach (GameObject button in buttons)
            {
                Destroy(button.gameObject);
            }

            buttons.Clear();
        }

        foreach (string str in stringArray)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            // Access attached button script
            button.GetComponent<ButtonListButton>().SetText(str);

            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }
              
    }

    public void ButtonClicked(string myTextString)
    {
        Debug.Log(myTextString);
    }

}
