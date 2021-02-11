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
    [Header("UI")]
    public TMP_Text TimeText;
    public Slider ShiftSlider;
    public Gradient2 ShiftSliderBackground;
    public TextAnimation ShiftStateText;
    public CanvasGroup InGameUIElementsCanvasGroup;

    [Header("Texts Rects")]
    public RectTransform ShiftSliderRect;
    public RectTransform ShiftStateTextRect;
    public RectTransform SpeedTextRect;
    public RectTransform TimeTextRect;
    public RectTransform ReloadButtonRect;
    [Header("Texts Targets")]
    public RectTransform ShiftSliderStartTargetRect;
    public RectTransform ShiftStateTextStartTargetRect;
    public RectTransform SpeedTextStartTargetRect;
    public RectTransform TimeTextStartTargetRect;
    public RectTransform ReloadButtonStartTargetRect;
    [Space]
    public RectTransform ShiftSliderEndTargetRect;
    public RectTransform ShiftStateTextEndTargetRect;
    public RectTransform SpeedTextEndTargetRect;
    public RectTransform TimeTextEndTargetRect;
    public RectTransform ReloadButtonEndTargetRect;

    [Header("Timers")]
    public float ShiftSliderMoveDuration;
    public float SpeedTextMoveDuration;
    public float ShiftStateTextMoveDuration;
    public float InGameUIElementsCanvasGroupFadeDuration;

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

        //  ShiftStateTextRect.anchoredPosition = ShiftStateTextEndTargetRect.anchoredPosition;

        SpeedTextRect.anchoredPosition = SpeedTextStartTargetRect.anchoredPosition;
        
        TimeTextRect.anchoredPosition = TimeTextStartTargetRect.anchoredPosition;

        ReloadButtonRect.anchoredPosition = ReloadButtonStartTargetRect.anchoredPosition;

    }
    private void StartAnimation()
    {

        InGameUIElementsCanvasGroup.DOFade(1, InGameUIElementsCanvasGroupFadeDuration);

        ShiftSliderRect.DOAnchorPos(ShiftSliderEndTargetRect.anchoredPosition, ShiftSliderMoveDuration);

        SpeedTextRect.DOAnchorPos(SpeedTextEndTargetRect.anchoredPosition, SpeedTextMoveDuration);
       
        TimeTextRect.DOAnchorPos(TimeTextEndTargetRect.anchoredPosition, SpeedTextMoveDuration);

        ReloadButtonRect.DOAnchorPos(ReloadButtonEndTargetRect.anchoredPosition, ShiftSliderMoveDuration);

    }
    private void EndAnimation()
    {

        InGameUIElementsCanvasGroup.DOFade(0, InGameUIElementsCanvasGroupFadeDuration);

        ShiftSliderRect.DOAnchorPos(ShiftSliderStartTargetRect.anchoredPosition, ShiftSliderMoveDuration);

        ShiftStateTextRect.DOAnchorPos(ShiftStateTextStartTargetRect.anchoredPosition, ShiftStateTextMoveDuration);

        SpeedTextRect.DOAnchorPos(SpeedTextStartTargetRect.anchoredPosition, SpeedTextMoveDuration);

    }
    private void Start()
    {
        LoadedLevelManager.Instance.OnRaceStarted += OnRaceStarted;
        LoadedLevelManager.Instance.OnBeforeRaceStarted += OnBeforeRaceStarted;
      
        ReloadButton.onClick.AddListener(ReloadButton_OnClikc);
    }

    private void ReloadButton_OnClikc()
    {
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

    private void Update()
    {
        TimeText.text = "Time: " + ((int)LoadedLevelManager.Instance.PlayerRaceTime).ToString();
    }
}
