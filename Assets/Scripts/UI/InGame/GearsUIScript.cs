using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class GearsUIScript : MonoBehaviour
{ 
    public GameObject Pointer;
    public TMP_Text SpeedText;
    public TMP_Text GearsText;
    public Button GearUpButton;
    public Button GearDownButton;

    // Start is called before the first frame update
    void Start()
    {
        GearUpButton.onClick.AddListener(() => ChangeGear(true));
        GearDownButton.onClick.AddListener(() => ChangeGear(false));
        GearsText.text = "N";
    }
    private void ChangeGear(bool up)
    {
        LoadedLevelManager.Instance.Player.ChangeGear(up);
        GearsText.text = (LoadedLevelManager.Instance.Player.Gear).ToString();
    }
    public bool willMove;
    // Update is called once per frame
    void Update()
    {
        SpeedText.text = ((int)LoadedLevelManager.Instance.Player.Speed).ToString();
        if (Pointer.transform.rotation.z <= -0.4f && LoadedLevelManager.Instance.Player.CanGear())
        {
            GearUpButton.gameObject.SetActive(true);
            GearDownButton.gameObject.SetActive(true);
        }
        else
        {
            GearUpButton.gameObject.SetActive(false);
            GearDownButton.gameObject.SetActive(false);
        } 
    }
}
