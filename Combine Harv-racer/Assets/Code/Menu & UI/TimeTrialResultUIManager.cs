using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeTrialResultUIManager : MonoBehaviour
{
    public GameData gameData;
    public TextMeshProUGUI raceTime;
    public TextMeshProUGUI pbText;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gameData == null)
        {
            gameData = GameObject.FindGameObjectWithTag("GameData").GetComponent<GameData>();
        }

        if(raceTime == null)
        {
            raceTime = GameObject.FindGameObjectWithTag("RaceTimeUI").GetComponent<TextMeshProUGUI>();
        }

        raceTime.text = FormatTime(gameData.latestRaceTime);

        if(gameData.IsPersonalBest())
        {
            // SHow the New Personal Best Sign
            pbText.color = new Color(pbText.color.r,pbText.color.g,pbText.color.b,1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    string FormatTime(float time)
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time - 60 * minutes;
        int milliseconds = (int) (100 * (time - minutes * 60 - seconds));
        
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds );
    }
}
