using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoad : MonoBehaviour
{
    public GameObject[] Roads;
    public Transform Player;
    public Vector3 NextPosition;
    public float zPos;
    public float RecycleOffest;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Roads.Length; i++)
        {
            Recycle();
        }
    }
    private void Recycle()
    {

        GameObject roadPiece = Roads[index];

        roadPiece.transform.position = NextPosition;

        NextPosition.z += zPos;
        index = (index + 1) % Roads.Length;
    }
    // Update is called once per frame
    void Update()
    {
        if (Roads[index].transform.localPosition.z + RecycleOffest < Player.transform.position.z - RecycleOffest)
            Recycle();
    }
}
