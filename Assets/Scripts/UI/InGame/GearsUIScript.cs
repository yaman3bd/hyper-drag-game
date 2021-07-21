using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class GearsUIScript : MonoBehaviour
{
    public TestCarMovement car;
    public TMP_Text SpeedText;
    public TMP_Text TopSeed;
    public TMP_Text GearSeed;

    public TMP_Text GearsText;
    public Button GearUpButton;
    public Button GearDownButton;
    public Button NitroButton;
    public Button Reload;
    public Image NitroFillImage;
    // Start is called before the first frame update
    void Start()
    {
        Reload.onClick.AddListener(() =>
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
        NitroButton.onClick.AddListener(() => {

            car.ActiveBoost();
        });
        GearUpButton.onClick.AddListener(() => ChangeGear(true));
        GearDownButton.onClick.AddListener(() => ChangeGear(false));
        GearsText.text = (car.CurrentGear + 1).ToString();
    }
    private void ChangeGear(bool up)
    {
        car.ChangeGear(up);
        GearsText.text = (car.CurrentGear + 1).ToString();
    }
    // Update is called once per frame
    void Update()
    {
        SpeedText.text = ((int)car.KPH).ToString();
        TopSeed.text = "top speed: " + car.CarMaxSpeedPerGear[car.CarMaxSpeedPerGear.Length - 1];
        GearSeed.text = "gear speed: " + car.CarMaxSpeedPerGear[car.CurrentGear];
        NitroFillImage.fillAmount = car.GetAvailableBoost();

    }
}
