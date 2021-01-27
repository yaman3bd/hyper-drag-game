using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "NewCarsData", menuName = "ScriptableObjects/Cars/Cars Data")]
public class CarsScriptableObject : ScriptableObject
{
    public List<CarDataScriptableObject> CarsDataList;

    public CarDataScriptableObject GetCarByIndex(int index)
    {
        return CarsDataList[index];
    }
    public CarDataScriptableObject GetCarByID(string id)
    {
        return CarsDataList.Where(c => c.ID == id).FirstOrDefault();
    }
}
