using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagment;
using UnityEngine.UI;
using TMPro;
using System;
public class LoadedLevelManager : MonoBehaviour
{
    public static LoadedLevelManager Instance;
    public Action OnRaceStarted;
    public Action OnRaceEnded;

    [HideInInspector]
    public PlayerCarController Player;
    [HideInInspector] 
    public AICarController AI;
    [HideInInspector]
    public bool DidWin;
    public CounterAnimation Counter;


    public float TimeToWaitToStartGame;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        DidWin = false;
        var playerCar = Instantiate(Resources.Load<GameObject>("InGameCars/" + GameManager.Instance.SelectedCarID));


        Player = playerCar.AddComponent<PlayerCarController>();
        Player.PCarController = Player.GetComponent<CarController>();
        Player.tag = "Player";
        var aICar = Instantiate(Resources.Load<GameObject>("InGameCars/c_" + UnityEngine.Random.Range(0, 3)));
        AI = aICar.AddComponent<AICarController>();
        AI.PCarController = AI.GetComponent<CarController>();

    }
    IEnumerator CountdownCoroutine()
    {
        Counter.Animation("3");
        yield return new WaitForSeconds(TimeToWaitToStartGame);
        Counter.Animation("2");
        yield return new WaitForSeconds(TimeToWaitToStartGame);
        Counter.Animation("1");
        yield return new WaitForSeconds(TimeToWaitToStartGame);
        Counter.End();

        StartRace();
    }
    private void StartRace()
    {
        if (OnRaceStarted != null)
        {
            OnRaceStarted();
        }
    }
    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }
    public void EndRace(bool winner)
    {
        DidWin = winner;
        if (OnRaceEnded != null)
        {
            OnRaceEnded();
        }
    }
}
