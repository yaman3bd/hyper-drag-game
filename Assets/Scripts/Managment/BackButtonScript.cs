using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonScript : MonoBehaviour
{
    public delegate void BackButton();
    public BackButton BackButtonCallBack;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (BackButtonCallBack != null)
            {
                BackButtonCallBack();
            }
        }
    }
}
