using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIBottomButtonsUIScript : MonoBehaviour
{
    [Header("Animation")]
    public float UIItemsScrollDuration;
    [Space]
    public Button SettingsUIButton;
    public Button GarageUIButton;
    public Button MainUIButton;
    public Button StoreUIButton;
    [Space]
    public RectTransform SettingsUIButtonRect;
    public RectTransform GarageUIButtonRect;
    public RectTransform MainUIButtonRect;
    public RectTransform StoreUIButtonRect;
 
    [Space]
    public ScrollRect MenusScroll;
    [Space]
    public Canvas Parent;

    [Header("Menu UI Items")]  
    public SettingsUIScript SettingsUI;
    public GarageUIScript GarageUI; 
    public MainMenuUIScript MainMenuUI;
    public StoreUIScript StoreUI;

    // Start is called before the first frame update
    void Start()
    {

        SettingsUIButton.onClick.AddListener(SettingsUIButton_OnClick);
        GarageUIButton.onClick.AddListener(GarageUIButton_OnClick);
        MainUIButton.onClick.AddListener(MainUIButton_OnClick);
        StoreUIButton.onClick.AddListener(StoreUIButton_OnClick);

    }

    private void SettingsUIButton_OnClick()
    {
        UIMenuItemAnimation(SettingsUI.UIRect, SettingsUIButtonRect);
    }

    private void GarageUIButton_OnClick()
    {
        UIMenuItemAnimation(GarageUI.UIRect, GarageUIButtonRect);
    }

    private void MainUIButton_OnClick()
    {
        UIMenuItemAnimation(MainMenuUI.UIRect, MainUIButtonRect);
    }
    
    private void StoreUIButton_OnClick()
    {
        UIMenuItemAnimation(StoreUI.UIRect,StoreUIButtonRect);
    }

    private void UIMenuItemAnimation(RectTransform target,RectTransform buttonTarget)
    {
        var pos = MenusScroll.ScrollToCenter(target, RectTransform.Axis.Horizontal);
        
    
        MenusScroll.DOHorizontalNormalizedPos(pos, UIItemsScrollDuration);
    }
    public RectTransform rect;
    public RectTransform center;
#if UNITY_EDITOR
    private void OnValidate()
    {
     
    }
#endif
    private void PlaceUI()
    {
        int multi = 3;

        var a = Parent.GetComponent<RectTransform>().rect.width;
        SettingsUI.GetComponent<RectTransform>().SetLeft(a * -multi);

        SettingsUI.GetComponent<RectTransform>().SetRight(a * multi);
        multi--;
        GarageUI.GetComponent<RectTransform>().SetLeft(a * -multi);
        GarageUI.GetComponent<RectTransform>().SetRight(a * multi);

        multi--;
        MainMenuUI.GetComponent<RectTransform>().SetLeft(a * -multi);
        MainMenuUI.GetComponent<RectTransform>().SetRight(a * multi);
        multi--;
        StoreUI.GetComponent<RectTransform>().SetLeft(a * -multi);
        StoreUI.GetComponent<RectTransform>().SetRight(a * multi);


    }
}
