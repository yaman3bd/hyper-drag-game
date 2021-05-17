using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public WheelCollider[] wheels;
    public float x;
    private void Update()
    {
        var pos = new Vector3(x, transform.position.y, transform.position.z);
        transform.position = pos;
    }
    private void FixedUpdate()
    {


        foreach (var item in wheels)
        {
            item.motorTorque = getrpm();
        }

    }
    public int gear = 1; // Current gear, initially the 1st
    public int gearCount = 5; // Total no. of gears

    public float speed = 0; // Speed (km/h), initially 0
     float[] maxSpeedsPerGear = new float[]
    {
    40,  // First gear max. speed at max. RPM
    70,  // Second gear max. speed at max. RPM
    100, // and so on
    130,
    170
    };

    public float rpm = 0; // Current engine RPM
    public int maxRPM = 8500; // Max. RPM

    public float getrpm()
    {
        if (rpm < maxRPM)
        {
            rpm += 65 / gear; // The higher the gear, the slower the RPM increases
        }

        speed = (rpm / maxRPM) * maxSpeedsPerGear[gear - 1];



        rpm = (maxRPM * speed) / maxSpeedsPerGear[gear - 1];
        if (rpm < 1500) rpm = 1500; // Just a silly "lower limit" for RPM
        return rpm;

    }


}
