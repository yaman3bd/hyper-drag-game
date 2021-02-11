using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class MainMenuUIScript : GlobalUIScript
{
    public RectTransform UIRect;
    public LayoutElement LayoutElement;

    public Button RaceButton;

    // Start is called before the first frame update
    void Start()
    {
        RaceButton.onClick.AddListener(PlayButton_OnClick);

    }
    public override void Show()
    {
        base.Show();
    }
    public override void Hide()
    {
        base.Hide();
    }

    private void PlayButton_OnClick()
    {
        GameManagment.GameManager.Instance.ScenesManager.LoadScene("InGame");
    }
}
