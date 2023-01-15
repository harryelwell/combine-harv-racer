using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public GameManager gameManager;
    public int checkpointNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            Debug.Log($"Player crossed checkpoint {checkpointNumber}!");

            if(checkpointNumber == 1 && gameManager.lastCheckpoint == gameManager.checkpointCount)
            {
                Debug.Log("Crossed finish line!");
                
                // Do a check thing in game manager to either do nothing (if starting grid), increase the lap count, or win the race
                gameManager.lastCheckpoint = checkpointNumber;
                gameManager.FinishLineCrossed();
            }
            else
            {
                if(checkpointNumber == gameManager.lastCheckpoint + 1)
                {
                    gameManager.lastCheckpoint = checkpointNumber;
                }
                else if(checkpointNumber == gameManager.lastCheckpoint - 1)
                {
                    Debug.Log($"Wrong way, turn around!");
                }
                else if(checkpointNumber == gameManager.lastCheckpoint)
                {
                    Debug.Log("Player has already crossed this checkpoint, do nothing.");
                }
                else
                {
                    Debug.Log("Checkpoint is neither next or previous, something has been skipped by accident.");
                }
            }
        }   
    }


}
