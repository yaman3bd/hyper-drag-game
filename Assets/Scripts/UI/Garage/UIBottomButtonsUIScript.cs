﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameManagment;
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
    public RectTransform Parent;

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

        string id = TempSavedDataSettings.GetCarID();

        if (string.IsNullOrEmpty(id))
        {
            id = GameManager.Instance.DefaultCarID;
        }
        GarageUI.ShowCar(id);

        StartCoroutine(PlaceUIItems());
    }

    IEnumerator PlaceUIItems()
    {
        SetWidthToMainUILayoutElement();
        GameManager.Instance.ScenesManager.UpdateProgress(0.75f);
        yield return new WaitForEndOfFrame();
        MainUIButton_OnClick();
        GameManager.Instance.ScenesManager.UpdateProgress(1f);

    }
    private void SettingsUIButton_OnClick()
    {
        UIMenuItemAnimation(SettingsUI.UIRect, SettingsUIButtonRect);
    }

    private void GarageUIButton_OnClick()
    {
        GameManager.Instance.BackButton.BackButtonCallBack = () =>
        {
            MainUIButton_OnClick();
        };
        GarageUI.Show();
        UIMenuItemAnimation(GarageUI.UIRect, GarageUIButtonRect);
    }

    public void MainUIButton_OnClick()
    {
        GameManager.Instance.BackButton.BackButtonCallBack = () =>
        {
            Application.Quit();
        };
        //GarageUI.HideCarCustomization();
        UIMenuItemAnimation(MainMenuUI.UIRect, MainUIButtonRect);
    }

    private void StoreUIButton_OnClick()
    {
        UIMenuItemAnimation(StoreUI.UIRect, StoreUIButtonRect);
    }

    private void UIMenuItemAnimation(RectTransform target, RectTransform buttonTarget)
    {
        var pos = MenusScroll.ScrollToCenter(target, RectTransform.Axis.Horizontal);


        MenusScroll.DOHorizontalNormalizedPos(pos, UIItemsScrollDuration);

    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        SetWidthToMainUILayoutElement();
        var pos = MenusScroll.ScrollToCenter(GarageUI.UIRect, RectTransform.Axis.Horizontal);
      
        MenusScroll.horizontalNormalizedPosition = pos;
    }


#endif

    private void SetWidthToMainUILayoutElement()
    {
        var width = Parent.rect.width;
        /* var items = MenusScroll.content.childCount;
         for (int i = 0; i < items; i++)
         {
             var item = MenusScroll.content.GetChild(i);
             if (item.gameObject.activeSelf)
             {
                 item.GetComponent<LayoutElement>().preferredHeight = width;
             }
         }*/

        SettingsUI.LayoutElement.preferredWidth = width;
        GarageUI.LayoutElement.preferredWidth = width;
        MainMenuUI.LayoutElement.preferredWidth = width;
        StoreUI.LayoutElement.preferredWidth = width;
    }
}
