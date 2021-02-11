using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoaderUIScript : MonoBehaviour
{
    public Slider LoadingProgressSlider;
    public TMP_Text LaodingText;

    public void UpdateProgress(float value)
    {
        LoadingProgressSlider.value = value;
        LaodingText.text = string.Format("Loading {0}%", (int)value);
    }
}
