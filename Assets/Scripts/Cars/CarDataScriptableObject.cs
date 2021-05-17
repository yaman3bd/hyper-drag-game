using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TypesofCar
{
    Common,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "NewCarData", menuName = "ScriptableObjects/Cars/Car Data")]
public class CarDataScriptableObject : ScriptableObject
{
    [Header("Movement")]
    public float MaxSpeed;
    public float Acceleration;
    public float NitroForc;
    public float StartAcceleration;
    public float Drag;
    public int MaxGears;
    [Header("Sounds")]
    public AudioClip EngineSound;
    public AudioClip NitroSound;
    public AudioClip GearChangeSound;
    [Header("Store")]
    public bool IsAvailable;
    public string Name;
    public string ID;
    public string ClassName;
    public TypesofCar TypesofCar;
    public int Price;
    public int Rank;
    public List<string> ColorsNames;
    public List<Color> Colors;

    public string DefaultColorsName;
    public string GetRankName()
    {

        return TypesofCar.ToString();
    }

    public string GetColorName()
    {
        string colorsName = TempSavedDataSettings.GetCarColorName(this.ID);
        if (string.IsNullOrEmpty(colorsName))
        {
            colorsName = DefaultColorsName;
        }
        return colorsName;
    }
    public string GetRandomColorName()
    {
        var colorName = UnityEngine.Random.Range(0, ColorsNames.Count);
        return ColorsNames[colorName];
    }
}
