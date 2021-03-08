using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
namespace GameManagment
{
    public class GameManager : Singleton<GameManager>
    {
        public string DefaultCarID;
        public GameSettings GameSettings;
        public BackButtonScript BackButton;
        public CarsScriptableObject CarsData;
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

        protected override void Awake()
        {
            base.Awake();
            CarsData.Filter();
        }
    }
}