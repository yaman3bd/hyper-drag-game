using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIBottomButtonsUIScript : MonoBehaviour
{
    public GameObject[] cars;
    public RectTransform TestT;
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

        StartCoroutine(Test());
    }
    IEnumerator Test()
    {
        SetWidthToMainUILayoutElement();

        yield return new WaitForEndOfFrame();
        GarageUIButton_OnClick();
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
        UIMenuItemAnimation(StoreUI.UIRect, StoreUIButtonRect);
    }

    private void UIMenuItemAnimation(RectTransform target, RectTransform buttonTarget)
    {
        var pos = MenusScroll.ScrollToCenter(target, RectTransform.Axis.Horizontal);


        MenusScroll.DOHorizontalNormalizedPos(pos, UIItemsScrollDuration);
     
    }
    public bool test = false;
#if UNITY_EDITOR
    private void OnValidate()
    {
        SetWidthToMainUILayoutElement();
        var pos = MenusScroll.ScrollToCenter(TestT, RectTransform.Axis.Horizontal);
        MenusScroll.horizontalNormalizedPosition = pos;
        #region Test
        /*  if (!test)
         {
             foreach (var item in cars)
             {
                     var CenterOfMass = item.transform.Find("CenterOfMass");
                     var Body= item.transform.Find("Body").GetComponent<Renderer>();
                     Vector3 center = Body.bounds.center;
                     CenterOfMass.transform.localPosition = new Vector3(0, center.y / 2, 0);
                      var FrontWheels = new GameObject("FrontWheels");

                       FrontWheels.transform.SetParent(item.transform);

                       FrontWheels.transform.position = Vector3.zero;
                       FrontWheels.transform.rotation = Quaternion.identity;

                       FrontWheels.transform.localPosition = Vector3.zero;
                       FrontWheels.transform.localRotation = Quaternion.identity;

                       var RearWheels = new GameObject("RearWheels");

                       RearWheels.transform.SetParent(item.transform);

                       RearWheels.transform.position = Vector3.zero;
                       RearWheels.transform.rotation = Quaternion.identity;

                       RearWheels.transform.localPosition = Vector3.zero;
                       RearWheels.transform.localRotation = Quaternion.identity;

                       var wheels = item.transform.Find("Wheels");


                       var Meshes = wheels.transform.Find("Meshes");

                       var FrontLeftWheelMesh = Meshes.transform.Find("FrontLeftWheel");
                       var FrontRightWheelMesh = Meshes.transform.Find("FrontRightWheel");
                       var RearLeftWheelMesh = Meshes.transform.Find("RearLeftWheel");
                       var RearRightWheelMesh = Meshes.transform.Find("RearRightWheel");


                       var Colliders = wheels.transform.Find("Colliders");

                       var FrontLeftWheelCollider = Colliders.transform.Find("FrontLeftWheel");
                       FrontLeftWheelMesh.SetParent(FrontLeftWheelCollider);

                       var FrontRightWheelCollider = Colliders.transform.Find("FrontRightWheel");
                       FrontRightWheelMesh.SetParent(FrontRightWheelCollider);

                       FrontLeftWheelCollider.SetParent(FrontWheels.transform);
                       FrontRightWheelCollider.SetParent(FrontWheels.transform);


                       var RearLeftWheelCollider = Colliders.transform.Find("RearLeftWheel");
                       RearLeftWheelMesh.SetParent(RearLeftWheelCollider);

                       var RearRightWheelCollider = Colliders.transform.Find("RearRightWheel");
                       RearRightWheelMesh.SetParent(RearRightWheelCollider);

                       RearLeftWheelCollider.SetParent(RearWheels.transform);
                       RearRightWheelCollider.SetParent(RearWheels.transform);

                       DestroyImmediate(wheels.gameObject);
             }
         }
         test = true;*/
        #endregion
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
