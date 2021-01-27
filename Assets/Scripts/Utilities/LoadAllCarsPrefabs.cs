using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
public class LoadAllCarsPrefabs   
{
    public int i;
    public string  path;
    public int zPos;
    public int xPos;
    private void Load()
    {
        var all = Resources.LoadAll(path); var name = path.Split('/');
        name[1] = name[1].Replace(" Car", "");
        zPos += 7;
        var parent = new GameObject(name[1]);
        parent.transform.position = new Vector3();
        xPos = -10;
        for (int i = 0; i < all.Length; i++)
        {

            var a = PrefabUtility.InstantiatePrefab(all[i] as GameObject, parent.transform) as GameObject;

            a.transform.eulerAngles = new Vector3(0, 25, 0);
            a.transform.position = new Vector3(xPos, 0, zPos);
            xPos += 5;
        }

    }
}
#endif