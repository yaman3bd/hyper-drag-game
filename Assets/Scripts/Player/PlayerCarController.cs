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


    private void Awake()
    {
        shiftsScript = GetComponent<ShiftsScript>();

    }

    void Start()
    {
        PCarController.GetInitialXPos(0);

        shiftsScript.UpdateEpicShiftValues();

    }

    void Update()
    {

        shiftsScript.HandleShiftSlider();


        if (Input.GetMouseButtonDown(0))
        {
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

}

