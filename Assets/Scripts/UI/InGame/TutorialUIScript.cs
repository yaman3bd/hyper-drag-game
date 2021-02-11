using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class TutorialUIScript : GlobalUIScript
{
    public RectTransform Content;
    public TMP_Text TapText;
    public RectTransform TapImage;
    public float TapImageScaleDuration;
    public float TapTextFadeDuration;
    public Ease Ease;
    
    private IEnumerator TextFadeInOutAnimation()
    {
        while (true)
        {
            TapText.DOFade(1, TapTextFadeDuration).SetEase(Ease);
            yield return new WaitForSeconds(TapTextFadeDuration);
            TapText.DOFade(0.3f, TapTextFadeDuration).SetEase(Ease);
            yield return new WaitForSeconds(TapTextFadeDuration);

        }
    }
    private IEnumerator ImageScaleInOutAnimation()
    {
        while (true)
        {
            TapImage.DOScale(0.7f, TapImageScaleDuration).SetEase(Ease);
            yield return new WaitForSeconds(TapImageScaleDuration);
            TapImage.DOScale(1, TapImageScaleDuration).SetEase(Ease);
            yield return new WaitForSeconds(TapImageScaleDuration);

        }
    }
    
    public override void Show()
    {
        base.Show();
        StartCoroutine("TextFadeInOutAnimation");
        StartCoroutine("ImageScaleInOutAnimation");
    }

    public override void Hide()
    {
        base.Hide();
        StopCoroutine("TextFadeInOutAnimation");
        StopCoroutine("ImageScaleInOutAnimation");
        TempSavedDataSettings.TutorialPlayed();
    }
}
