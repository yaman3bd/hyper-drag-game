using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class ScenesManager : MonoBehaviour
{

    private AsyncOperation Operation;
    private Canvas Canvas;
    public Slider LoadingProgressSlider;
    public TMP_Text LaodingText;
    public bool LoadDone;
    private void Awake()
    {
        Canvas = GetComponent<Canvas>();
        LoadDone = false;
    }
    private void Start()
    {
        UpdateProgress(0.5f);
        Canvas.gameObject.SetActive(true);
    }
    public void LoadScene(string name)
    {

        UpdateProgress(0);
        Canvas.gameObject.SetActive(true);
        SceneManager.LoadScene(name);
        UpdateProgress(0.5f);

    }

    //IEnumerator Load(string name)
    //{

    //    while (!Operation.isDone)
    //    {
    //        UpdateProgress(Operation.progress);
    //        yield return new WaitForSeconds(0.2f);
    //    }


    //    UpdateProgress(Operation.progress);
    //    Operation = null;
    //    Canvas.gameObject.SetActive(false);

    //}
    public void UpdateProgress(float value)
    {
        LoadingProgressSlider.DOValue(value, 1f).OnUpdate(() =>
        {
            LaodingText.text = string.Format("Loading {0}%", (int)(LoadingProgressSlider.value * 100));
        }).OnComplete(() =>
        {
            if (value >= 1)
            {
                UpdateProgress(0);
                Canvas.gameObject.SetActive(false);
                LoadDone = true;
            }
        });
    }

}
