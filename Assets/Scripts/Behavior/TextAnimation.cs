using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class TextAnimation : MonoBehaviour
{

    public TMP_Text text;
    public float ScaleTime;
    public float FadeTime;
    public float IntervalTime;
    private void SetText(string textVal)
    {
        text.text = textVal;
        text.transform.localScale = Vector3.zero;
        text.SetAlpha(0);
    }
    Sequence sq;
    public void DoText(string textVal)
    {

        SetText(textVal);
        if (sq!=null)
        {
              sq.Kill();
        }
        sq = DOTween.Sequence();
        
        sq.AppendCallback(() =>
        {
        
            text.transform.DOScale(Vector3.one, ScaleTime);
            text.DOFade(1, FadeTime);
     
        }).AppendInterval(IntervalTime).AppendCallback(() =>
        {
            
            text.transform.DOScale(Vector3.zero, ScaleTime);
            text.DOFade(0, FadeTime);
       
        });

    }
    private void Start()
    {
        SetText("");
    }

    // Start is called before the first frame update

    // Update is called once per frame
    //private void Start()
    //{
    //   // var scaleTo = new Vector3(1.5f, 1.5f, 1.5f);

    //    //StartCoroutine(ScaleOverSeconds(text.gameObject, scaleTo, ScaleTime));
    //}
    //public IEnumerator ScaleOverSeconds(GameObject objectToScale, Vector3 scaleTo, float seconds)
    //{
    //    float elapsedTime = 0;
    //    Vector3 startingScale = objectToScale.transform.localScale;
    //    while (elapsedTime < seconds)
    //    {
    //        objectToScale.transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / seconds));
    //        elapsedTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    yield return new WaitForSeconds(1);

    //    scaleTo = Vector3.zero;
    //    elapsedTime = 0; startingScale = objectToScale.transform.localScale;
    //    while (elapsedTime < seconds)
    //    {
    //        objectToScale.transform.localScale = Vector3.Lerp(startingScale, scaleTo, (elapsedTime / seconds));
    //        elapsedTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
}
