/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class TestCarMovement : MonoBehaviour
{

    internal enum DriveType
    {
        fw,
        rw,
        aW,
    }
    public GameObject CenterOfmass;
    public WheelCollider[] wheels;
    public AnimationCurve engineTorqu;
    public float MoveTorque;
    public float wheelsRPM;
    public float totalPower;
    public float engineRPM;
    public float[] gears;
    public int currentGear;
    public float smoothTime;
    public float KMP;
    public float _downForce = 50;
    private DriveType _DriveType;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfmass.transform.position;
    }
    void wheelRPM(){
        float sum = 0;
        int r = 0;
        foreach (var item in wheels)
        {
            sum += item.rpm;
            r++;
        }
        wheelsRPM = (r != 0) ? sum / r : 0;
}
    void moveCar()
    {
        int wheelsDrive;
        switch (_DriveType)
        {
            case DriveType.fw:
            case DriveType.rw:
                wheelsDrive = 2;
                break;
            case DriveType.aW:
                wheelsDrive = 4;
                break;
            default:
                wheelsDrive = 2;
                break;
        }
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].motorTorque = (totalPower / wheelsDrive);
        }
        KMP = rb.velocity.magnitude * 3.6f;
    }
    void EnginePower()
    {
        wheelRPM();
        totalPower = engineTorqu.Evaluate(engineRPM) * gears[currentGear];
        float v=0;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * gears[currentGear]),ref v, smoothTime);
    }
    void downForce()
    {
        rb.AddForce(-transform.up * _downForce * rb.velocity.magnitude);
    }

}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestCarMovement : MonoBehaviour
{
    internal enum driveType
    {
        frontWheelDrive,
        rearWheelDrive,
        allWheelDrive
    }
    [SerializeField] private driveType drive;
    public UnityEngine.UI.Text GearText;

    //other classes ->

    [HideInInspector] public bool test; //engine sound boolean

    [Header("Variables")]
    public float handBrakeFrictionMultiplier = 2f;
    public float maxRPM;
    public float minRPM;
    public float[] gears;
    public float[] gearChangeSpeed;
    public AnimationCurve enginePower;


    public int gearNum = 1;
    [HideInInspector] public bool playPauseSmoke = false, hasFinished;
    public float KPH;
    public float engineRPM;
    [HideInInspector] public bool reverse = false;
    [HideInInspector] public float nitrusValue;
    [HideInInspector] public bool nitrusFlag = false;


    public WheelCollider[] wheels = new WheelCollider[4];
    public GameObject centerOfMass;
    private Rigidbody rigidbody;

    //car Shop Values
    private float smoothTime = 0.09f;


    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    private float radius = 6, brakPower = 0, DownForceValue = 10f, wheelsRPM, driftFactor, lastValue;
    public float totalPower;
    private bool flag = false;




    private void Awake()
    {

        if (SceneManager.GetActiveScene().name == "awakeScene") return;
        getObjects();
        StartCoroutine(timedLoop());

    }

    private void FixedUpdate()
    {

        lastValue = engineRPM;

        addDownForce();

        calculateEnginePower();

        activateNitrus();

       

        var a = new Quaternion(0, transform.rotation.y, 0, 1);
        transform.rotation = Quaternion.Slerp(transform.rotation, a, 0.01f);
        transform.position = new Vector3(-14.85f, transform.position.y, transform.position.z);

    }
    public float limt;
    private void AutomaticSpeedLimiter()
    {
        Vector3 velocity = Vector3.zero;
        if (KPH > gearChangeSpeed[gearNum])
        {
            rigidbody.velocity = Vector3.SmoothDamp(rigidbody.velocity,
                rigidbody.velocity.normalized * (gearChangeSpeed[gearNum] / 3.6f), ref velocity, limt * Time.deltaTime);
        }
    }
    private void calculateEnginePower()
    {
        //get wheels RPM
        wheelRPM();


        rigidbody.drag = 0.005f;
        //this one as you told me
        totalPower = 3.6f * enginePower.Evaluate(engineRPM) * 1;


        //this is the else
        float velocity = 0;
        engineRPM = Mathf.SmoothDamp(engineRPM, 1000 + (Mathf.Abs(wheelsRPM) * 3.6f * (gears[gearNum])), ref velocity, smoothTime);
        //this max it at max
        if (engineRPM >= maxRPM + 1000) engineRPM = maxRPM + 1000; // clamp at max

        //move the car
        moveVehicle();
        AutomaticSpeedLimiter();
        shifter();

        GearText.text = gearNum.ToString();
    }

    private void wheelRPM()
    {
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;

    }


    private bool checkGears()
    {
        if (KPH >= gearChangeSpeed[gearNum]) return true;
        else return false;
    }

    private void shifter()
    {

        if (!isGrounded()) return;
        //automatic
        if (engineRPM > maxRPM && gearNum < gears.Length - 1 && !reverse && checkGears())
        {
            gearNum++;
            return;
        }
        if (engineRPM < minRPM && gearNum > 0)
        {
            gearNum--;
        }

    }
    private bool isGrounded()
    {
        if (wheels[0].isGrounded && wheels[1].isGrounded && wheels[2].isGrounded && wheels[3].isGrounded)
            return true;
        else
            return false;
    }
    private void moveVehicle()
    {


        if (drive == driveType.allWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = totalPower / 4;
                wheels[i].brakeTorque = brakPower;
            }
        }
        else if (drive == driveType.rearWheelDrive)
        {
            wheels[2].motorTorque = totalPower / 2;
            wheels[3].motorTorque = totalPower / 2;

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = brakPower;
            }
        }
        else
        {
            wheels[0].motorTorque = totalPower / 2;
            wheels[1].motorTorque = totalPower / 2;

            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].brakeTorque = brakPower;
            }
        }

        KPH = rigidbody.velocity.magnitude * 3.6f;


    }




    private void getObjects()
    {

        rigidbody = GetComponent<Rigidbody>();

        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void addDownForce()
    {

        rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);

    }



    private IEnumerator timedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(.7f);
            radius = 6 + KPH / 20;

        }
    }

    public void activateNitrus()
    {
        if (nitrusValue <= 10)
        {
            nitrusValue += Time.deltaTime / 2;
        }
        else
        {
            nitrusValue -= (nitrusValue <= 0) ? 0 : Time.deltaTime;
        }


        {
            if (nitrusValue > 0)
            {
                rigidbody.AddForce(transform.forward * 5000);
            }
        }

    }

}