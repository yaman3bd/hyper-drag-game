using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
public class EndUIScript : GlobalUIScript
{
    [Header("Images")]
    public Image Background;

    [Header("Texts Rects")]
    public RectTransform EndStateTextLeftRect;
    public RectTransform EndStateTextRightRect;
    public RectTransform EndStateTextOverlayRect;
   
    [Header("Texts Targets")]
    public RectTransform EndStateTextLeftStartTargetRect;
    public RectTransform EndStateTextRightStartTargetRect;
    public RectTransform TextsEndTargetRect;
    
    [Header("Texts Elements")]
    public TMP_Text EndStateTextLeft;
    public TMP_Text EndStateTextRight;
    public TMP_Text EndStateTextOverlay;
    public TMP_Text ReplayText;

    [Header("Buttons")]
    public Button GarageButton;
    public Button ReplayButton;

    [Header("Timers")]
    public float ReplayTextFadeDuration;
    [Space]
    public float BackgroundFadeDuration;
    public float LeftRightTextsMoveDuration;
    public float LeftRightTextsFadeDuration;
    [Space]
    public float OverlayTextMoveDuration;
    public float OverlayTextScaleDuration;
    public float OverlayTextFadeDuration;
    [Header("Ease")]
    public Ease ReplayTextEase;

    private void Start()
    {
        GarageButton.onClick.AddListener(GarageButton_OnClick);
        ReplayButton.onClick.AddListener(ReplayButton_OnClick);
    }

    private void ReplayButton_OnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GarageButton_OnClick()
    {
        SceneManager.LoadScene("Garage");
    }
    private IEnumerator TextFadeInOutAnimation()
    {
        while (true)
        {
            ReplayText.DOFade(1, ReplayTextFadeDuration).SetEase(ReplayTextEase);
            yield return new WaitForSeconds(ReplayTextFadeDuration);
            ReplayText.DOFade(0, ReplayTextFadeDuration).SetEase(ReplayTextEase);
            yield return new WaitForSeconds(ReplayTextFadeDuration);

        }
    }

    private void InitTexts(bool winner)
    {
        Background.SetAlpha(0);

        ReplayText.SetAlpha(0);

        float yPos = TextsEndTargetRect.anchoredPosition.y;

        EndStateTextLeftStartTargetRect.anchoredPosition = new Vector2(EndStateTextLeftStartTargetRect.anchoredPosition.x, yPos);

        EndStateTextLeftRect.anchoredPosition = EndStateTextLeftStartTargetRect.anchoredPosition;
        EndStateTextLeft.SetAlpha(0);
        EndStateTextRightStartTargetRect.anchoredPosition = new Vector2(EndStateTextRightStartTargetRect.anchoredPosition.x, yPos);

        EndStateTextRightRect.anchoredPosition = EndStateTextRightStartTargetRect.anchoredPosition;
        EndStateTextRight.SetAlpha(0);

        EndStateTextOverlayRect.transform.localScale = new Vector2(3, 3);

        EndStateTextOverlay.SetAlpha(0);

        string winnerText = "Winner";

        if (!winner)
        {
            winnerText = "Loser";
        }

        EndStateTextLeft.text = winnerText;

        EndStateTextRight.text = winnerText;

        EndStateTextOverlay.text = winnerText;
    }

   

    private void Animation()
    {
        InitTexts(LoadedLevelManager.Instance.DidWin);

        StartCoroutine(TextFadeInOutAnimation());
 
        Sequence seq = DOTween.Sequence();
       
        seq.AppendCallback(() =>
        {
            
            Background.DOFade(0.8f, BackgroundFadeDuration);

            EndStateTextLeftRect.DOAnchorPos(TextsEndTargetRect.anchoredPosition, LeftRightTextsMoveDuration);
            EndStateTextLeft.DOFade(1, LeftRightTextsFadeDuration);

        }).AppendCallback(() =>
        {
        
            EndStateTextRightRect.DOAnchorPos(TextsEndTargetRect.anchoredPosition, LeftRightTextsMoveDuration);
            EndStateTextRight.DOFade(1, LeftRightTextsFadeDuration);

        }).AppendCallback(() =>
        {

            EndStateTextOverlayRect.DOAnchorPos(TextsEndTargetRect.anchoredPosition, OverlayTextMoveDuration);
            EndStateTextOverlayRect.DOScale(1, OverlayTextScaleDuration);
            EndStateTextOverlay.DOFade(1, OverlayTextFadeDuration);

        });

    }
    public override void Show()
    {
        base.Show(); 
        Animation();
    }

}
