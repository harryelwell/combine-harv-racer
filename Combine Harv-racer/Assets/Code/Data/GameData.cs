using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;
    public float latestRaceTime;
    public float personalBestTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    public bool IsPersonalBest()
    {
        bool pbAchieved = false;

        if(latestRaceTime > personalBestTime)
        {
            pbAchieved = true;
            personalBestTime = latestRaceTime;
        }

        return pbAchieved;
    }
}
