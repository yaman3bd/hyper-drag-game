using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagment;
public class AICarController : MonoBehaviour
{
    public float DistancTravled;
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
    private float shiftDelay;
 
    private void Update()
    {
        DistancTravled = transform.position.z;
    }
    IEnumerator MyUpdate()
    {
        while (true)
        {


            UpdateValues();

            var acc = Acceleration;
            var gears = UpdateDiffGearing();
            float now = Time.timeSinceLevelLoad;

            if (now > shiftDelay)
            {
                PCarController.ShiftUp();
                shiftDelay = now + Random.Range(1f, 2f);
            }

            PCarController.Drive(acc, gears);
            yield return new WaitForSeconds(TimeToWaitToDrive);
        }
    }

    void Start()
    {
        PCarController.GetInitialXPos(-10);

        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;

    }
    private void OnRaceStarted()
    {
        LoadedLevelManager.Instance.OnRaceStarted -= OnRaceStarted;

        PCarController.FreePositionY();
        min = max = 0.1f;
        TimeToWaitToDrive = 0.8f;

        UpdateValues();

        StartCoroutine("MyUpdate");
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
    
    public void ToogleHandbrake(bool val)
    {
        StopCoroutine("MyUpdate");
        PCarController.ToogleHandbrake(val);
    }
    public void SetCarToPosition(Vector3 pos)
    {
        PCarController.SetCarToPosition(pos);
        PCarController.FreezePositionY();
    }
}
