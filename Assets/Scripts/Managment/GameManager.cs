using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
namespace GameManagment
{
    public class GameManager : Singleton<GameManager>
    {
        public CarsScriptableObject CarsData;
        public CarsIDsScriptableObject CarsIds;
        public EpicShiftValuesScriptableObject PlayerEpicShiftsValues;
        public EpicShiftValuesScriptableObject AIEpicShiftsValues;
        public ScenesManager ScenesManager;
        public string SelectedCarID
        {
            get
            {
                return TempSavedDataSettings.GetCarID();
            }
        }
        public CarDataScriptableObject SelectedCarData
        {
            get
            {
                return this.CarsData.GetCarByID(TempSavedDataSettings.GetCarID());
            }
        }
    }
}