using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    internal enum DriveType
    {
        None,
        FrontWheels,
        RearWheels,
        AllWheels
    }
    [Header("Engine")]
    private float EngineRPM;
    private float MaxRPM;
    private AnimationCurve EngineTorque = new AnimationCurve(new Keyframe(4.3f, 101.0f), new Keyframe(7903.0f, 472.0f), new Keyframe(9529.0f, 259.0f));
    [Header("Gears")]
    private float[] GearBox;
    private float[] GearBoxSpeedsLimits;
    public int CurrentGear;

    [Header("Rotation")]
    public bool LimitRotations;
    public float MaxX, MaxZ;
    public float RotationSpeed;
    [Header("Push Force")]
    public ForceMode ForceMode;
    public float ForcePower;
    private bool CarInGearForce;
    [Header("Forces")]
    private float BrakeForce = 0;
    public float Downforce = 1.0f;
    public bool isGrounded;
    [Header("Boost")]
    private bool IsBoosting;
    private bool AllowBoosting;

    [SerializeField] float MaxBoostTime;
    [SerializeField] float BoostTime;
    [SerializeField] float BoostRegen;
    [SerializeField] float BoostForce;
    
    float MaxGearForceTime;
    float GearForceTime;
    float GearForceRegen;
    [Header("Behavior")]
    public Transform centerOfMass;
    public float kph;
    public float InitialXPos;
    [Header("Wheels")]
    [SerializeField]
    private DriveType WheelsDriveType;
    private float Acceleration;
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

    public float KPH { get { return kph; } }
    public float Gear { get { return CurrentGear; } }
    public bool CanGear()
    {
        return CurrentGear < GearBox.Length - 1;
    }
    public bool CanBoost()
    {
        return CurrentGear == GearBox.Length - 1;
    }
    public float GetAvailableBoost()
    {
        return BoostTime / MaxBoostTime;
    }
    public void ActiveBoost()
    {
        AllowBoosting = true;
        IsBoosting = true;
    }

    // Private variables set at the start
    private Rigidbody rb;
    private List<WheelCollider> wheels;
    private float smoothTime = 0.09f;

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
        BoostTime = MaxBoostTime;
        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;
        this.ForceMode = GameManagment.GameManager.Instance.GameSettings.ForceMode;
        this.ForcePower = GameManagment.GameManager.Instance.GameSettings.ForcePower;
        this.MaxGearForceTime = GameManagment.GameManager.Instance.GameSettings.MaxGearForceTime;
        this.GearForceTime = GameManagment.GameManager.Instance.GameSettings.GearForceTime;
       this.GearForceRegen = GameManagment.GameManager.Instance.GameSettings.GearForceRegen;
        this.Downforce = GameManagment.GameManager.Instance.GameSettings.DownForce;

        this.MaxX = GameManagment.GameManager.Instance.GameSettings.MaxX;

        this.MaxRPM = GameManagment.GameManager.Instance.GameSettings.MaxRPM;

        this.GearBox = GameManagment.GameManager.Instance.GameSettings.GearBox;
        this.GearBoxSpeedsLimits = GameManagment.GameManager.Instance.GameSettings.GearBoxSpeedLimits;

        this.LimitRotations = GameManagment.GameManager.Instance.GameSettings.LimitRotation;

        this.MaxBoostTime = GameManagment.GameManager.Instance.GameSettings.MaxBoostTime;
        this.BoostTime = GameManagment.GameManager.Instance.GameSettings.BoostTime;
        this.BoostRegen = GameManagment.GameManager.Instance.GameSettings.BoostRegen;
        this.BoostForce = GameManagment.GameManager.Instance.GameSettings.BoostForce;

        this.smoothTime = GameManagment.GameManager.Instance.GameSettings.smoothTime;


        GetWheels();

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
    }
    public void ShiftDown()
    {
        if (CurrentGear > 0)
        {
            CurrentGear--;
        }
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

        var a = new Quaternion(0, transform.rotation.y, 0, 1);
        transform.rotation = Quaternion.Slerp(transform.rotation, a, 0.01f);

    }
    void Update()
    {
        LimitRotation();

        isGrounded = IsGrounded;

        FreezeTransformPosAndRot(InitialXPos, 0);

        if (!IsBoosting)
        {
            BoostTime += Time.deltaTime * BoostRegen;
            if (BoostTime > MaxBoostTime)
            {
                BoostTime = MaxBoostTime;
            }
        }
        if (!CarInGearForce)
        {

            GearForceTime += Time.deltaTime * GearForceRegen;
            if (GearForceTime > MaxGearForceTime)
            {
                GearForceTime = MaxGearForceTime;
            }
        }
    }
    public void Drive(float acceleration)
    {

        UpdateAcceleration(acceleration);
        AddForceIfBetween(0.5f, 1.0f);

    }
    
    public void SetInitialXPos(float val)
    {
        InitialXPos = val;
    }
    public void UpdateAcceleration(float val)
    {
        Acceleration = Math.Abs(val);
    }

    void FixedUpdate()
    {
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


            for (int i = 0; i < wheels.Count; i++)
            {
                wheels[i].motorTorque = GetWheelsTorque() / 4;
                wheels[i].brakeTorque = BrakeForce;
            }



            if (AllowBoosting && IsBoosting && BoostTime > 0.0f)
            {
                rb.AddForce(transform.forward * BoostForce);

                BoostTime -= Time.fixedDeltaTime;
                if (BoostTime <= 0f)
                {
                    AllowBoosting = false;
                    BoostTime = 0f;
                    IsBoosting = false;
                }
            }

            if (CarInGearForce && GearForceTime > 0.0f)
            {
                GearForceTime -= Time.fixedDeltaTime;
                
                if (GearForceTime <= 0f)
                {
                    GearForceTime = 0f;
                    CarInGearForce = false;
                }
            }

            kph = rb.velocity.magnitude * 3.6f;

            if (!CarInGearForce && !IsBoosting)
            {
                AutomaticSpeedLimiter();
            }
         
            AddDownForce();

        }
    }

    private float GetWheelsTorque()
    {
        float wheelsRPM = GetWheelsRPM();

        return GetTotalPower(wheelsRPM);
    }

    private float GetTotalPower(float wheelsRPM)
    {
        float totalPower = 0;
        totalPower = 3.6f * EngineTorque .Evaluate(EngineRPM) * 1;
        float velocity = 0;
        EngineRPM = Mathf.SmoothDamp(EngineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (GearBox[CurrentGear])), ref velocity, 0.01f * Time.deltaTime);
        //this max it at max
        if (EngineRPM >= MaxRPM + 1000) EngineRPM = MaxRPM + 1000; // clamp at max

        return totalPower;
    }
    private float GetWheelsRPM()
    {
        float rmp;
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        rmp = (R != 0) ? sum / R : 0;
        return rmp;
    }

    private void AutomaticSpeedLimiter()
    {
        Vector3 velocity = Vector3.zero;
        if (kph > GearBoxSpeedsLimits[CurrentGear])
        {
            rb.velocity = Vector3.SmoothDamp(rb.velocity,
                rb.velocity.normalized * (GearBoxSpeedsLimits[CurrentGear] / 3.6f), ref velocity, smoothTime * Time.deltaTime);
        }
    }
    private void AddDownForce()
    {

        rb.AddForce(-transform.up * Downforce * rb.velocity.magnitude);

    }
    public void ToogleHandbrake(bool h)
    {
        rb.drag = 3;
        handbrake = h;
    }
    #region Physics
    private void AddForceIfBetween(float a, float b)
    {
        if (Acceleration.IsBetween(a, b))
        {
            CarInGearForce = true;

            rb.AddForce(transform.position + Vector3.forward * ForcePower * GetWheelsTorque(), ForceMode);
        }
    }
    public void SetCarToPosition(Vector3 pos)
    {
        rb.isKinematic = true;
        transform.position = pos;
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
        rb.angularDrag = GameManagment.GameManager.Instance.GameSettings.AngularDrag;

    }

    private float GetBrakeForce()
    {
        var newForce = BrakeForce * Acceleration;
        newForce -= BrakeForce;
        return Mathf.Abs(newForce);
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

