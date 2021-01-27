using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuUIScript : MonoBehaviour
{
    public Button PlayButton;
    public Button GarageButton;

    public GameObject GarageUI;
    public GameObject MainMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        PlayButton.onClick.AddListener(PlayButton_OnClick);
        GarageButton.onClick.AddListener(GarageButton_OnClick);

    }

    private void GarageButton_OnClick()
    {
        GarageUI.SetActive(true);
        MainMenuUI.SetActive(false);
    }

    private void PlayButton_OnClick()
    {
        SceneManager.LoadScene("Level_1");
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
