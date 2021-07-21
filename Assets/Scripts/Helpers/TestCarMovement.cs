
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class TestCarMovement : MonoBehaviour
{
    internal enum DriveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [Header("Test Move")]
    public float[] GearRatio = { 4.171f, 2.340f, 1.521f, 1.143f, 0.867f, 0.691f };
    public float[] EngineTorquePerGear;
    public float FinalDriveRatio = 3.460f;

    public float GetWheelTorque(int gear)
    {
        float Tw = (GearRatio[gear] * FinalDriveRatio * EngineTorquePerGear[gear]) / 2;
        return Tw;
    }


    [Header("Wheels")]
    [SerializeField] private DriveType CarDriveType;
    [SerializeField] private WheelCollider[] FrontWheels;
    [SerializeField] private WheelCollider[] RearWheels;
    private Transform[] FrontWheelMeshes;
    private Transform CarBodyMesh;
    [SerializeField] private float MotorTorque;


    [Header("Gears")]
    public int currentGear;
    public int CurrentGear
    {
        get { return this.currentGear; }
    }

    [SerializeField] private float[] carMaxSpeedPerGear;

    public float[] CarMaxSpeedPerGear
    {
        get { return this.carMaxSpeedPerGear; }
    }


    [SerializeField] public float CapSpeedSmoothing;
    [SerializeField] private bool AutoGear;
    //other classes ->

    [Header("Boost")]
    [SerializeField] private float MaxBoostTime;
    [Range(0f, 10f)]
    [SerializeField] private float BoostRefillSpeed;
    [SerializeField] private float BoostForce;
    private bool IsBoosting;
    private float BoostTime = 10f;
    private bool BoostAllowed;



    [Header("Gear Stunt")]
    public float GearUpStuntForce;
    public Transform GearStuntForceAt;
    public ForceMode GearStuntForceMode;

    [SerializeField] private GameObject CenterOfMass;
    [SerializeField] private float DownForce;

    private Rigidbody CarRB;

    //car Shop Values

    [Header("Car Rotation")]
    [SerializeField] private bool LimitRotations;
    [SerializeField] private float MaxX, MaxZ;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float InitialXPos;

    [Header("Car Stunt")]
    public Vector3 StuntVector;
    public float StuntDuration;
    private bool StuntCompleted;
    public Ease StuntEase;
    private bool WaitingForGearing;
    private Vector3 CarRotationBeforeStunt;

    public bool startAcc;
    public float StartAccForce;

    public float m_KPH;
    public float KPH
    {
        get
        {
            return m_KPH;
        }
    }

    private CarStuntAnimation stuntAnimation;
    private void Awake()
    {

        InitCar();

    }
    public void Start()
    {
        if (startAcc)
        {
            CarRB.AddForce(transform.forward * StartAccForce * CarRB.mass, ForceMode.Impulse);
        }
    }

    private void InitCar()
    {
        CarRB = GetComponent<Rigidbody>();
        stuntAnimation = GetComponentInChildren<CarStuntAnimation>();

        CarRB.centerOfMass = CenterOfMass.transform.localPosition;

        InitialXPos = transform.position.x;

        BoostAllowed = true;
        IsBoosting = false;
    }

    private void Update()
    {


        CarRB.drag = Mathf.Clamp((KPH / CarMaxSpeedPerGear[CarMaxSpeedPerGear.Length - 1]) * 0.075f, 0.001f, 0.075f);

        if (!IsBoosting)
        {
            BoostTime += Time.deltaTime * BoostRefillSpeed;
            if (BoostTime > MaxBoostTime)
            {
                BoostTime = MaxBoostTime;
            }
        }
        LimitRotation();
        FreezeTransformPosAndRot(InitialXPos, 0);
         StuntAnimation();
        if (CanGear() && AutoGear && !WaitingForGearing)
        {
            ChangeGear(true);
        }
    }

    private void FixedUpdate()
    {
        moveVehicle();

        ActivateBoost();
        AutomaticSpeedLimiter();
        AddDownForce();
    }

    private void AutomaticSpeedLimiter()
    {
        Vector3 velocity = Vector3.zero;
        if (KPH > CarMaxSpeedPerGear[currentGear])
        {

            CarRB.velocity = Vector3.SmoothDamp(CarRB.velocity, CarRB.velocity.normalized * (CarMaxSpeedPerGear[currentGear] / 3.6f), ref velocity, CapSpeedSmoothing);
        }
    }

    public bool IsGrounded()
    {
        for (int i = 0; i < FrontWheels.Length; i++)
        {
            if (!FrontWheels[i].isGrounded)
            {
                return false;
            }
        }
        for (int i = 0; i < RearWheels.Length; i++)
        {
            if (!RearWheels[i].isGrounded)
            {
                return false;
            }
        }
        return true;
    }
    public bool CanGear()
    {
        if (KPH > CarMaxSpeedPerGear[currentGear] && currentGear < CarMaxSpeedPerGear.Length - 1)
        {
            return true;
        }
        return false;
    }
    public void StuntAnimation()
    {
        /*if (!IsGrounded())
        {
            return;
        }
        */
        if (!StuntCompleted)
        {
            CarRotationBeforeStunt = transform.eulerAngles;
        }

        if (IsBoosting)
        {
            StuntCompleted = true;
            transform.DORotate(StuntVector, StuntDuration).SetEase(StuntEase);
        }

        if (!IsBoosting && StuntCompleted)
        {
            transform.DORotate(CarRotationBeforeStunt, StuntDuration).SetEase(StuntEase).OnComplete(() =>
            {
                StuntCompleted = false;
            });
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
    private void FreezeTransformPosAndRot(float xPos, float yRot)
    {
        transform.rotation = new Quaternion(transform.rotation.x, yRot, transform.rotation.z, transform.rotation.w);

        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }
    IEnumerator Gear(float time)
    {
        WaitingForGearing = true;
        yield return new WaitForSeconds(time);
        ChangeGear(true);
        WaitingForGearing = false;
    }
    public void ChangeGear(bool up)
    {
        if (up)
        {
            currentGear++;
        }
        else
        {
            currentGear--;
        }

        CarRB.AddForceAtPosition((transform.up * GearUpStuntForce * CarRB.mass), GearStuntForceAt.position, GearStuntForceMode);

        currentGear = Mathf.Clamp(currentGear, 0, CarMaxSpeedPerGear.Length - 1);
    }
    public float getPower()
    {

        var val = (KPH + 1) / 2;

        val = 1 - (KPH / CarMaxSpeedPerGear[currentGear]);
        StuntVector.z = val;
        StuntVector.y = MotorTorque * val;

        return MotorTorque * val;
    }

    private void moveVehicle()
    {


        if (CarDriveType == DriveType.allWheelDrive)
        {
            for (int i = 0; i < FrontWheels.Length; i++)
            {
                FrontWheels[i].motorTorque = GetWheelTorque(currentGear);
            }
            for (int i = 0; i < RearWheels.Length; i++)
            {
                RearWheels[i].motorTorque = GetWheelTorque(currentGear);
            }
        }
        else if (CarDriveType == DriveType.rearWheelDrive)
        {
            for (int i = 0; i < RearWheels.Length; i++)
            {


                RearWheels[i].motorTorque = GetWheelTorque(currentGear);

            }
        }
        else
        {
            for (int i = 0; i < FrontWheels.Length; i++)
            {

                FrontWheels[i].motorTorque = GetWheelTorque(currentGear);

            }
        }

        m_KPH = CarRB.velocity.magnitude * 3.6f;


    }




    private void AddDownForce()
    {
        CarRB.AddForce(-transform.up * DownForce * CarRB.velocity.magnitude);
    }

    public void ActivateBoost()
    {
        // Boost
        if (IsBoosting && BoostAllowed && BoostTime > 0.0f)
        {

            CarRB.AddForce(transform.forward * BoostForce);
            //CarRB.AddForceAtPosition((transform.forward * StuntVector.y * CarRB.mass), BoostStuntForceAt.position, BoostStuntForceMode);

            BoostTime -= Time.fixedDeltaTime;
            if (BoostTime < 0f)
            {
                BoostTime = 0f;
                IsBoosting = false;
            }

        }
    }
    public float GetAvailableBoost()
    {
        return BoostTime / MaxBoostTime;
    }
    public void ActiveBoost()
    {
        IsBoosting = true;
    }
}