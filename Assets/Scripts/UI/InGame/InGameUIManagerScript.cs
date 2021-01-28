using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class InGameUIManagerScript : MonoBehaviour
{
    public static InGameUIManagerScript Instance;
    [Header("UI")]
    public Slider ShiftSlider;
    public Gradient2 ShiftSliderBackground;
    public TextAnimation ShiftStateText;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
 
}
