using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RaceTimeUIManager : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI raceTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

        if(raceTime == null)
        {
            raceTime = GameObject.FindGameObjectWithTag("RaceTimeUI").GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        raceTime.text = FormatTime(gameManager.raceTimeElapsed);
    }

    string FormatTime(float time)
    {
        int minutes = (int) time / 60 ;
        int seconds = (int) time - 60 * minutes;
        int milliseconds = (int) (100 * (time - minutes * 60 - seconds));
        
        return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds );
    }
}
