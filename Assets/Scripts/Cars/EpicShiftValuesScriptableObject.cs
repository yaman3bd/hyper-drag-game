using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEpicShiftsValues", menuName = "ScriptableObjects/Cars/Epic Shifts Values")]
public class EpicShiftValuesScriptableObject : ScriptableObject
{

    [SerializeField]
    private float EpicShiftTime;
    [Header("Epic Shift Values")]
    [Range(0.0f, 1.0f)]
    [SerializeField] 
    private float EpicShiftMinTime;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float EpicShiftMaxTime;

    
    public void UpdateEpicShiftTimeValue()
    {
        EpicShiftTime = Random.Range(EpicShiftMinTime, EpicShiftMaxTime);
    }
    public float GetEpicShiftTimeValue()
    {
        return EpicShiftTime;
    }
}
