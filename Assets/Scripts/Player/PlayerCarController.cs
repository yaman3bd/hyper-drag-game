using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(Rigidbody), typeof(ShiftsScript))]
public class PlayerCarController : MonoBehaviour
{

    [Header("Push Force")]
    public ForceMode ForceMode;
    public float ForcePower;

    [Header("Wheels")]
    public bool IsFrontWheelsDriving;
    public bool IsRearWheelsDriving;
    /*
     *  Motor torque represent the torque sent to the wheels by the motor with x: speed in km/h and y: torque
     *  The curve should start at x=0 and y>0 and should end with x>topspeed and y<0
     *  The higher the torque the faster it accelerate
     *  the longer the curve the faster it gets
     */
 
    public AnimationCurve MotorTorque = new AnimationCurve(new Keyframe(0, 200), new Keyframe(50, 300), new Keyframe(200, 0));
    public float[] diffGearingss;

    [Header("Forces")]
    public float BrakeForce;
    [Range(0.5f, 10f)]
    public float Downforce = 1.0f;
    public bool isGrounded;
    [Header("Behavior")]
    public Transform centerOfMass;
    public float speed = 0.0f;

    private bool handbrake;

    int lastGroundCheck = 0;
    public bool IsGrounded
    {
        get
        {
            if (lastGroundCheck == Time.frameCount)
                return isGrounded;

            lastGroundCheck = Time.frameCount;
            isGrounded = true;
            foreach (WheelCollider wheel in wheels)
            {
                if (!wheel.gameObject.activeSelf || !wheel.isGrounded)
                    isGrounded = false;
            }
            return isGrounded;
        }
    }

    public bool Handbrake { get { return handbrake; } set { handbrake = value; } }

    public float Speed { get { return speed; } }

    // Private variables set at the start
    private Rigidbody rb;
    private ShiftsScript shiftsScript;
    private List<WheelCollider> wheels;

    private float CurrentDiffGearing;
    private float Acceleration;
    private float InitialXPos;

    private void Awake()
    {
        InitialXPos = transform.position.x;

        shiftsScript = GetComponent<ShiftsScript>();

        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

        if (rb != null && centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }

        GetWheels();

        shiftsScript.UpdateEpicShiftValues();

    }

    void Update()
    {

        isGrounded = IsGrounded;


        FreezeTransformPosAndRot(InitialXPos, 0);


        shiftsScript.HandleShiftSlider();


        if (Input.GetMouseButtonDown(0))
        {
            shiftsScript.ResetShiftSliderTime();

            Acceleration = shiftsScript.GetShiftTime();

            shiftsScript.HandleShiftState();

            shiftsScript.UpdateEpicShiftValues();

            CurrentDiffGearing = UpdateDiffGearing();

            AddForceIfBetween(0.5f, 1.0f);

        }
    }

    void FixedUpdate()
    {

        // Mesure current speed
        speed = transform.InverseTransformDirection(rb.velocity).z * 3.6f;


        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = 200f;
            wheel.brakeTorque = 0;
        }

        if (Acceleration.IsBetween(0.0f, 0.3f))
        {

            foreach (WheelCollider wheel in wheels)
            {
                // Don't zero out this value or the wheel completely lock up
                wheel.motorTorque = 0.01f;
                wheel.brakeTorque = GetBrakeForce();
            }
        }
        else if (Mathf.Abs(speed) < 4)
        {

            foreach (WheelCollider wheel in wheels)
            {
                wheel.motorTorque = GetWheelMotorTorque();
            }
        }


        // Down-force
        rb.AddForce(-transform.up * speed * Downforce);
    }

    #region Physics
    private void AddForceIfBetween(float a, float b)
    {
        if (Acceleration.IsBetween(a, b))
        {
            rb.AddForce(transform.position + Vector3.forward * ForcePower * GetWheelMotorTorque(), ForceMode);
        }
    }

    public void Stop()
    {
        rb.velocity = rb.velocity;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ 
            | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = Mathf.Epsilon;
            wheel.brakeTorque = BrakeForce * BrakeForce * BrakeForce;
        }
    }

    #endregion

    #region Wheels

    private void GetWheels()
    {
        wheels = new List<WheelCollider>();

        if (IsFrontWheelsDriving)
        {
            Transform Wheels = transform.Find("FrontWheels");

            wheels.Add(Wheels.GetChild(0).GetComponent<WheelCollider>());
            wheels.Add(Wheels.GetChild(1).GetComponent<WheelCollider>());
        }

        if (IsRearWheelsDriving)
        {
            Transform Wheels = transform.Find("RearWheels");

            wheels.Add(Wheels.GetChild(0).GetComponent<WheelCollider>());
            wheels.Add(Wheels.GetChild(1).GetComponent<WheelCollider>());
        }

        foreach (var wheel in wheels)
        {
            wheel.motorTorque = 0.0001f;
        }

    }

    private float GetBrakeForce()
    {
        var newForce = BrakeForce * Acceleration;
        newForce -= BrakeForce;
        return Mathf.Abs(newForce);
    }

    private float GetWheelMotorTorque()
    {
        return (MotorTorque.Evaluate(speed) * Acceleration * GetDiffGearings());
    }

    #endregion

    #region Gearings

    private float GetDiffGearings()
    {

        int driveWheels = 2;

        if (IsFrontWheelsDriving && IsRearWheelsDriving)
        {
            driveWheels = 4;
        }

        return CurrentDiffGearing / driveWheels;

    }

    private float UpdateDiffGearing()
    {
        return shiftsScript.GetShiftTime().Remap(shiftsScript.GetMinShiftTime(), shiftsScript.GetMaxShiftTime(), diffGearingss[0], diffGearingss[1]);
    }

    #endregion

    #region Transform
    private void FreezeTransformPosAndRot(float xPos, float yRot)
    {
        transform.rotation = new Quaternion(transform.rotation.x, yRot, transform.rotation.z, transform.rotation.w);

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
    #endregion

    public void Reset()
    {
        transform.position = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
    }
}

