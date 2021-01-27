using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;
namespace GameManagment
{
    public class GameManager : Singleton<GameManager>
    {
        public CarsScriptableObject CarsData;
        public CarsIDsScriptableObject CarsIds;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}