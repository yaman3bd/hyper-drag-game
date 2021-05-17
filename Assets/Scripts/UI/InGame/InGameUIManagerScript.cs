using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using DG.Tweening;
using System;
using TMPro;
using GameManagment;

public class InGameUIManagerScript : GlobalUIScript
{
    public static InGameUIManagerScript Instance;
    public EndUIScript EndUI;
    public TutorialUIScript TutorialUI;
    [Header("Buttons")]
    public Button ReloadButton;
    [Header("Nitro")]
    public Button NitroButton;
    public Image NitroFillImage;
    [Header("Gears")]
    public GameObject Pointer;


    [Header("UI")]
     public TextAnimation ShiftStateText;
    public CanvasGroup InGameUIElementsCanvasGroup;

    [Header("Texts Rects")]
    public RectTransform ShiftSliderRect;
    public RectTransform ReloadButtonRect;
    [Header("Texts Targets")]
    public RectTransform ShiftSliderStartTargetRect;
    public RectTransform ReloadButtonStartTargetRect;
    [Space]
    public RectTransform ShiftSliderEndTargetRect;
    public RectTransform ReloadButtonEndTargetRect;

    [Header("Timers")]
    public float ShiftSliderMoveDuration;
     public float InGameUIElementsCanvasGroupFadeDuration;
    [Header("Progress")]
    public Slider PlayerProgress;
    public Slider AIProgress;

    private bool RaceStarted;
    private void Awake()
    {
        Instance = this;
        RaceStarted = false;
    }
    private void InitAnimation()
    {

        InGameUIElementsCanvasGroup.alpha = 0;

        ShiftSliderRect.anchoredPosition = ShiftSliderStartTargetRect.anchoredPosition;

        ReloadButtonRect.anchoredPosition = ReloadButtonStartTargetRect.anchoredPosition;
    }
    private void StartAnimation()
    {

        InGameUIElementsCanvasGroup.DOFade(1, InGameUIElementsCanvasGroupFadeDuration);

        ShiftSliderRect.DOAnchorPos(ShiftSliderEndTargetRect.anchoredPosition, ShiftSliderMoveDuration);
       

        ReloadButtonRect.DOAnchorPos(ReloadButtonEndTargetRect.anchoredPosition, ShiftSliderMoveDuration);

    }
    private void EndAnimation()
    {

        InGameUIElementsCanvasGroup.DOFade(0, InGameUIElementsCanvasGroupFadeDuration);

        ShiftSliderRect.DOAnchorPos(ShiftSliderStartTargetRect.anchoredPosition, ShiftSliderMoveDuration);

 

    }
    private void Start()
    {
        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;
        LoadedLevelManager.Instance.OnBeforeRaceStarted += OnBeforeRaceStarted;
      
        ReloadButton.onClick.AddListener(ReloadButton_OnClikc);


        NitroButton.onClick.AddListener(NitroButton_OnClick);
    }

    private void NitroButton_OnClick()
    {
        LoadedLevelManager.Instance.Player.ActiveBoost();
    }

    private void ReloadButton_OnClikc()
    {
        if (!RaceStarted)
        {
            return;
        }
       
        GameManager.Instance.ScenesManager.LoadScene("InGame");
    }

    private void OnBeforeRaceStarted()
    {
        LoadedLevelManager.Instance.OnBeforeRaceStarted -= OnBeforeRaceStarted;

        InitAnimation();
        StartAnimation();
    }
     private void OnRaceStarted()
    {
        LoadedLevelManager.Instance.OnRaceStarted -= OnRaceStarted;

        if (!TempSavedDataSettings.IsTutorialPlayed())
        {
            TutorialUI.Show();
        }
        RaceStarted = true;
    }
    public void HideTurtorialUI()
    {
        if (TutorialUI.gameObject.activeSelf)
        {
            TutorialUI.Hide();
        }
    }

    private void Progress()
    {

        float playerProgress = LoadedLevelManager.Instance.Player.DistancTravled / LoadedLevelManager.Instance.EndLine.transform.position.z;
        float aiProgress = LoadedLevelManager.Instance.AI.DistancTravled / LoadedLevelManager.Instance.EndLine.transform.position.z;

        PlayerProgress.value = playerProgress;
        AIProgress.value = aiProgress;

    }
    private bool boosted;
    private void Update()
    {
        Progress();
        
        if (!boosted && LoadedLevelManager.Instance.Player.CanBoost())
        {
            boosted = true;
            NitroButton.gameObject.SetActive(true);
        }
        NitroFillImage.fillAmount = LoadedLevelManager.Instance.Player.GetAvailableBoost();
    }
}
