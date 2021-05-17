using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TempStop : MonoBehaviour
{
    public float MaxX, MaxZ;
    public float RotationSpeed;
    private void Update()
    {


        var rot = transform.rotation;
        rot.x = Mathf.Clamp(rot.x, -MaxX, MaxX);
        rot.z = Mathf.Clamp(rot.z, -MaxZ, MaxZ);

        //if (Math.Abs(transform.rotation.x) > Math.Abs(rot.x) || Math.Abs(transform.rotation.z) > Math.Abs(rot.z))
        {
            var a = new Quaternion(0, transform.rotation.y, transform.rotation.z, 1);
            transform.rotation = Quaternion.Slerp(transform.rotation, a, RotationSpeed);
        }
    }
}
