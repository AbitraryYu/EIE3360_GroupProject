#if UNITY_EDITOR_WIN
using System.Collections;
using System.Collections.Generic;
#endif
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour {

	Animator animator;
    GameObject mainCamera;

    private NavMeshAgent nav;
    private CameraMovement cameraMovement;

    private int wayPointIndex;

    public float walkingSpeed = 1f;
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.1f;
    public Transform[] wanderWayPoints; // An array of transforms for the wander route

    void Awake(){
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            cameraMovement = mainCamera.GetComponent<CameraMovement>();
    }

	// Update is called once per frame
	void Update () {
        nav.speed = walkingSpeed;

        if (cameraMovement.idelTimeExceeded && !cameraMovement.summonCharacter)
        {
            animator.SetFloat("Speed", 1.0f);
            wandering();    
        }
        if (cameraMovement.summonCharacter)
        {
            animator.SetFloat("Speed", 1.0f);
            followCamera();
            cameraMovement.idelTimeExceeded = false;
            cameraMovement.idelTimer = 0f;
        }

        if(!cameraMovement.idelTimeExceeded && !cameraMovement.summonCharacter)
        {
            animator.SetFloat("Speed", 0.0f);
        }
    }

    void FixedUpdate(){
        float h = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        MovementManager(h, y);
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if(collision.gameObject.tag == "Object")
        {
            // start animation
			animator.SetTrigger("Die");
		}
		
    }

    void Rotating(float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        GetComponent<Rigidbody>().MoveRotation(newRotation);
    }

    public void greeting(){
        animator.SetTrigger("Greeting");
    }

    void wandering()
    {
        //Debug.Log("wandering in CharacterMovement");

        if(Vector3.Distance(transform.position,wanderWayPoints[wayPointIndex].
            transform.position) < 0.5){
            if(wayPointIndex==wanderWayPoints.Length -1)
                wayPointIndex = 0;
            else
                wayPointIndex++;
        }else if(!cameraMovement.isIdel)
            nav.SetDestination(transform.position);
        
        //Debug.Log("Waypoint index: "+wayPointIndex );
        nav.SetDestination(wanderWayPoints[wayPointIndex].position);
    }

    private void followCamera() {
        
        Vector3 tempMainCamera = new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z);
        Vector3 tempCharacter = new Vector3(transform.position.x, 0, transform.position.z);

        // Get the distance without y
        float distance = Vector3.Distance(tempMainCamera, tempCharacter);
        //Debug.Log("Distance: " + distance);
        
        transform.LookAt(tempMainCamera);
     
        nav.speed = 1;
        nav.SetDestination(tempMainCamera);
        if(distance < 1) 
        {
            cameraMovement.summonCharacter = false;
            animator.SetFloat("Speed", 0.0f);
        }
            
    }

    private void MovementManager(float horizontal, float vertical){
        //Debug.Log("H: "+horizontal+" V: "+vertical);
            if(horizontal != 0f || vertical != 0f)
            {
                //Rotating(horizontal, vertical);
                animator.SetFloat("Speed", 1f);
            }else
            {
                animator.SetFloat("Speed", 0);
            }
    }
}