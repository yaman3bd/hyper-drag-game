using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagment;
public class LoadedLevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var playerCar = Instantiate(Resources.Load<GameObject>(GameManager.Instance.SelectedCarID));
      
        var pcc = playerCar.AddComponent<PlayerCarController>();
        pcc.PCarController = playerCar.GetComponent<CarController>();

        var aICar = Instantiate(Resources.Load<GameObject>("car_2"));
        var aIcc = aICar.AddComponent<AICarController>();
        aIcc.PCarController = aIcc.GetComponent<CarController>();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
