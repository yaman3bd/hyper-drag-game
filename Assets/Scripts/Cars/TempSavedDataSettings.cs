using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TempSavedDataSettings
{
    public static void SaveCarID(string id)
    {
        PlayerPrefs.SetString("CarID", id);
    }
    public static string GetCarID()
    {
        return PlayerPrefs.GetString("CarID");
    }
    public static void SaveBestTime(int val)
    {
        PlayerPrefs.SetInt("BestTime", val);
    }
    public static int GetBestTime()
    {
        return PlayerPrefs.GetInt("BestTime");
    }

    public static bool IsNewBestTime(int val, bool updateBestTime)
    {
        if (val < GetBestTime())
        {
            if (updateBestTime)
            {
                SaveBestTime(val);
            }
            return true;
        }
        return false;
    }
    public static void TutorialPlayed()
    {
        PlayerPrefs.SetInt("TutorialPlayed", 1);
    }
    public static bool IsTutorialPlayed()
    {
        return PlayerPrefs.HasKey("TutorialPlayed") && PlayerPrefs.GetInt("TutorialPlayed") == 1;
    }
}
