using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using DG.Tweening;
using System;
using TMPro;
public class InGameUIManagerScript :GlobalUIScript
{
    public static InGameUIManagerScript Instance;
    public EndUIScript EndUI;
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

    [Header("Texts Targets")]
    public RectTransform ShiftSliderStartTargetRect;
    public RectTransform ShiftStateTextStartTargetRect;
    public RectTransform SpeedTextStartTargetRect;
    [Space]
    public RectTransform ShiftSliderEndTargetRect;
    public RectTransform ShiftStateTextEndTargetRect;
    public RectTransform SpeedTextEndTargetRect;
    [Header("Timers")]
    public float ShiftSliderMoveDuration;
    public float SpeedTextMoveDuration;
    public float ShiftStateTextMoveDuration;
    public float InGameUIElementsCanvasGroupFadeDuration;


    private void Awake()
    {
        Instance = this;
    }
    private void InitAnimation()
    {

        InGameUIElementsCanvasGroup.alpha = 0;

        ShiftSliderRect.anchoredPosition = ShiftSliderStartTargetRect.anchoredPosition;

        ShiftStateTextRect.anchoredPosition = ShiftStateTextEndTargetRect.anchoredPosition;

        SpeedTextRect.anchoredPosition = SpeedTextStartTargetRect.anchoredPosition;

    }
    private void StartAnimation()
    {

        InGameUIElementsCanvasGroup.DOFade(1, InGameUIElementsCanvasGroupFadeDuration);

        ShiftSliderRect.DOAnchorPos(ShiftSliderEndTargetRect.anchoredPosition, ShiftSliderMoveDuration);

        SpeedTextRect.DOAnchorPos(SpeedTextEndTargetRect.anchoredPosition, SpeedTextMoveDuration);

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
    }

    private void OnRaceStarted()
    {
        LoadedLevelManager.Instance.OnRaceStarted -= OnRaceStarted;

        InitAnimation();
        StartAnimation();
    }
    private void Update()
    {
        TimeText.text = DateTime.Now.Second.ToString();
    }
}
