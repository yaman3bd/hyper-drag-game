using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(ShiftsScript))]
[RequireComponent(typeof(CarController))]
public class PlayerCarController : MonoBehaviour
{
    public float DistancTravled;

    public CarController PCarController;
    // Private variables set at the start
    private ShiftsScript shiftsScript;
    private bool IsRaceStarted;
    public float Speed
    {
        get
        {
            return PCarController.KPH;
        }
    }
    public float Gear
    {
        get
        {
            return PCarController.CurrentGear;
        }
    }
    private void Awake()
    {
        shiftsScript = GetComponent<ShiftsScript>();

    }

    void Start()
    {
        PCarController.SetInitialXPos(0);

        IsRaceStarted = false;

        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;
        LoadedLevelManager.Instance.OnRaceEnded += OnRaceEnded;

        shiftsScript.OnEpicShift += OnEpicShift;
        shiftsScript.OnTooLateShift += OnTooLateShift;
        shiftsScript.OnTooEarlyShift += OnTooEarlyShift;

    }

    private void OnTooEarlyShift(ShiftState shiftState, float shiftTime)
    {
        //PCarController.ShiftDown();
    }

    private void OnTooLateShift(ShiftState shiftState, float shiftTime)
    {
        //PCarController.ShiftUp();
    }

    private void OnEpicShift(ShiftState shiftState, float shiftTime)
    {
        //PCarController.ShiftUp();

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

        IsRaceStarted = true;
    }

    void Update()
    {
        if (IsRaceStarted)
        {
            shiftsScript.HandleShiftSlider();
        }
        DistancTravled = transform.position.z;

        /* if (Input.GetMouseButtonDown(0) && IsRaceStarted)
         {
             AudioManager.Instance.Play("ShiftGear");
             shiftsScript.ResetShiftSliderTime();
             shiftsScript.HandleShiftState();
         }*/

    }
    public void ChangeGear(bool up)
    {
        AudioManager.Instance.Play("ShiftGear");
        InGameUIManagerScript.Instance.HideTurtorialUI();
        shiftsScript.HandleShiftState();

        if (up)
        {
            var acc = shiftsScript.GetShiftTime();

            PCarController.Drive(acc);

            PCarController.ShiftUp();
        }
        else
        {
            PCarController.ShiftDown();
        }
        shiftsScript.ResetShiftSliderTime();

    }
    public void ToogleHandbrake(bool val)
    {
        PCarController.ToogleHandbrake(val);
    }
    public void SetCarToPosition(Vector3 pos)
    {
        
        PCarController.SetCarToPosition(pos);

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
    public float GetAvailableBoost()
    {
        return PCarController.GetAvailableBoost();
    }
}

