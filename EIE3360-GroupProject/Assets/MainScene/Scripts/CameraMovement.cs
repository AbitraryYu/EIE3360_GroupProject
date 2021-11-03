#if UNITY_EDITOR_WIN
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#endif
using UnityEngine;

public class CameraMovement : MonoBehaviour
{ 
    
    public float mainSpeed = 10.0f; //regular speed
    public float maxIdelTime = 3.0f; // maximum idel time in second
    public float stepSize = 1.0f; // step size of movement
    
    private float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    private float maxShift = 1000.0f; //Maximum speed when holdin gshift
    private float camSens = 0.25f; //How sensitive it with mouse
    private float totalRun = 1.0f; // for camera viewing angle
    private float wanderTimer;

    public bool isIdel; // player idel
    public bool idelTimeExceeded; // player idel exceeded given maximum time
    public bool summonCharacter; // player summons the character
    public float idelTimer = 0f; // timer of player's idel time

    public bool useMouseControl = false;

    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

    GameObject character;
    private CharacterMovement characterMovement;

    private void Awake(){
        character = GameObject.FindGameObjectWithTag("Character");
            characterMovement = character.GetComponent<CharacterMovement>();
    }
    
    private void Start()
    {
        isIdel = false;
        summonCharacter = false;
    }

    
    void Update()
    {
        /*
        Vector3 p = GetBaseInput();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }
        else
        {
            transform.Translate(p);
        }

         */
        
        if(useMouseControl){
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            //Debug.Log("Use mouse control now");
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0); // mouse control view
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        }else if(!useMouseControl){
            //Debug.Log("No longer use mouse control");
        }
            
    }

    private void FixedUpdate(){
        if(!Input.anyKey)
        {
            //Starts counting when no button is being pressed
            idelTimer = idelTimer + 1;
            isIdel = true;
        }
        else
        {
            // If a button is being pressed restart counter to Zero
            idelTimer = 0;
            isIdel = false;
            idelTimeExceeded = false;
        }
                      
        //Now after preset frames of nothing being pressed it will do activate this if statement
        if(idelTimer == maxIdelTime*60) {
            Debug.Log(maxIdelTime + " seconds ( "+maxIdelTime*60+" frames) passed with no input");
            idelTimeExceeded = true;
            //Now you could set time too zero so this happens every preset frames
            idelTimer = 0;
        }
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, stepSize);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -stepSize);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-stepSize, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(stepSize, 0, 0);  
        }
        return p_Velocity;
    }
}