using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncWheelsValues : MonoBehaviour
{
    WheelCollider[] wheels;
    public void SyncValues()
    {
        if (wheels == null)
        {
            wheels = GetComponentsInChildren<WheelCollider>();

        }
        foreach (var wheel in wheels)
        {
            wheel.wheelDampingRate = GameManagment.GameManager.Instance.GameSettings.WheelDampingRate;
            wheel.suspensionDistance = GameManagment.GameManager.Instance.GameSettings.SuspensionDistance;

            var JointSpring = new JointSpring();

            JointSpring.spring = GameManagment.GameManager.Instance.GameSettings.Spring;
            JointSpring.damper = GameManagment.GameManager.Instance.GameSettings.Damper;
            JointSpring.targetPosition = GameManagment.GameManager.Instance.GameSettings.TargetPosition;
            wheel.suspensionSpring = JointSpring;
        }
        GetComponent<Rigidbody>().mass= GameManagment.GameManager.Instance.GameSettings.BodyMass;
    }
    // Start is called before the first frame update
    void Start()
    {
        wheels = GetComponentsInChildren<WheelCollider>();
        SyncValues();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
