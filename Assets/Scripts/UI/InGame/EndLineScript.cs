using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLineScript : MonoBehaviour
{
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
