using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CameraCtrl : MonoBehaviour
{

    // Use this for initialization
   
    public Transform target;
    public float lookSmooth = 0.09f;
    public Vector3 offsetFromTarget = new Vector3(0, 4, -8);
    public float xTilt = 6;
    public float closeT = 1;

    Vector3 destination = Vector3.zero;
    Vector3 myDestination = Vector3.zero;
    CharacterController charController;
    float rotateVel = 0;

    void Start()
    {
        SetCameraTarget(target);
    }
    void SetCameraTarget(Transform t)
    {
        target = t;
        if (target != null)
        {
            if (target.GetComponent<CharacterController>())
            {
                charController = target.GetComponent<CharacterController>();
            }
            else
                Debug.LogError("the camera ` target needs a character Controller");
        }
        else
            Debug.LogError("camera needs a target");
    }

    // Update is called once per frame

    void Update()
    {
        //mov 
        MoveToTarget();
        //rotating
        LookAtTarget();
    }
   void MoveToTarget()
    {

        myDestination = offsetFromTarget / closeT ;
        destination = target.rotation * myDestination;
        //destination = charController.TargetRotaion * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }
    void LookAtTarget()
    {
        float eulerYAnlge = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, lookSmooth);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAnlge, 0);
    }
    
}
