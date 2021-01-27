using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{

    public CarController PCarController;
    // Private variables set at the start
    private ShiftsScript shiftsScript;
    public float min, max;
    public float d;
    private void Awake()
    {
        //get the random value
        shiftsScript = GetComponent<ShiftsScript>();
    }
    IEnumerator MyUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(d);
            shiftsScript.UpdateEpicShiftValues();
            timer = Random.Range(shiftsScript.EpicShiftTime - min, shiftsScript.EpicShiftTime + max);
            var acc = timer;
            var gears = UpdateDiffGearing();
            PCarController.Drive(acc, gears);

        }
    }

    public float timer;
    void Start()
    {

        shiftsScript.UpdateEpicShiftValues();
        timer = Random.Range(shiftsScript.EpicShiftTime - min, shiftsScript.EpicShiftTime + max);
        StartCoroutine(MyUpdate());
    }



    private float UpdateDiffGearing()
    {
        return timer.Remap(shiftsScript.EpicShiftTime - min, shiftsScript.EpicShiftTime + max, PCarController.diffGearingss[0], PCarController.diffGearingss[1]);
    }
}
