using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
public class RoadGenerator : MonoBehaviour
{
    [Header("Straight Roads")]
    public int MinStraightRoadCount;
    public int MaxStraightRoadCount;
    [Header("Wave Roads")]
    public int MinWaveRaodsCount;
    public int MaxWaveRaodsCount;

    public List<GameObject> Roads;
    public List<GameObject> Props;

    public Vector3 StartPosition;
    public GameObject StraightRoad;
    public GameObject EndRoad;
    public GameObject EndRoadCol;

    public GameObject PrevStraightRoad;
    public GameObject CurrentStraightRoad;
    public int currentLoop;
    public int loops;
    Transform _Parent;
    public Vector3 lastPos;
    public Vector3 EndLinePos;
    public string path;
    private string[] chars = { "a", "b", "r", "s", "x" };

#if UNITY_EDITOR
    public void SearchInAllPrefabs()
    {

        var path = "Assets/Resources/GarageCars";

        DirectoryInfo dirInfo = new DirectoryInfo(path);
        FileInfo[] fileInf = dirInfo.GetFiles("*.*");

        foreach (FileInfo fileInfo in fileInf)
        {

            var name = fileInfo.Name;
            name = name.Replace(".meta", "");
            path = "Assets/Resources/GarageCars/" + name;

            DirectoryInfo dirs = new DirectoryInfo(path);
            FileInfo[] infos = dirs.GetFiles("*.prefab*");




            foreach (var item in infos)
            {
                if (!item.Name.Contains(".prefab.meta"))
                {
                    string fullPath = item.FullName.Replace(@"\", "/");
                    string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "");

                    GameObject prefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                    if (prefab.GetComponent<SyncWheelsValues>() == null)
                    {

                        prefab.AddComponent<SyncWheelsValues>();
                    }
                }
            }
        }
    }
    public void InitializeCarsData()
    {

        var path = "Assets/Resources/InGameCars";

        DirectoryInfo carsFolders = new DirectoryInfo(path);
        FileInfo[] carsFoldersInfo = carsFolders.GetFiles("*.*");

        foreach (var carsFolderInfo in carsFoldersInfo)
        {
            var name = carsFolderInfo.Name;
            name = name.Replace(".meta", "");
            path = "Assets/Resources/InGameCars/" + name;

            DirectoryInfo dirs = new DirectoryInfo(path);
            FileInfo[] infos = dirs.GetFiles("*.prefab*");
            foreach (var item in infos)
            {
                if (!item.Name.Contains(".prefab.meta"))
                {


                    Debug.LogError(item.Name);
                }
            }
            //Debug.LogError(carsFolderInfo.Name);
        }



    }
#endif
    private void Start()
    {


        GenerateRoad();
        _Parent.transform.position = new Vector3(15, 0, 0);
        Instantiate(_Parent, new Vector3(5, 0, 0), Quaternion.identity);
    }
    public void GenerateRoad()
    {


        Transform Parent = new GameObject("Parent").transform;
        _Parent = Parent;
        currentLoop = 0;

        while (currentLoop < loops)
        {

            GenerateStraightRoads(Parent);

            GenerateWaveRoads(Parent);


            currentLoop++;
        }
        GenerateStraightRoads(Parent);
        GenerateEndLine(Parent);
        lastPos = CurrentStraightRoad.transform.position;
        GenerateEndLineCol();
    }
    private void GenerateEndLineCol()
    {
        var col = Instantiate(EndRoadCol, Vector3.zero, Quaternion.identity);
        LoadedLevelManager.Instance.EndLine = CurrentStraightRoad;

        col.name = "EndLineCol";
        var pos = lastPos;
        pos.x = 0;
        pos.z += 25;
        col.transform.position = pos;

    }
    private void GenerateEndLine(Transform parent)
    {

        CurrentStraightRoad = Instantiate(EndRoad, Vector3.zero, Quaternion.identity, parent);
        CurrentStraightRoad.transform.eulerAngles = new Vector3(0, -90, 0);

        CurrentStraightRoad.name = "EndLines";

        if (PrevStraightRoad != null)
        {
            CurrentStraightRoad.transform.position = PrevStraightRoad.transform.GetChild(0).position;
        }
        else
        {
            CurrentStraightRoad.transform.position = StartPosition;
        }
        EndLinePos = CurrentStraightRoad.transform.position;
        PrevStraightRoad = CurrentStraightRoad;
    }

    private void GenerateWaveRoads(Transform parent)
    {

        int waveRaodsCount = UnityEngine.Random.Range(MinWaveRaodsCount, MaxWaveRaodsCount + 1);
        Roads.Shuffle();
        for (int x = 0; x < waveRaodsCount; x++)
        {

            int j = UnityEngine.Random.Range(0, Roads.Count);
            var road = Instantiate(Roads[j], Vector3.zero, Quaternion.identity, parent);
            road.transform.eulerAngles = new Vector3(0, -90, 0);

            road.transform.position = PrevStraightRoad.transform.GetChild(0).position;
            PrevStraightRoad = road;
        }

    }

    private void GenerateStraightRoads(Transform parent)
    {
        int straightRoadCount = UnityEngine.Random.Range(MinStraightRoadCount, MaxStraightRoadCount + 1);

        for (int i = 0; i < straightRoadCount; i++)
        {

            CurrentStraightRoad = Instantiate(StraightRoad, Vector3.zero, Quaternion.identity, parent);

            CurrentStraightRoad.transform.eulerAngles = new Vector3(0, -90, 0);

            CurrentStraightRoad.name = "raod_" + i;

            if (PrevStraightRoad != null)
            {
                CurrentStraightRoad.transform.position = PrevStraightRoad.transform.GetChild(0).position;
            }
            else
            {
                CurrentStraightRoad.transform.position = StartPosition;
            }

            PrevStraightRoad = CurrentStraightRoad;

            if (currentLoop != 0 && (i == (straightRoadCount / 2)))
            {
                Props.Shuffle();

                int j = UnityEngine.Random.Range(0, Props.Count);

                var prop = Instantiate(Props[j], Vector3.zero, Quaternion.identity, parent);
                var propPos = new Vector3(PrevStraightRoad.transform.position.x, PrevStraightRoad.transform.position.y + 1, PrevStraightRoad.transform.position.z);

                prop.transform.position = propPos;

            }

        }
    }
}
