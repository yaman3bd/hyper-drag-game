using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GearsUI : MonoBehaviour
{
    [Header("test")]
    public float CurrentSpeed, MaxSpeed;
    public int NoOfGears;
    public int m_GearNum;
    private void GearChanging()
    {
        float f = Mathf.Abs(CurrentSpeed / MaxSpeed);
        float upgearlimit = (1 / (float)NoOfGears) * (m_GearNum + 1);
        float downgearlimit = (1 / (float)NoOfGears) * m_GearNum;

        if (m_GearNum > 0 && f < downgearlimit)
        {
            m_GearNum--;
        }

        if (f > upgearlimit && (m_GearNum < (NoOfGears - 1)))
        {
            m_GearNum++;
        }
    }
    public TestCarMovement test;
    public float startPosiziton, endPosition;
    public float desiredPosition;
    public float carSpeed;
    public float maxcarSpeed;
    public TMPro.TMP_Text SpeedText;
    public TMPro.TMP_Text GearsText;
    public void updateNeedle()
    {
        GearsText.text = (test.gearNum + 1).ToString();
        SpeedText.text = ((int)test.KPH).ToString();
        float totalAngle = startPosiziton - endPosition;
        float speednr = test.KPH / test.gearChangeSpeed[5];
        float rot = startPosiziton - speednr * totalAngle;
       // Pointer.transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(startPosiziton, endPosition, test.KPH / test.gearChangeSpeed[5]));
        Pointer.transform.eulerAngles = new Vector3(0, 0, rot);

    }
    [SerializeField]
    private float ForwardSpeed;
    [SerializeField]
    private float BackwardSpeed;
    [SerializeField]
    private float InitRotationLimit;

    public float SlowRatio = -0.57f;
    public GameObject Pointer;

    private float RotationLimit;
    public float RotationSpeed;
    private float axis;
    // Start is called before the first frame update
    void Start()
    {
        RotationLimit = InitRotationLimit;
        RotationSpeed = ForwardSpeed;
        axis = -1;
    }
    public bool SlowPointerSpeed;
    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.S))
        {
 
            Debug.LogError(Pointer.transform.rotation.z);
        }
        return;
        var rot = Pointer.transform.rotation;
        if (SlowPointerSpeed)
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
        if (rot.z <= -InitRotationLimit)
        {
            SlowPointerSpeed = false;
            ForwardSpeed = 1;
            axis = 1;
            RotationSpeed = BackwardSpeed;
        }

        Pointer.transform.rotation = rot;

    }
    private void FixedUpdate()
    {
        //updateNeedle();
    }

}
