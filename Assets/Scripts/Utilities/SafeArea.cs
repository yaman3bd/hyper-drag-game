
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
 

public enum VerticalAxis
{
    None,
    Top,
    Bottom,
    Top_Bottom
}
public enum HorizontalAxis
{
    None,
    Left,
    Right,
    Left_Right
}
[System.Serializable]
public class ApplySafeAreaIn
{

    [Header("Y Axis")]
    public VerticalAxis VerticalAxis;
    [Header("X Axis")]
    public HorizontalAxis HorizontalAxis;

    public bool AllAxises;
}
 public class SafeArea : MonoBehaviour
{
    public ApplySafeAreaIn ApplySafeAreaIn;

     private Canvas canvas;
    private RectTransform safeAreaTransform;

    void Awake()
    {

        canvas = GetComponentInParent<Canvas>();

        safeAreaTransform = GetComponent<RectTransform>();

        ApplySafeArea();
    }
   

    void ApplySafeArea()
    {
        if (safeAreaTransform == null)
            return;

        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;


        if (ApplySafeAreaIn.AllAxises)
        {
            //right
            anchorMax.x /= canvas.pixelRect.width;
            //top
            anchorMax.y /= canvas.pixelRect.height;
            //bottom
            anchorMin.y /= canvas.pixelRect.height;
            //left
            anchorMin.x /= canvas.pixelRect.width;
        }
        else
        {
            switch (ApplySafeAreaIn.VerticalAxis)
            {

                case VerticalAxis.Top:
                    //top
                    anchorMax.y /= canvas.pixelRect.height;
                    //right
                    anchorMax.x = 1;
                    //bottom
                    anchorMin.y = 0;
                    //left     
                    anchorMin.x = 0;
                    break;
                case VerticalAxis.Bottom:
                    //bottom
                    anchorMin.y /= canvas.pixelRect.height;
                    //left     
                    anchorMin.x = 0;
                    //right
                    anchorMax.x = 1;
                    //top
                    anchorMax.y = 1;

                    break;
                case VerticalAxis.Top_Bottom:
                    //top
                    anchorMax.y /= canvas.pixelRect.height;
                    //bottom
                    anchorMin.y /= canvas.pixelRect.height;
                    //left     
                    anchorMin.x = 0;
                    //right
                    anchorMax.x = 1;
                    break;
                default:
                    break;
            }
        }

        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;
    }

}