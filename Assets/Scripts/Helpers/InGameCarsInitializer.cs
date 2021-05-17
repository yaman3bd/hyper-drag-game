using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameCarsInitializer : MonoBehaviour
{
    public GameObject[] cars;
    public bool CarsInitialized = false;

    public void InitializeCars()
    {

        if (!CarsInitialized)
        {
            int i = 0;
            foreach (var item in cars)
            {
                item.name = "c_" + i;

                var Rigid = item.GetComponent<Rigidbody>();

                Rigid.mass = 500;
                Rigid.angularDrag = 5;

                var carController = item.AddComponent<CarController>();

                carController.MaxX = 0.25f;
                carController.MaxZ = 0.25f;
                carController.RotationSpeed = 50.0f;

                
                //carController.BrakeForce = 30000;
                carController.Downforce = 10;

                var CenterOfMass = new GameObject("CenterOfMass");

                CenterOfMass.transform.SetParent(item.transform);

                carController.centerOfMass = CenterOfMass.transform;


                var shiftsScript = item.AddComponent<ShiftsScript>();
                var effectsScript = item.AddComponent<EffectsScript>();
                shiftsScript.EffectsScript = effectsScript;

                shiftsScript.EpicShiftThreshold = 0.1f;
                shiftsScript.TooCloseShiftThreshold = 0.15f;


                var temp = new GameObject("TempEffects");
                temp.transform.SetParent(item.transform);

                effectsScript.EpicEffect = temp;
                effectsScript.EpicComboEffect = temp;

                var Body = item.transform.Find("Body").GetComponent<Renderer>();
                var box = item.transform.Find("Body").GetComponent<BoxCollider>();

                if (box != null)
                {
                    DestroyImmediate(box);

                }
                try
                {
                    var Spoiler = item.transform.Find("Spoiler").GetComponent<BoxCollider>();
                    DestroyImmediate(Spoiler);
                }
                catch (Exception e)
                {

                }

                Vector3 center = Body.bounds.center;
                CenterOfMass.transform.localPosition = new Vector3(0, center.y / 2, 0);
                var FrontWheels = new GameObject("FrontWheels");

                FrontWheels.transform.SetParent(item.transform);

                FrontWheels.transform.position = Vector3.zero;
                FrontWheels.transform.rotation = Quaternion.identity;

                FrontWheels.transform.localPosition = Vector3.zero;
                FrontWheels.transform.localRotation = Quaternion.identity;

                var RearWheels = new GameObject("RearWheels");

                RearWheels.transform.SetParent(item.transform);

                RearWheels.transform.position = Vector3.zero;
                RearWheels.transform.rotation = Quaternion.identity;

                RearWheels.transform.localPosition = Vector3.zero;
                RearWheels.transform.localRotation = Quaternion.identity;

                var wheels = item.transform.Find("Wheels");


                var Meshes = wheels.transform.Find("Meshes");

                var FrontLeftWheelMesh = Meshes.transform.Find("FrontLeftWheel");
                var FrontRightWheelMesh = Meshes.transform.Find("FrontRightWheel");
                var RearLeftWheelMesh = Meshes.transform.Find("RearLeftWheel");
                var RearRightWheelMesh = Meshes.transform.Find("RearRightWheel");


                var Colliders = wheels.transform.Find("Colliders");

                var FrontLeftWheelCollider = Colliders.transform.Find("FrontLeftWheel");
                FrontLeftWheelMesh.SetParent(FrontLeftWheelCollider);

                var FrontRightWheelCollider = Colliders.transform.Find("FrontRightWheel");
                FrontRightWheelMesh.SetParent(FrontRightWheelCollider);

                FrontLeftWheelCollider.SetParent(FrontWheels.transform);
                FrontRightWheelCollider.SetParent(FrontWheels.transform);


                var RearLeftWheelCollider = Colliders.transform.Find("RearLeftWheel");
                RearLeftWheelMesh.SetParent(RearLeftWheelCollider);

                var RearRightWheelCollider = Colliders.transform.Find("RearRightWheel");
                RearRightWheelMesh.SetParent(RearRightWheelCollider);

                RearLeftWheelCollider.SetParent(RearWheels.transform);
                RearRightWheelCollider.SetParent(RearWheels.transform);
                DestroyImmediate(wheels.gameObject);
                DestroyImmediate(item.transform.Find("Body").gameObject);

                i++;
            }
        }
        CarsInitialized = true;
    }

    public int carsId = 0;
    public void InitializeCarsBody()
    {
        foreach (var item in cars)
        {
            item.GetComponent<Rigidbody>().mass = 2500;
            var cols = item.GetComponentsInChildren<WheelCollider>();
            foreach (var items in cols)
            {
                var JointSpring = new JointSpring();
                JointSpring.spring = 55000;
                JointSpring.damper = 4500;
                JointSpring.targetPosition = 0.5f;

                items.suspensionSpring = JointSpring;
            }
        /*    var wheels = item.transform.Find("Wheels").transform;

            var Meshes = wheels.GetChild(0);
            var Colliders = wheels.GetChild(1);
            for (int i = 0; i < Colliders.childCount; i++)
            {
                var Suspension = Colliders.GetChild(i).gameObject.AddComponent<Suspension>();
                Suspension.wheelModel = Meshes.transform.Find(Colliders.GetChild(i).name).gameObject;
            }*/

        }
        cars = new GameObject[] { };
        return;
        foreach (var item in cars)
        {

            var name = item.name.Split('_');
            item.name = "c_" + carsId + "_" + name[1];
            item.GetComponent<Rigidbody>().mass = 500;

        }
        cars = new GameObject[] { };
        carsId++;
    }
}
