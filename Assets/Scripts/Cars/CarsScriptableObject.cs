using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "NewCarsData", menuName = "ScriptableObjects/Cars/Cars Data")]
public class CarsScriptableObject : ScriptableObject
{
    public List<CarDataScriptableObject> CarsDataList;

    public List<CarDataScriptableObject> FilteredCarsDataList;


    public CarDataScriptableObject GetCarByIndex(int index)
    {
        return FilteredCarsDataList[index];
    }
    public CarDataScriptableObject GetCarByID(string id)
    {
        return FilteredCarsDataList.Where(c => c.ID == id).FirstOrDefault();
    }
    public CarDataScriptableObject GetRandomCar()
    {
        var id = Random.Range(0, FilteredCarsDataList.Count - 1);

        return GetCarByIndex(id);

    }
    public void Filter()
    {
        FilteredCarsDataList = CarsDataList.Where(c => c.IsAvailable).ToList();
    }
}
