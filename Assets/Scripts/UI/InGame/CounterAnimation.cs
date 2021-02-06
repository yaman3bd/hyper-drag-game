using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;

public class CounterAnimation : MonoBehaviour
{
    public TMP_Text CounterText;


    public float fade, scale;
    public float minScale, maxScale;
    private void InitText(string val)
    {
        CounterText.text = val;
        CounterText.gameObject.SetActive(true);
        CounterText.transform.localScale = new Vector3(minScale, minScale, minScale);
        CounterText.SetAlpha(1);
    }

    public void Animation(string val)
    {
        InitText(val);

        /* Sequence sequence = DOTween.Sequence();
         CounterText.text = timer.ToString();
         //0.2f+fade+fade / 2+scale+scale / 2+fade / 2
         sequence.AppendCallback(() =>
         {
             start = DateTime.UtcNow;
         }).AppendInterval(0.2f).AppendCallback(() =>
         {
             CounterText.text = timer.ToString();

             CounterText.DOFade(1, fade);

         }).AppendInterval(fade / 2).AppendCallback(() =>
           {
               CounterText.transform.DOScale(maxScale, scale);

           }).AppendInterval(scale).AppendCallback(() =>
           {
               CounterText.transform.DOScale(minScale, scale);

           }).AppendInterval(scale / 2).AppendCallback(() =>
             {
                 CounterText.DOFade(0, fade / 2).OnComplete(() =>
                 {
                     timer--;
                     if (timer != 0)
                         Animation(timer.ToString());
                 });
             });*/

    }

    public void End()
    {
        Destroy(this.gameObject);
    }

}
