using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic : MonoBehaviour
{
    public float forwardVel = 5;
    public float backwardVel = 1;
    public float rotateVel = 50;
    // jump var
    public float jumpVel = 25;
    public float distToGrounded = 0.1f;
    public LayerMask ground;
    Vector3 velocity = Vector3.zero;


    public float downAccel = 0.75f;




    public float inputDelay = 0.1f;
    public string FORWARD_AXIS = "Vertical";
    public string TURN_AXIS = "Horizontal";
    public string JUMP_AXIS = "Space";


    Quaternion targetRotation;
    Rigidbody rBody;
    float forwardInput, turnInput, backwardInput,jumpInput;
    public Animator anim;

    public Quaternion TargetRotaion
    {
        get { return targetRotation; }
    }

    // Use this for initialization

    //bool
    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGrounded, ground);
    }



    void Start()
    {
        anim = GetComponent<Animator>();

        targetRotation = transform.rotation;
        if (GetComponent<Rigidbody>())
            rBody = GetComponent<Rigidbody>();
        else
            Debug.LogError("the character needs RigidBody");
        forwardInput = turnInput = 0;
        backwardInput = 0;
        jumpInput = 0;

    }

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        backwardInput = Input.GetAxis("Vertical");
        //jumpInput = Input.GetAxisRaw("Space");
    }

    // Update is called once per frame
    void Update()
    {

        //forward

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            anim.SetBool("Bjog", false);
            anim.SetFloat("Speed", 1);
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            anim.SetFloat("Speed", 0);
        }

        //backward

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            forwardVel = backwardVel;
            anim.SetBool("Bjog", false);
            anim.SetFloat("BackSpeed", 1);
        }


        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            anim.SetFloat("BackSpeed", 0);


            StartCoroutine(Wait(15f));

            forwardVel = 5;
        }

        GetInput();
        Turn();

    }

    public IEnumerator Wait(float delayInSecs)
    {
        yield return new WaitForSeconds(delayInSecs);
    }

    void FixedUpdate()
    {
        Run();
       // Jumper();
        //rBody.velocity = transform.forward * forwardInput * forwardVel;

    }

    void Run()
    {


        if (Mathf.Abs(forwardInput) > inputDelay)
        {
            //Move
            rBody.velocity = transform.forward * forwardInput * forwardVel;
            //velocity.z = forwardVel * forwardInput;
        }
        else  //zero Velocity
               rBody.velocity = Vector3.zero;
           // velocity.z = 0;


    }
    void Turn()
    {
        targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;
    }

    void Jumper()
    {
        if  (jumpInput > 0 &&  Grounded())
        {
            //jump
            velocity.y = jumpVel;
        }
        else if(jumpInput == 0 && Grounded())
            {
            //zero out velocity.y
            velocity.y = 0;
        }
        else
        {
            //decrease velocity.y
            velocity.y -=downAccel;
        }

    }
}

