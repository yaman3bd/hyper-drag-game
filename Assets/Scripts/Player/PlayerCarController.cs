using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

[RequireComponent(typeof(ShiftsScript))]
public class PlayerCarController : MonoBehaviour
{

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

        shiftsScript.HandleShiftSlider();


        if (Input.GetMouseButtonDown(0) && IsRaceStarted)
        {
            InGameUIManagerScript.Instance.HideTurtorialUI();
            shiftsScript.ResetShiftSliderTime();
            shiftsScript.HandleShiftState();
            shiftsScript.UpdateEpicShiftValues();
            var acc = shiftsScript.GetShiftTime();         
            var gears = UpdateDiffGearing();
            PCarController.Drive(acc, gears);
        }
        if (!IsRaceStarted && transform.position.y >= 5)
        {
            Debug.Break();
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
    }
}

