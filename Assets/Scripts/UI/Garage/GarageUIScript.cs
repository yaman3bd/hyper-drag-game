using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using GameManagment;
public class GarageUIScript : GlobalUIScript
{
    public GameObject MainUI;
    public RectTransform UIRect;
    public LayoutElement LayoutElement;

    [Header("Cars")]
    public GarageCarsLoader CarsLoader;
    public Vector3 CarPosition;
    public Vector3 CarRotation;
    public Transform CarsParent;

    [Header("Car Name and Type")]
    public TMP_Text CarNameText;
    public TMP_Text CarRankText;

    [Header("Next And Prev Buttons")]
    public Button NextCarButton;
    public Button PrevCarButton;

    [Header("Buy Buttons")]
    public Button BuyCarButton;
    public TMP_Text BuyCarButtonText;

    [Header("Car Specifications")]
    public Slider TopSpeedSlider;
    public Slider GearsSliders;

    public int CarIndex;
    public string SelectedCarID;
    // Start is called before the first frame update
    void Start()
    {
        NextCarButton.onClick.AddListener(NextCarButton_OnClick);
        PrevCarButton.onClick.AddListener(PrevCarButton_OnClick);
        BuyCarButton.onClick.AddListener(BuyCarButton_OnClick);      
    }
    public override void Show()
    {
        base.Show();
        CarIndex = -1;

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
            CarIndex = GameManager.Instance.CarsData.CarsDataList.Count - 1;
        }

        UpdateGarageUIandGarage(CarIndex);
       
    }

    private void NextCarButton_OnClick()
    {
     
        // Moves to the next spawn point index. If it goes out of range, it wraps back to the start.
        CarIndex = (CarIndex + 1) % GameManager.Instance.CarsData.CarsDataList.Count;

        UpdateGarageUIandGarage(CarIndex);

    }
    private void UpdateUI(CarDataScriptableObject carData)
    {
        CarNameText.text = carData.Name;
        CarRankText.text = carData.GetRankName();

        TopSpeedSlider.value = UnityEngine.Random.Range(0.5f, 1.0f);
        GearsSliders.value = UnityEngine.Random.Range(0.5f, 1.0f);
    }
    private void UpdateCar(CarDataScriptableObject carData)
    {

        if (activeCar != null)
        {
            activeCar.Model.SetActive(false);
        }
        
        activeCar = CarsLoader.GetCar(carData.ID);
        PlaceCar(activeCar.Model, CarPosition, CarRotation, CarsParent);
        activeCar.Model.SetActive(true);

    }
    private void UpdateGarageUIandGarage(int carIndex)

    {
       
        var carData = GameManager.Instance.CarsData.GetCarByIndex(carIndex);
        
        UpdateUI(carData);
        
        UpdateCar(carData);

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
        SelectedCarID = carData.ID;
        GameManager.Instance.SelectedCarID = SelectedCarID;

    }

    private void BuyCarButton_OnClick()
    {
        this.gameObject.SetActive(false);
        MainUI.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
