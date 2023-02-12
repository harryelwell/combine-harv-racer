using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentFrontViewManager : MonoBehaviour
{
    public GameObject opponent;
    public OpponentManager opponentManager;
    
    // Start is called before the first frame update
    void Start()
    {
        if(opponent == null)
        {
            opponent = transform.parent.gameObject;
        }

        if(opponentManager == null)
        {
            opponentManager = opponent.GetComponent<OpponentManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(opponentManager.targetCheckpointNumber == 1 && opponentManager.lapsComplete <= 0)
        {
            if(col.tag == "Cow" || col.tag == "Wall")
            {
                //Debug.Log($"Obstacle entered front view of {transform.parent.transform.name}: {col.gameObject.transform.name}.");

                // Set the avoidingObstacle bool to true
                opponentManager.avoidingObstacle = true;
                // Tell the opponentManager to start avoiding
                opponentManager.AvoidObstacle();
            }
        }
        else
        {
            if(col.tag == "Player" || col.tag == "Opponent" || col.tag == "Cow" || col.tag == "Wall")
            {
                //Debug.Log($"Obstacle entered front view of {transform.parent.transform.name}: {col.gameObject.transform.name}.");

                // Set the avoidingObstacle bool to true
                opponentManager.avoidingObstacle = true;
                // Tell the opponentManager to start avoiding
                opponentManager.AvoidObstacle();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            //Debug.Log($"Player exited front view of {transform.parent.transform.name}: {col.gameObject.transform.name}.");
        }

        if(col.tag == "Opponent")
        {
            //Debug.Log($"Opponent exited front view of {transform.parent.transform.name}: {col.gameObject.transform.name}.");
        }

        if(col.tag == "Cow")
        {
            //Debug.Log($"Cow exited front view of {transform.parent.transform.name}: {col.gameObject.transform.name}.");
        }

        if(col.tag == "Wall")
        {
            //Debug.Log($"Wall exited front view of {transform.parent.transform.name}: {col.gameObject.transform.name}.");
        }
    }
}
