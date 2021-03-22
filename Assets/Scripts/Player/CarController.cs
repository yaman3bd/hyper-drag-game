using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{

    [Header("Rotation")]
    public bool LimitRotations;
    public float MaxX, MaxZ;
    [UnityEngine.Serialization.FormerlySerializedAs("MaxY")]
    public float RotationSpeed;
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
    private List<WheelCollider> wheels;

    private float CurrentDiffGearing;
    private float Acceleration;
    private float InitialXPos;
    private float[] GearBox;
    private int CurrentGear;

    private void Awake()
    {

        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        if (rb != null && centerOfMass != null)
        {
            rb.centerOfMass = centerOfMass.localPosition;
        }
        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;
        this.ForceMode = GameManagment.GameManager.Instance.GameSettings.ForceMode;
        this.ForcePower = GameManagment.GameManager.Instance.GameSettings.ForcePower;
        this.GearBox = GameManagment.GameManager.Instance.GameSettings.GearBox;
        this.LimitRotations = GameManagment.GameManager.Instance.GameSettings.LimitRotation;
        GetWheels();
        UpdateBodyDrag(0);


    }

    private void OnRaceStarted()
    {
        rb.isKinematic = false;
    }

    public void ShiftUp()
    {
        if (CurrentGear < GearBox.Length - 1)
        {
            CurrentGear++;
        }

        UpdateBodyDrag(CurrentGear);

    }

    private void UpdateBodyDrag(int CurrentGear)
    {
        rb.drag = GearBox[CurrentGear];
    }
    public void ShiftDown()
    {
        if (CurrentGear > 0)
        {
            CurrentGear--;
        }
        UpdateBodyDrag(CurrentGear);

    }
    private void LimitRotation()
    {
        if (!LimitRotations)
        {
            return;
        }
        var rot = transform.rotation;
        rot.x = Mathf.Clamp(rot.x, -MaxX, MaxX);
        rot.z = Mathf.Clamp(rot.z, -MaxZ, MaxZ);

        if (Math.Abs(transform.rotation.x) > Math.Abs(rot.x) || Math.Abs(transform.rotation.z) > Math.Abs(rot.z))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, RotationSpeed * Time.deltaTime);
        }

    }
    void Update()
    {
        LimitRotation();

        isGrounded = IsGrounded;

        FreezeTransformPosAndRot(InitialXPos, 0);

    }
    public void Drive(float acceleration, float currentDiffGearing)
    {

        UpdateAcceleration(acceleration);
        UpdateCurrentDiffGearing(currentDiffGearing);
        AddForceIfBetween(0.5f, 1.0f);

    }
    public void GetInitialXPos(float val)
    {
        InitialXPos = val;
    }
    public void UpdateAcceleration(float val)
    {
        Acceleration = val;
    }
    public void UpdateCurrentDiffGearing(float val)
    {
        CurrentDiffGearing = val;
    }

    void FixedUpdate()
    {

        // Mesure current speed
        //speed = transform.InverseTransformDirection(rb.velocity).z * 3.6f;
        speed = rb.velocity.magnitude * 2.7f;
        if (handbrake)
        {
            foreach (WheelCollider wheel in wheels)
            {
                // Don't zero out this value or the wheel completly lock up
                wheel.motorTorque = 0.0001f;
                wheel.brakeTorque = BrakeForce;
            }
        }
        else
        {
            foreach (WheelCollider wheel in wheels)
            {
                wheel.motorTorque = GetWheelMotorTorque();
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


            // Down-force
            rb.AddForce(-transform.up * speed * Downforce);
        }
    }

    public void ToogleHandbrake(bool h)
    {
        rb.drag = 2;
        handbrake = h;
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
    public void SetCarToPosition(Vector3 pos)
    {
        rb.isKinematic = true;
        transform.position = pos;
    }
    public void FreezePositionY()
    {
   //  rb.constraints = RigidbodyConstraints.FreezeRotation;
    }
    public void FreePositionY()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    #endregion

    #region Wheels

    private void GetWheels()
    {
        wheels = new List<WheelCollider>();

       // if (IsFrontWheelsDriving)
        {
            Transform Wheels = transform.Find("FrontWheels");

            wheels.Add(Wheels.GetChild(0).GetComponent<WheelCollider>());
            wheels.Add(Wheels.GetChild(1).GetComponent<WheelCollider>());
        }

       // if (IsRearWheelsDriving)
        {
            Transform Wheels = transform.Find("RearWheels");

            wheels.Add(Wheels.GetChild(0).GetComponent<WheelCollider>());
            wheels.Add(Wheels.GetChild(1).GetComponent<WheelCollider>());
        }

        foreach (var wheel in wheels)
        {

            wheel.motorTorque = 0.0001f;
            wheel.wheelDampingRate = GameManagment.GameManager.Instance.GameSettings.WheelDampingRate;
            wheel.suspensionDistance = GameManagment.GameManager.Instance.GameSettings.SuspensionDistance;

            var JointSpring = new JointSpring();
            JointSpring.spring = GameManagment.GameManager.Instance.GameSettings.Spring;
            JointSpring.damper = GameManagment.GameManager.Instance.GameSettings.Damper;
            JointSpring.targetPosition = GameManagment.GameManager.Instance.GameSettings.TargetPosition;
            wheel.suspensionSpring = JointSpring;

        }
        rb.mass = GameManagment.GameManager.Instance.GameSettings.BodyMass;

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

    #endregion

    #region Transform
    private void FreezeTransformPosAndRot(float xPos, float yRot)
    {
        transform.rotation = new Quaternion(transform.rotation.x, yRot, transform.rotation.z, transform.rotation.w);

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
    #endregion

}

