using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
#if UNITY_EDITOR
public class CarsDataScriptableObjectInitializer : MonoBehaviour
{
    private string[] chars = { "a", "b", "r", "s", "x" };
    public void InitializeCarsData()
    {
        var path = "Assets/ARCADE - Mega Racing Cars Pack/Prefabs With Colliders";

        DirectoryInfo carsFolders = new DirectoryInfo(path);
        FileInfo[] carsFoldersInfo = carsFolders.GetFiles("*.*");
        int charsIndex = 0;
        int i = 0;
        foreach (var carsFolderInfo in carsFoldersInfo)
        {

            path = "Assets/ARCADE - Mega Racing Cars Pack/Prefabs With Colliders/" + chars[charsIndex].ToUpper() + " Class";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] info = dir.GetFiles("*.*");

            foreach (FileInfo o in info)
            {
                CarDataScriptableObject car = ScriptableObject.CreateInstance<CarDataScriptableObject>();

                var name = o.Name;
                name = name.Replace(".meta", "");
                car.Name = name;
                var className = carsFolderInfo.Name;

                car.ClassName = className.Replace(".meta", "");
                car.ID = "c_" + i++;

                try
                {

                    path = "Assets/ARCADE - Mega Racing Cars Pack/Prefabs With Colliders/" + chars[charsIndex].ToUpper() + " Class/" + name;

                    DirectoryInfo dirs = new DirectoryInfo(path);
                    FileInfo[] infos = dirs.GetFiles("*.*");
                    car.ColorsNames = new List<string>();

                    foreach (var item in infos)
                    {
                        if (!item.Name.Contains(".meta"))
                        {

                            var itemname = item.Name;
                            itemname = itemname.Replace(".prefab", "");
                            var vals = itemname.Split('_');
                            car.ColorsNames.Add(vals[1]);


                        }

                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);

                }


                string psath = "Assets/Scripts/Cars/ScriptableObjects/Cars/" + car.ID + ".asset";
                AssetDatabase.CreateAsset(car, psath);


                AssetDatabase.SaveAssets();

            }

            charsIndex++;
        }


        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();

    }
}
#endif