#if UNITY_EDITOR_WIN
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System.Linq;
#endif
using UnityEngine;

public class VoiceControlEIE3360 : MonoBehaviour {
    public float mainSpeed = 10.0f; //regular speed
    public float stepSize = 1.0f; // step size of movement

    private float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    private float maxShift = 1000.0f; //Maximum speed when holdin gshift
    private float camSens = 0.25f; //How sensitive it with mouse
    private float totalRun = 1.0f; // for camera viewing angle
    private float wanderTimer;

    private float viewingAxisX;
    private float viewingAxisY;
    private float viewingAsixZ;

    // movement boolean value
    private bool moveForward = false;
    private bool moveBackward = false;
    private bool moveUp = false;
    private bool moveDown = false;  
    private bool moveRight = false;
    private bool moveLeft = false;

    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

#if UNITY_EDITOR_WIN
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();     
#endif

    GameObject camera;
    GameObject character;
    GameObject leapRig;

    private CameraMovement cameraMovement;
    private CharacterMovement characterMovement; 

    private void Awake()
    {
        character = GameObject.FindGameObjectWithTag("Character");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        leapRig = GameObject.FindGameObjectWithTag("LeapRig");

        characterMovement = character.GetComponent<CharacterMovement>();
        cameraMovement = camera.GetComponent<CameraMovement>();
        Debug.Log("Voice Control Awake");
    }

    private void Start()
    {
        Debug.Log("Start");


#if UNITY_EDITOR_WIN
        { // dictionary
        actions.Add("testing", testing);

        actions.Add("move forward", Forward);
            actions.Add("forward", Forward);
            actions.Add("go forward", Forward);   
        actions.Add("move up", Up);
            actions.Add("up", Up);
            actions.Add("go up", Up);
        actions.Add("move down", Down);
            actions.Add("go down", Down);
            actions.Add("down", Down);
        actions.Add("move back", Back);
            actions.Add("step back", Back);
            actions.Add("go back", Back);
        actions.Add("move left", Leftward);
            actions.Add("go left", Leftward);
        actions.Add("move right", Rightward);
            actions.Add("go right", Rightward);

        actions.Add("come here", summonChar);
            actions.Add("come", summonChar);
            actions.Add("follow me", summonChar);
            actions.Add("come to me", summonChar);

        actions.Add("Hello", greetingChar);
            actions.Add("Hi", greetingChar);
            actions.Add("Hey", greetingChar);

        actions.Add("Look back", LookBack);
        actions.Add("Turn Right", TurnRight);
        actions.Add("Turn right by 10 degrees", TurnRightBy10Deg);
        actions.Add("Turn right by 20 degrees", TurnRightBy20Deg);
        actions.Add("Turn right by 30 degrees", TurnRightBy30Deg);
        actions.Add("Turn right by 40 degrees", TurnRightBy40Deg);
        actions.Add("Turn right by 50 degrees", TurnRightBy50Deg);
        actions.Add("Turn right by 60 degrees", TurnRightBy60Deg);
        actions.Add("Turn right by 70 degrees", TurnRightBy70Deg);
        actions.Add("Turn right by 80 degrees", TurnRightBy80Deg);

        actions.Add("Turn Left", TurnLeft);
        actions.Add("Turn left by 10 degrees", TurnLeftBy10Deg);
        actions.Add("Turn left by 20 degrees", TurnLeftBy20Deg);
        actions.Add("Turn left by 30 degrees", TurnLeftBy30Deg);
        actions.Add("Turn left by 40 degrees", TurnLeftBy40Deg);
        actions.Add("Turn left by 50 degrees", TurnLeftBy50Deg);
        actions.Add("Turn left by 60 degrees", TurnLeftBy60Deg);
        actions.Add("Turn left by 70 degrees", TurnLeftBy70Deg);
        actions.Add("Turn left by 80 degrees", TurnLeftBy80Deg);
    }
#endif

#if UNITY_EDITOR_WIN
        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
#endif
    }

    void Update()
    {
        if(!cameraMovement.useMouseControl)
        {
            //Debug.Log("Use voice control now");
            lastMouse = new Vector3(viewingAxisX, viewingAxisY+180, viewingAsixZ); // voice control view
            camera.transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        }else if(cameraMovement.useMouseControl){
            //Debug.Log("No longer use voice control now");
        }
             
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
        Vector3 newPosition = camera.transform.position;
        if (Input.GetKey(KeyCode.Space))
        { //If player wants to move on X and Z axis only
            camera.transform.Translate(p);
            newPosition.x = camera.transform.position.x;
            newPosition.z = camera.transform.position.z;
            camera.transform.position = newPosition;
        }
        else
        {
            camera.transform.Translate(p);
            leapRig.transform.Translate(p);
        }
    }
    

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W) || moveForward)
        {
            p_Velocity += new Vector3(0, 0, stepSize);
            moveForward = false;
            
        }
        if (Input.GetKey(KeyCode.S) || moveBackward)
        {
            p_Velocity += new Vector3(0, 0, -stepSize);
            moveBackward = false;
        }
        if (Input.GetKey(KeyCode.A) || moveLeft)
        {
            p_Velocity += new Vector3(-stepSize, 0, 0);
            moveLeft = false;
        }
        if (Input.GetKey(KeyCode.D) || moveRight)
        {
            p_Velocity += new Vector3(stepSize, 0, 0);
            moveRight = false;
        }
        if (moveUp)
        {
            p_Velocity += new Vector3(0, stepSize, 0);
            moveUp = false;
        }
        if (moveDown)
        {
            p_Velocity += new Vector3(0, -stepSize, 0);
            moveDown = false;
        }
        return p_Velocity;
    }
#if UNITY_EDITOR_WIN
    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }
#endif

    private void Forward() { moveForward = true; }
    private void Back() { moveBackward = true; }
    private void Up() { moveUp = true; }
    private void Down() { moveDown = true; }
    private void Leftward() { moveLeft = true; }
    private void Rightward() { moveRight = true; }

    private void summonChar() { 
        cameraMovement.summonCharacter = true; 
        cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f;
        }
    private void greetingChar(){ characterMovement.greeting(); }

    private void LookBack() { viewingAxisY += 180; 
    }
    private void TurnRight()        { viewingAxisY += 90;
    cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy10Deg() { viewingAxisY += 10;
    cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy20Deg() { viewingAxisY += 20;
    cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy30Deg() { viewingAxisY += 30;
    cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy40Deg() { viewingAxisY += 40;
    cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy50Deg() { viewingAxisY += 50;
    cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy60Deg() { viewingAxisY += 60;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy70Deg() { viewingAxisY += 70;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnRightBy80Deg() { viewingAxisY += 80;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }

    private void TurnLeft()        { viewingAxisY -= 90;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy10Deg() { viewingAxisY -= 10;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy20Deg() { viewingAxisY -= 20;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy30Deg() { viewingAxisY -= 30;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy40Deg() { viewingAxisY -= 40;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy50Deg() { viewingAxisY -= 50;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy60Deg() { viewingAxisY -= 60;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy70Deg() { viewingAxisY -= 70;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }
    private void TurnLeftBy80Deg() { viewingAxisY -= 80;cameraMovement.idelTimeExceeded = false;
        cameraMovement.idelTimer = 0f; }

    private void testing() { Debug.Log("Testing"); }

}
