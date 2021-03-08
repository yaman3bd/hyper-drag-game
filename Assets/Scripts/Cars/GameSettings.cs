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
    public ForceMode ForceMode;
    public float ForcePower;
    public float[] GearBox;

}
