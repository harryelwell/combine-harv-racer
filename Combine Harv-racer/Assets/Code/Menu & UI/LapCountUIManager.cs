using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapCountUIManager : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI totalLaps;
    public TextMeshProUGUI lapCount;
    int calculatedLapCount;

    // Start is called before the first frame update
    void Start()
    {
        if(gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

        if(totalLaps == null)
        {
            totalLaps = GameObject.FindGameObjectWithTag("TotalLapsUI").GetComponent<TextMeshProUGUI>();
        }

        if(lapCount == null)
        {
            lapCount = GameObject.FindGameObjectWithTag("LapCountUI").GetComponent<TextMeshProUGUI>();
        }

        totalLaps.text = gameManager.totalLaps.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.lapCount > gameManager.totalLaps)
        {
            calculatedLapCount = gameManager.totalLaps;
        }
        else
        {
            calculatedLapCount = gameManager.lapCount + 1;
        }

        lapCount.text = calculatedLapCount.ToString();
    }
}
