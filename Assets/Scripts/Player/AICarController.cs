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
    private bool boosted;
    IEnumerator MyUpdate()
    {
        while (true)
        {


            UpdateValues();

            var acc = Acceleration;
            float now = Time.timeSinceLevelLoad;

            if (InGameUIManagerScript.Instance.Pointer.transform.rotation.z <= -0.4f && CanGear())
            {
                PCarController.ShiftUp();
                PCarController.Drive(acc);
                shiftDelay = now + Random.Range(1f, 2f);
            }
            if (!boosted && CanBoost())
            {
                boosted = true;
                ActiveBoost();
            }
            yield return new WaitForSeconds(TimeToWaitToDrive);
        }
    }
    public bool CanGear()
    {
        return PCarController.CanGear();
    }
    public bool CanBoost()
    {
        return PCarController.CanBoost();
    }
    public void ActiveBoost()
    {
        PCarController.ActiveBoost();
    }
   
    void Start()
    {
        PCarController.SetInitialXPos(-10);

        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;

    } 
    private void OnRaceStarted()
    {
        LoadedLevelManager.Instance.OnRaceStarted -= OnRaceStarted;

        min = max = 0.1f;

        UpdateValues();

        StartCoroutine("MyUpdate");
    }
    private void UpdateValues()
    {
        TimeToWaitToDrive = Random.Range(1f, 1.2f);
        EpicShiftsValues.UpdateEpicShiftTimeValue();

        Acceleration = GetNewEpicTime();
    }
    private float GetNewEpicTime()
    {
        var epicTime = EpicShiftsValues.GetEpicShiftTimeValue();
        return Random.Range(epicTime - min, epicTime + max);
    }
    
    public void ToogleHandbrake(bool val)
    {
        StopCoroutine("MyUpdate");
        PCarController.ToogleHandbrake(val);
    }
    public void SetCarToPosition(Vector3 pos)
    {
        PCarController.SetCarToPosition(pos);
    }
}
