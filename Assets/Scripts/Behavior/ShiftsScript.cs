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
public class ShiftsScript : MonoBehaviour
{

    //Events
    public delegate void OnShift(ShiftState shiftState, float shiftTime);
    public event OnShift OnEpicShift;
    public event OnShift OnTooCloseShift;
    public event OnShift OnTooEarlyShift;
    public event OnShift OnTooLateShift;

    public EffectsScript EffectsScript;

    [Header("Shifts")]
    public List<ShiftState> ComboShifts;


    [Header("Threshold")]
    public float EpicShiftThreshold;
    public float TooCloseShiftThreshold;


    public Slider ShiftSlider
    {
        get
        {
            return InGameUIManagerScript.Instance.ShiftSlider;
        }
        set
        {
            InGameUIManagerScript.Instance.ShiftSlider = value;
        }
    }
    public Gradient2 ShiftSliderBackground
    {
        get
        {
            return InGameUIManagerScript.Instance.ShiftSliderBackground;
        }
        set
        {
            InGameUIManagerScript.Instance.ShiftSliderBackground = value;
        }
    }

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
    public float ShiftSliderSpeed;
    public float MaxTime;
    public float MinTime;
    public float TimeThreshold;

    private bool IsSliderIncrease;
    private bool IsSliderDecrease;

    private float CurrentSpeed;
    private float EpicShiftTime;

    private EpicShiftValuesScriptableObject EpicShiftsValues;

    private void Start()
    {
        EpicShiftsValues = GameManagment.GameManager.Instance.PlayerEpicShiftsValues;
    }
    private void SpeedUp()
    {
        if (MaxTime >= MinTime)
            MaxTime -= TimeThreshold * Time.deltaTime;
    }
    public void HandleShiftSlider()
    {
        ShiftSlider.value = CurrentSpeed / MaxTime;

        if (ShiftSlider.value <= ShiftSlider.minValue)
        {
            IsSliderIncrease = true;
            IsSliderDecrease = false;
        }
        else if (ShiftSlider.value >= ShiftSlider.maxValue)
        {
            IsSliderIncrease = false;
            IsSliderDecrease = true;
        }

        if (IsSliderIncrease)
        {
            CurrentSpeed += Time.deltaTime * ShiftSliderSpeed;
        }
        else if (IsSliderDecrease)
        {
            CurrentSpeed -= Time.deltaTime * ShiftSliderSpeed;
        }
    }

    public void UpdateEpicShiftValues()
    {

        UnityEngine.Gradient gradient = new UnityEngine.Gradient();
        GradientColorKey[] gck = new GradientColorKey[4];

        Color col1;
        ColorUtility.TryParseHtmlString("#16FF00", out col1);

        Color col2;
        ColorUtility.TryParseHtmlString("#F2F400", out col2);
        Color col3;
        ColorUtility.TryParseHtmlString("#FF0000", out col3);

        gck[0].color = col1;
        gck[1].color = col2;
        gck[2].color = col3;
        gck[3].color = col2;

        EpicShiftsValues.UpdateEpicShiftTimeValue();
        EpicShiftTime = EpicShiftsValues.GetEpicShiftTimeValue();

        gck[0].time = 0;
        gck[1].time = 0.5f;
        gck[2].time = EpicShiftTime;
        gck[3].time = 0.8f;

        gradient.SetKeys(gck, ShiftSliderBackground.EffectGradient.alphaKeys);
        ShiftSliderBackground.EffectGradient = gradient;

    }

    #region Shifts

    public ShiftState GetShiftFromTime(float shiftTime)
    {
        //Epic
        if (shiftTime.IsBetween(EpicShiftTime - EpicShiftThreshold, EpicShiftTime + EpicShiftThreshold))
        {
            return ShiftState.Epic;
        }
        //TooClose
        else if (shiftTime.IsBetween(EpicShiftTime - TooCloseShiftThreshold, EpicShiftTime + TooCloseShiftThreshold))
        {
            return ShiftState.TooClose;
        }
        //Too Late
        else if (shiftTime > (EpicShiftTime + EpicShiftThreshold + TooCloseShiftThreshold))
        {
            return ShiftState.TooLate;
        }
        //Too Early
        else if (shiftTime < (EpicShiftTime - EpicShiftThreshold - TooCloseShiftThreshold))
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
        return ShiftSlider.value;
    }

    public float GetMinShiftTime()
    {
        return ShiftSlider.minValue;
    }

    public float GetMaxShiftTime()
    {
        return ShiftSlider.maxValue;
    }

    #endregion

    public void ResetShiftSliderTime()
    {
        CurrentSpeed = 0;
    }

}
