using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLineScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.parent.CompareTag("Player"))
        {
            LoadedLevelManager.Instance.AI.ToogleHandbrake(true);
            return;
        }

        bool winner = false;
        if (LoadedLevelManager.Instance.Player.transform.position.z > LoadedLevelManager.Instance.AI.transform.position.z)
        {
            winner = true;
        }
        LoadedLevelManager.Instance.EndRace(winner);
    }
}
