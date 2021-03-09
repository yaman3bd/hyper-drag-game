using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GarageCarData
{
    public string ID;
    public bool Isloaded;
   // public bool IsActive;
    public GameObject Model;
}
public class GarageCarsLoader : MonoBehaviour
{
    public Dictionary<string, GarageCarData> GarageCars;
    public string CarsResourcesPath;

    private void Awake()
    {
        GarageCars = new Dictionary<string, GarageCarData>();
    }
    /// <summary>
    /// returns a car by it's id, if does not exist Instantiate it
    /// </summary>
    /// <param name="id">car id</param>
    /// <param name="isActive">is active after Instantiate</param>
    /// <returns></returns>
    public GarageCarData GetCar(string id, string fullName, bool isActive = false)
    {

        if (!GarageCars.ContainsKey(id+"_"+fullName))
        {
            LoadCar(id, fullName, isActive);
        }

        return GarageCars[id + "_" + fullName];

    }
    private void LoadCar(string id, string fullName, bool isActive)
    {

        var garageCar = new GarageCarData();
        garageCar.ID = id;
        garageCar.Isloaded = true;
        // garageCar.IsActive = isActive;
        GameObject car = Instantiate(Resources.Load<GameObject>(CarsResourcesPath + id + "/" + fullName));
        car.SetActive(isActive);
        garageCar.Model = car;
        GarageCars.Add(id + "_" + fullName, garageCar);
    }

}
