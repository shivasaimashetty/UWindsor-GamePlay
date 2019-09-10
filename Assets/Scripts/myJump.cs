using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myJump : MonoBehaviour {
    public bool onGround;
    Rigidbody myBody;
    public float myg = 15f;



    // Use this for initialization
    void Start()
    {
        onGround = true;
        myBody.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onGround)
        {
            if(Input.GetButtonDown("Jump"))
            {
                myBody.velocity = new Vector3(0f, 5f, 0f);
                onGround = false;
            }
        }
           
    }

    void oncollision(Collision any)
    {
        any.gameObject.CompareTag("MyGround");
        onGround = true;
    }
}