using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using GameManagment;
public class GarageUIScript : GlobalUIScript
{
    public UIBottomButtonsUIScript UIBottomButtons;
    public GameObject MainUI;
    public RectTransform UIRect;
    public LayoutElement LayoutElement;

    [Header("Cars")]
    public GarageCarsLoader CarsLoader;
    public Vector3 CarPosition;
    public Vector3 CarPosition_AfterColor;

    public Vector3 CarRotation;
    public Transform CarsParent;

    [Header("Car Name and Type")]
    public TMP_Text CarNameText;
    public TMP_Text CarRankText;

    [Header("Next And Prev Buttons")]
    public Button NextCarButton;
    public Button PrevCarButton;
    public Button RaceButton;

    [Header("Buy Buttons")]
    public Button BuyCarButton;
    public TMP_Text BuyCarButtonText;

    [Header("Car Specifications")]
    public Slider TopSpeedSlider;
    public Slider GearsSliders;
    [Header("Colors")]
    public GameObject ColorImageTemp;
     
    public RectTransform ColorImagesParent;
    public int CarIndex;
    [Header("CarCustomization")]
    public Button CustomizeCarButton;
    public GameObject BuyButtons;
    public GameObject CarCustomization;
    public GameObject CarWheels;
    public GameObject CarColors;
    public Button CustomizeWheelsButton;
    public Button CustomizeColorsButton;
    public RectTransform CustomizeButtonMaskImg;
    public CarDataScriptableObject SelectedCarData;
    // Start is called before the first frame update
    void Start()
    {
        NextCarButton.onClick.AddListener(NextCarButton_OnClick);
        PrevCarButton.onClick.AddListener(PrevCarButton_OnClick);
        BuyCarButton.onClick.AddListener(BuyCarButton_OnClick);
        RaceButton.onClick.AddListener(PlayButton_OnClick);
        CustomizeCarButton.onClick.AddListener(CustomizeCarButton_OnClick);

        CustomizeColorsButton.onClick.AddListener(CustomizeColorsButton_OnClick);
        CustomizeWheelsButton.onClick.AddListener(CustomizeWheelsButton_OnClick);

    }

    private void CustomizeCarButton_OnClick()
    {
        GameManager.Instance.BackButton.BackButtonCallBack = () =>
        {
            HideCarCustomization();
        };

        //CustomizeCarButton.gameObject.SetActive(false);
        
        //CarCustomization.SetActive(true);
       // BuyButtons.SetActive(false);
    }

    private void CustomizeWheelsButton_OnClick()
    {
        CustomizeButtonMaskImg.SetParent(CustomizeWheelsButton.transform);
        CustomizeButtonMaskImg.SetAsFirstSibling();
        CustomizeButtonMaskImg.SetAllZero();
        CarWheels.SetActive(true);
        CarColors.SetActive(false);
    }

    private void CustomizeColorsButton_OnClick()
    {
        CustomizeButtonMaskImg.SetParent(CustomizeColorsButton.transform);
        CustomizeButtonMaskImg.SetAsFirstSibling();
        CustomizeButtonMaskImg.SetAllZero();
        CarWheels.SetActive(false);
        CarColors.SetActive(true);
    }
    public void HideCarCustomization()
    {
        GameManager.Instance.BackButton.BackButtonCallBack = () =>
        {
            UIBottomButtons.MainUIButton_OnClick();
        };
        CustomizeCarButton.gameObject.SetActive(true);
        //BuyButtons.SetActive(true);
        CarCustomization.SetActive(false);
    }
    public override void Show()
    {
        base.Show();
        CarIndex = GameManager.Instance.CarsData.GetCarIndex(SelectedCarData.ID);
        //CustomizeColorsButton_OnClick();
        //HideCarCustomization();
        InitColors(SelectedCarData);
    }
    public override void Hide()
    {
        base.Hide();
    }
    GarageCarData activeCar;
    private void PrevCarButton_OnClick()
    {

        CarIndex--;

        if (CarIndex < 0)
        {
            CarIndex = GameManager.Instance.CarsData.FilteredCarsDataList.Count - 1;
        }

        UpdateGarageUIandGarage(CarIndex);

    }

    private void NextCarButton_OnClick()
    {

        // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
        CarIndex = (CarIndex + 1) % GameManager.Instance.CarsData.FilteredCarsDataList.Count;

        UpdateGarageUIandGarage(CarIndex);

    }
    private void UpdateUI(CarDataScriptableObject carData)
    {
        CarNameText.text = carData.Name;
        CarRankText.text = carData.GetRankName();

        TopSpeedSlider.value = UnityEngine.Random.Range(0.5f, 1.0f);
        GearsSliders.value = UnityEngine.Random.Range(0.5f, 1.0f);
    }
    private void UpdateCar(CarDataScriptableObject carData,Vector3 CarPosition)
    {

        if (activeCar != null)
        {
            activeCar.Model.SetActive(false);
        }

        activeCar = CarsLoader.GetCar(carData.ID, carData.ID + "_" + carData.GetColorName());
        PlaceCar(activeCar.Model, CarPosition, CarRotation, CarsParent);
        activeCar.Model.SetActive(true);
        activeCar.Model.GetComponent<SyncWheelsValues>().SyncValues();

    }
    private void UpdateGarageUIandGarage(int carIndex)
    {

        var carData = GameManager.Instance.CarsData.GetCarByIndex(carIndex);
        //HideCarCustomization();

     
        UpdateUI(carData);

        UpdateCar(carData, CarPosition);

        UpdateCarID(carData);
        InitColors(SelectedCarData);
    }
    private void InitColors(CarDataScriptableObject carData)
    {
        for (int i = 0; i < ColorImagesParent.childCount; i++)
        {
            Destroy(ColorImagesParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < carData.ColorsNames.Count; i++)
        {
            var colorName = carData.ColorsNames[i];
            var img = Instantiate(ColorImageTemp, ColorImagesParent);
            var btn = img.gameObject.AddComponent<Button>();
            int j = i;
            btn.onClick.AddListener(() =>
            {
                TempSavedDataSettings.SaveCarColorName(carData.ID, carData.ColorsNames[j]);
                UpdateCar(carData, CarPosition_AfterColor);
            });
            img.SetActive(true);
            img.transform.GetChild(0).gameObject.GetComponent<Image>().color = carData.Colors[i];
            img.name = colorName;
        }
    }
    private void UpdateGarageUIandGarage(string carID)
    {

        var carData = GameManager.Instance.CarsData.GetCarByID(carID);

        UpdateUI(carData);

        UpdateCar(carData, CarPosition);

        UpdateCarID(carData);


    }
    private void PlaceCar(GameObject car, Vector3 postion, Vector3 rotation, Transform parent)
    {
        car.transform.localPosition = postion;
        car.transform.localEulerAngles = rotation;
        car.transform.SetParent(parent);

    }
    private void UpdateCarID(CarDataScriptableObject carData)
    {
        SelectedCarData = carData;

        TempSavedDataSettings.SaveCarID(SelectedCarData.ID);
        TempSavedDataSettings.SaveCarColorName(SelectedCarData.ID, SelectedCarData.GetColorName());

    }
    public void ShowCar(string id)
    {

        UpdateGarageUIandGarage(id);
    }

    private void BuyCarButton_OnClick()
    {
        //   this.gameObject.SetActive(false);
        //  MainUI.SetActive(true);
    }
    private void PlayButton_OnClick()
    {
        GameManagment.GameManager.Instance.ScenesManager.LoadScene("InGame");
    }

}
