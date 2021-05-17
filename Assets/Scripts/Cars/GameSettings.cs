using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class CarID
{
    public string Name;
    public string ID;
}
[CreateAssetMenu(fileName = "NewGameSettings", menuName = "ScriptableObjects/Settings/Game Settings")]

public class GameSettings : ScriptableObject
{
    public bool LimitRotation;
    [Header("Wheels")]
    public float WheelDampingRate;
    public float SuspensionDistance;
    public float Spring;
    public float Damper;
    public float TargetPosition;
    [Header("")]
    public float BodyMass;
    public float AngularDrag;
    public ForceMode ForceMode;
    public float ForcePower;
    public float DownForce;
    public float MaxX;
    public float[] GearBox;
    public float[] GearBoxSpeedLimits;
    public float MaxRPM;
    public float MaxGearForceTime;
    public float GearForceTime;
    [Range(0f, 1f)]
    public float GearForceRegen;
    [Header("Boost")]
    public float MaxBoostTime;
    public float BoostTime;
    [Range(0f, 1f)]
    public float BoostRegen;
    public float BoostForce;
    public float smoothTime;
    [Header("Gears")]
    public float RotationSpeed;
    public float ForwardSpeed;
    public float BackwardSpeed;
    public float RotationLimit;

    public float EpicShiftThreshold;
    public float TooCloseShiftThreshold;
    public float TooEarlyShiftThreshold;
    public float SlowRatio;

}
