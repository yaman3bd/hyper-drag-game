using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagment;
public class AICarController : MonoBehaviour
{
    [HideInInspector]
    public CarController PCarController;
   
    public float min, max;

    public float TimeToWaitToDrive;

    // Private variables set at the start
    private EpicShiftValuesScriptableObject EpicShiftsValues;
    private float Acceleration;

    private void Awake()
    {
        EpicShiftsValues = GameManager.Instance.AIEpicShiftsValues;
    }

    IEnumerator MyUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeToWaitToDrive);

            UpdateValues();

            var acc = Acceleration;
            var gears = UpdateDiffGearing();
            PCarController.Drive(acc, gears);

        }
    }

    void Start()
    {
        PCarController.GetInitialXPos(-10);
       
        min = max = 0.1f;
        TimeToWaitToDrive = 0.8f;
      
        UpdateValues();
        
        StartCoroutine(MyUpdate());
    }
    private void UpdateValues()
    {
        EpicShiftsValues.UpdateEpicShiftTimeValue();

        Acceleration = GetNewEpicTime();
    }
    private float GetNewEpicTime()
    {
        var epicTime = EpicShiftsValues.GetEpicShiftTimeValue();
        return Random.Range(epicTime - min, epicTime + max);
    }

    private float UpdateDiffGearing()
    {
        var epicTime = EpicShiftsValues.GetEpicShiftTimeValue();

        return Acceleration.Remap(epicTime - min, epicTime + max, PCarController.diffGearingss[0], PCarController.diffGearingss[1]);
    }
}
