using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidSteerController : MonoBehaviour
{

    public float LeftTrackSpeedCommand, RightTrackSpeedCommand, Throttle, Angular,maxvel=3,maxang=4;
    public Rigidbody[] RbR, RbL;
    public HingeJoint[] jointsR, jointsL;
    Transform myref;
    public float Torque = 4000, WheelRadius = 0.15f;
    public float xVel, RightXVel, LeftXVel;
    public float LeftTrackSpeed, RightTrackSpeed;
    Rigidbody rb;
    public bool velcontrol = false, UseMotors = false;
    public float AngCommand = 0, LinCommand = 0, p = 0.001f;
    public Vector3 AngularVel;
    private float Angfactor, LinFactor;

    // Use this for initialization
    void Start()
    {
        myref = transform;

        rb = GetComponent<Rigidbody>();
        if (UseMotors)
        {
            for (int i = 0; i < jointsL.Length; i++)
            {

                jointsR[i].useMotor = true;
                jointsL[i].useMotor = true;
            }
        }
        else
        {
            for (int i = 0; i < jointsL.Length; i++)
            {

                jointsR[i].useMotor = false;
                jointsL[i].useMotor = false;
            }
        }
    }

    void Update()
    {
        Throttle = Input.GetAxisRaw("Vertical");
        Angular = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Keypad9)) RightTrackSpeedCommand += 0.2f;
        if (Input.GetKeyDown(KeyCode.Keypad6)) RightTrackSpeedCommand -= 0.2f;
        if (Input.GetKeyDown(KeyCode.Keypad7)) LeftTrackSpeedCommand += 0.2f;
        if (Input.GetKeyDown(KeyCode.Keypad4)) LeftTrackSpeedCommand -= 0.2f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xVel = myref.InverseTransformDirection(rb.velocity).z;
        RightXVel = myref.InverseTransformDirection(RbR[0].velocity).z;
        LeftXVel = myref.InverseTransformDirection(RbL[0].velocity).z;

        AngularVel = rb.angularVelocity;
        if (velcontrol)
        {
            LinCommand = Throttle *maxvel;
            AngCommand = Angular*maxang;
            LinFactor += p * Time.fixedDeltaTime * 100 * (LinCommand - xVel);
            Angfactor += p * Time.fixedDeltaTime * 100 * (AngCommand - AngularVel.y);
            LeftTrackSpeedCommand = Mathf.Clamp(LinFactor + Angfactor,-5,5);
            RightTrackSpeedCommand =Mathf.Clamp(LinFactor - Angfactor,-5,5);
        
        }
        else
        {
            LeftTrackSpeedCommand = Throttle*maxvel + Angular * maxang;
            RightTrackSpeedCommand = Throttle*maxvel - Angular * maxang;
        }

        if (UseMotors)
        {
            for (int i = 0; i < jointsR.Length; i++)
            {
                var tempmotor = jointsR[i].motor;
                tempmotor.targetVelocity = RightTrackSpeedCommand * Mathf.Rad2Deg / WheelRadius;
                tempmotor.force = Torque;
                jointsR[i].motor = tempmotor;
            }
            for (int i = 0; i < jointsL.Length; i++)
            {
                var tempmotor = jointsR[i].motor;
                tempmotor.targetVelocity = LeftTrackSpeedCommand * Mathf.Rad2Deg / WheelRadius;
                tempmotor.force = Torque;
                jointsL[i].motor = tempmotor;
            }
        }
        else
        {
            for (int i = 0; i < RbL.Length; i++)
            {
                // var tempmotor=joints[i].motor;
                // tempmotor.targetVelocity=Speed;
                // joints[i].motor=tempmotor;
                RbL[i].AddRelativeTorque(new Vector3(Torque * Mathf.Sign(LeftTrackSpeedCommand), 0, 0), ForceMode.Force);
                RbL[i].maxAngularVelocity = LeftTrackSpeedCommand / WheelRadius;
            }
            for (int i = 0; i < RbR.Length; i++)
            {
                RbR[i].AddRelativeTorque(new Vector3(Torque * Mathf.Sign(RightTrackSpeedCommand), 0, 0), ForceMode.Force);
                RbR[i].maxAngularVelocity = RightTrackSpeedCommand / WheelRadius;
            }
        }
        }
    


public void Drive(float velR, float velL)
{
    RightTrackSpeedCommand = velR;
    LeftTrackSpeedCommand = velL;
}
}
