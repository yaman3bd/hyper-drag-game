using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(ShiftsScript))]
public class PlayerCarController : MonoBehaviour
{
    public float DistancTravled;

    public CarController PCarController;
    // Private variables set at the start
    private ShiftsScript shiftsScript;
    private bool IsRaceStarted;
    private DateTime StartedTime;
    private void Awake()
    {
        StartedTime = DateTime.Now;
        shiftsScript = GetComponent<ShiftsScript>();

    }

    void Start()
    {
        PCarController.GetInitialXPos(0);

        shiftsScript.UpdateEpicShiftValues();
        IsRaceStarted = false;

        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;
        LoadedLevelManager.Instance.OnRaceEnded += OnRaceEnded;

        shiftsScript.OnEpicShift += OnEpicShift;
        shiftsScript.OnTooLateShift += OnTooLateShift;
        shiftsScript.OnTooEarlyShift += OnTooEarlyShift;

    }

    private void OnTooEarlyShift(ShiftState shiftState, float shiftTime)
    {
        PCarController.ShiftDown();
    }

    private void OnTooLateShift(ShiftState shiftState, float shiftTime)
    {
        PCarController.ShiftDown();
    }

    private void OnEpicShift(ShiftState shiftState, float shiftTime)
    {
        PCarController.ShiftUp();
    }

    private void OnRaceEnded()
    {
        LoadedLevelManager.Instance.OnRaceEnded -= OnRaceEnded;
        ToogleHandbrake(true);
        IsRaceStarted = false;
        InGameUIManagerScript.Instance.EndUI.Show();
        InGameUIManagerScript.Instance.Hide();
    }

    private void OnRaceStarted()
    {
        LoadedLevelManager.Instance.OnRaceStarted -= OnRaceStarted;
        PCarController.FreePositionY();

        IsRaceStarted = true;
    }

    void Update()
    {

        shiftsScript.HandleShiftSlider();
        DistancTravled = transform.position.z;


        if (Input.GetMouseButtonDown(0) && IsRaceStarted)
        {
            InGameUIManagerScript.Instance.HideTurtorialUI();
            AudioManager.Instance.Play("ShiftGear");
            shiftsScript.ResetShiftSliderTime();
            shiftsScript.HandleShiftState();
            shiftsScript.UpdateEpicShiftValues();
            var acc = shiftsScript.GetShiftTime();         
            var gears = UpdateDiffGearing();
            PCarController.Drive(acc, gears);
        }
       
    }

    private float UpdateDiffGearing()
    {
        return shiftsScript.GetShiftTime().Remap(shiftsScript.GetMinShiftTime(), shiftsScript.GetMaxShiftTime(), PCarController.diffGearingss[0], PCarController.diffGearingss[1]);
    }
    public void ToogleHandbrake(bool val)
    {
        PCarController.ToogleHandbrake(val);
    }
    public void SetCarToPosition(Vector3 pos)
    {
        
        PCarController.SetCarToPosition(pos);
        PCarController.FreezePositionY();

    }
}

