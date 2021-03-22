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
    public Action OnBeforeRaceStarted;

    public GameObject EndLine;
    [HideInInspector]
    public PlayerCarController Player;
    [HideInInspector] 
    public AICarController AI;
    [HideInInspector]
    public bool DidWin;
    private bool ShouldUpdate;
    public CounterAnimation Counter;


    public float TimeToWaitToStartGame;
    // Start is called before the first frame update
    void Awake()
    {
        AudioManager.Instance.Play("Backgroud");

        Instance = this;
        DidWin = false;
        GameManager.Instance.ScenesManager.UpdateProgress(0.75f);

        var playerCar = Instantiate(Resources.Load<GameObject>("InGameCars/" + GameManager.Instance.SelectedCarData.ID + "/" + GameManager.Instance.SelectedCarData.ID));

        var playerCarBody = Instantiate(Resources.Load<GameObject>("InGameCars/" + GameManager.Instance.SelectedCarData.ID + "/Body/" +
            GameManager.Instance.SelectedCarData.ID + "_body_" + GameManager.Instance.SelectedCarData.GetColorName()));

        playerCarBody.transform.SetParent(playerCar.transform);
        playerCarBody.transform.position = Vector3.zero;
        playerCarBody.transform.SetAsFirstSibling();

        Player = playerCar.AddComponent<PlayerCarController>();
        Player.PCarController = Player.GetComponent<CarController>();
        Player.SetCarToPosition(new Vector3(0, 0.1f, 10));
        Player.tag = "Player";


        var aiCarData = GameManager.Instance.CarsData.GetRandomCar();
        
        var carPath = "InGameCars/" + aiCarData.ID + "/" + aiCarData.ID;
        var aICar = Instantiate(Resources.Load<GameObject>(carPath));

        var carBodyPath = "InGameCars/" + aiCarData.ID + "/Body/" + aiCarData.ID + "_body_" + aiCarData.GetRandomColorName();

        var aICarBody = Instantiate(Resources.Load<GameObject>(carBodyPath));

        aICarBody.transform.SetParent(aICar.transform);
        aICarBody.transform.position = Vector3.zero;
        aICarBody.transform.SetAsFirstSibling();
        AI = aICar.AddComponent<AICarController>();
        AI.PCarController = AI.GetComponent<CarController>();
        AI.SetCarToPosition(new Vector3(0, 0.1f, 10));
       
        GameManager.Instance.ScenesManager.UpdateProgress(1f);

    }
    private void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }
    private IEnumerator CountdownCoroutine()
    {
        yield return new WaitUntil(() => GameManager.Instance.ScenesManager.LoadDone);
        yield return new WaitForSeconds(TimeToWaitToStartGame);
        Counter.Animation("3");
        yield return new WaitForSeconds(TimeToWaitToStartGame);
        Counter.Animation("2");
        BeforeRaceStarted();
        yield return new WaitForSeconds(TimeToWaitToStartGame);
        Counter.Animation("1");
        yield return new WaitForSeconds(TimeToWaitToStartGame);
        Counter.End();

        StartRace();
    }
    private void StartRace()
    {
        ShouldUpdate = true;
        if (OnRaceStarted != null)
        {
            OnRaceStarted();
        }
    }
    private void BeforeRaceStarted()
    {
        if (OnBeforeRaceStarted != null)
        {
            OnBeforeRaceStarted();
        }
    }

    public void EndRace(bool winner)
    {
        DidWin = winner;
        ShouldUpdate = false;

        if (OnRaceEnded != null)
        {
            OnRaceEnded();
        }
    }
    public float PlayerRaceTime
    {
        get;
        private set;
    }
    private void Update()
    {
         
        if (!ShouldUpdate)
        {
         /*   Player.SetCarToPosition(new Vector3(0,0.1f,0));
            AI.SetCarToPosition(new Vector3(0, 0.1f, 0));*/
            return;
        }
        PlayerRaceTime += Time.deltaTime;
    }
   
}
