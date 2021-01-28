using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIBottomButtonsUIScript : MonoBehaviour
{
    public Button MainUIButton;
    public Button GarageUIButton; 

    public GarageUIScript GarageUI;
    public MainMenuUIScript MainMenuUI; 

    public ScrollRect MenusScroll;

    public RectTransform Content;

    // Start is called before the first frame update
    void Start()
    {
      
        MainUIButton.onClick.AddListener(MainUIButton_OnClick);
        GarageUIButton.onClick.AddListener(GarageUIButton_OnClick);
          
    }


    private void GarageUIButton_OnClick()
    {
        UIMenuItemAnimation(GarageUI.UIRect);
    }

    private void MainUIButton_OnClick()
    {
        UIMenuItemAnimation(MainMenuUI.UIRect);

    }
    private void UIMenuItemAnimation(RectTransform target)
    {
        var pos = MenusScroll.ScrollToCenter(target, RectTransform.Axis.Horizontal);
        MenusScroll.DOHorizontalNormalizedPos(pos, d);
    }
    public Vector2 v;
    public float d;

}
