using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public enum ShiftState
{
    None,
    TooLate,
    Epic,
    TooClose,
    TooEarly

}
[System.Serializable]
public class Shifts
{
    public string name;
    public float value;
}
public class ShiftsScript : MonoBehaviour
{

    //Events
    public delegate void OnShift(ShiftState shiftState, float shiftTime);
    public event OnShift OnEpicShift;
    public event OnShift OnTooCloseShift;
    public event OnShift OnTooEarlyShift;
    public event OnShift OnTooLateShift;

    public EffectsScript EffectsScript;
    [Header("New Move")]
    public Text Gerars;
    [SerializeField]
    private float RotationSpeed;
    [SerializeField]
    private float ForwardSpeed;
    [SerializeField]
    private float BackwardSpeed;

    [SerializeField]
    private float RotationLimit;
    [SerializeField]
    private float InitRotationLimit;


    public GameObject Pointer
    {
        get
        {
            return InGameUIManagerScript.Instance.Pointer;
        }
        set
        {
            InGameUIManagerScript.Instance.Pointer = value;
        }
    }


    public float axis;

    [Header("Shifts")]
    public List<ShiftState> ComboShifts;


    [Header("Threshold")]
    public float EpicShiftThreshold = -0.91f;
    public float TooCloseShiftThreshold=-0.84f;
    public float TooEarlyShiftThreshold=-0.57f;
    public float SlowRatio = -0.57f;


   
   

    public TextAnimation ShiftStateText
    {
        get
        {
            return InGameUIManagerScript.Instance.ShiftStateText;
        }
        set
        {
            InGameUIManagerScript.Instance.ShiftStateText = value;
        }
    }

    [Header("Shift Slider Values")]
     private float EpicShiftTime;

    private void Start()
    {
        RotationSpeed = GameManagment.GameManager.Instance.GameSettings.RotationSpeed;
        ForwardSpeed = GameManagment.GameManager.Instance.GameSettings.ForwardSpeed;
        BackwardSpeed = GameManagment.GameManager.Instance.GameSettings.BackwardSpeed;
        InitRotationLimit = GameManagment.GameManager.Instance.GameSettings.RotationLimit;
        EpicShiftThreshold = GameManagment.GameManager.Instance.GameSettings.EpicShiftThreshold;
        TooCloseShiftThreshold  = GameManagment.GameManager.Instance.GameSettings.TooCloseShiftThreshold;
        TooEarlyShiftThreshold = GameManagment.GameManager.Instance.GameSettings.TooEarlyShiftThreshold;
        SlowRatio = GameManagment.GameManager.Instance.GameSettings.SlowRatio;
      
        RotationLimit = InitRotationLimit;
        SlowPointerSpeed = true;
    }
    private bool SlowPointerSpeed;

    public bool CanGear;


    public void HandleShiftSlider()
    {
        var rot = Pointer.transform.rotation;
        if (SlowPointerSpeed && LoadedLevelManager.Instance.Player.CanGear())
        {
            RotationSpeed -= SlowRatio * Time.deltaTime;
        }
        rot.z += axis * RotationSpeed * Time.deltaTime;
        rot.z = Mathf.Clamp(rot.z, -RotationLimit, RotationLimit);


        if (rot.z >= InitRotationLimit)
        {
            axis = -1;
            RotationSpeed = ForwardSpeed;
            SlowPointerSpeed = true;

        }
        if (rot.z <= -InitRotationLimit && LoadedLevelManager.Instance.Player.CanGear())
        {
            ResetShiftSliderTime();
        }

        if (rot.z <= -0.4f)
        {
            CanGear = true;
        }
        else
        {
            CanGear = false;
        }

        Pointer.transform.rotation = rot;

        //var rot = Pointer.transform.rotation;
        //rot.z += axis * RotationSpeed * Time.deltaTime;
        //rot.z = Mathf.Clamp(rot.z, RotationLimit, 0);

       

        //if (Mathf.Abs(rot.z) >= Mathf.Abs(InitRotationLimit))
        //{

        //    RotationLimit = Random.Range(InitRotationLimit, -1f);
        //}
        //if (Mathf.Abs(rot.z) <= 0)
        //{
        //    RotationSpeed = ForwardSpeed;
        //    axis = -1;
        //}
        //Pointer.transform.rotation = rot;

 
       
 
    }

    #region Shifts

    public ShiftState GetShiftFromTime(float pointerRotation)
    {
        var rot = pointerRotation;


        if (rot.IsBetween(TooCloseShiftThreshold, EpicShiftThreshold))
        {
            return ShiftState.Epic;
        }
        else if (rot.IsBetween(TooCloseShiftThreshold, TooEarlyShiftThreshold))
        {
            return ShiftState.TooClose;
        }
        else if (rot.IsBetween(0, Mathf.Abs(EpicShiftThreshold)))
        {
            return ShiftState.TooEarly;
        }

        return ShiftState.None;

    }

    private void __OnTooLateShift()
    {
        ShiftStateText.DoText("Too Late");
        EffectsScript.StopEpicComboEffect();
    }

    private void _OnEpicShift()
    {
        ShiftStateText.DoText("Epic");
        if (ComboShifts.Count == 3)
        {
            StartCoroutine(EffectsScript.PlayEpicEffect());
        }
        else if (ComboShifts.Count > 3)
        {
            EffectsScript.StopEpicEffect();

            EffectsScript.PlayEpicComboEffect();
        }
    }

    private void _OnTooCloseShift()
    {
        ShiftStateText.DoText("Too Close");
    }

    private void _OnTooEarlyShift()
    {
        ShiftStateText.DoText("Too Early");
        EffectsScript.StopEpicEffect();
        EffectsScript.StopEpicComboEffect();
    }

    public void HandleShiftState()
    {
        var shift = GetShiftFromTime(GetShiftTime());

        switch (shift)
        {
            case ShiftState.TooLate:

                if (ComboShifts.Count > 0)
                {

                    int i = ComboShifts.Count >= 2 ? 2 : 1;

                    ComboShifts.RemoveRange(0, i);

                }
                if (OnTooLateShift != null)
                {
                    OnTooLateShift(ShiftState.TooLate, GetShiftTime());
                }
                __OnTooLateShift();

                break;
            case ShiftState.Epic:

                ComboShifts.Add(ShiftState.Epic);
                if (OnEpicShift != null)
                {
                    OnEpicShift(ShiftState.Epic, GetShiftTime());
                }
                _OnEpicShift();

                break;
            case ShiftState.TooClose:

                if (ComboShifts.Count > 0)
                    ComboShifts.RemoveAt(ComboShifts.Count - 1);

                if (OnTooCloseShift != null)
                {
                    OnTooCloseShift(ShiftState.TooClose, GetShiftTime());
                }

                _OnTooCloseShift();

                break;
            case ShiftState.TooEarly:

                if (ComboShifts.Count > 0)
                    ComboShifts.Clear();

                if (OnTooEarlyShift != null)
                {
                    OnTooEarlyShift(ShiftState.TooEarly, GetShiftTime());
                }

                _OnTooEarlyShift();

                break;
            default:
                break;
        }
    }

    public float GetShiftTime()
    {
        return Pointer.transform.rotation.z;
    }


    #endregion

    public void ResetShiftSliderTime()
    {
        SlowPointerSpeed = false;
        ForwardSpeed = 1;
        axis = 1;
        RotationSpeed = BackwardSpeed;


    }

}
