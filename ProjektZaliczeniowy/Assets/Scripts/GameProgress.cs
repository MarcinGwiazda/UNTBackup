using UnityEngine;

public static class GameProgress
{
    // ACHIEVEMENTS
    public static bool Track1Finished
    {
        get => PlayerPrefs.GetInt("Track1Finished", 0) == 1;
        set => PlayerPrefs.SetInt("Track1Finished", value ? 1 : 0);
    }

    public static bool Track1FastTime
    {
        get => PlayerPrefs.GetInt("Track1FastTime", 0) == 1;
        set => PlayerPrefs.SetInt("Track1FastTime", value ? 1 : 0);
    }

    // UNLOCKS
    public static bool Track2Unlocked
    {
        get => PlayerPrefs.GetInt("Track2Unlocked", 0) == 1;
        set => PlayerPrefs.SetInt("Track2Unlocked", value ? 1 : 0);
    }

    public static bool Car2Unlocked
    {
        get => PlayerPrefs.GetInt("Car2Unlocked", 0) == 1;
        set => PlayerPrefs.SetInt("Car2Unlocked", value ? 1 : 0);
    }

    public static void CheckUnlocks()
    {
        if (Track1Finished && Track1FastTime)
        {
            Track2Unlocked = true;
            Car2Unlocked = true;
        }
    }
}