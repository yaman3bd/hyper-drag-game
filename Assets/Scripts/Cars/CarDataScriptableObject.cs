using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "NewCarData", menuName = "ScriptableObjects/Cars/Car Data")]
public class CarDataScriptableObject : ScriptableObject
{
    public string Name;
    public string ID;
    public int Price;
    public int Rank;

    public string GetRankName()
    {
        Debug.LogError("CarDataScriptableObject/GetRank/ Car Rank Not Set Yet!");
      
        return "not set";
    }
}
