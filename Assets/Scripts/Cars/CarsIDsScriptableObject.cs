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
[CreateAssetMenu(fileName = "NewCarsIDs", menuName = "ScriptableObjects/Cars/Cars IDs")]

public class CarsIDsScriptableObject : ScriptableObject
{
    public List<CarID> CarsIdsList;
}
