using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterCollisionManager : MonoBehaviour
{   
    public GameObject opponent;
    public OpponentManager opponentManager;
    public bool inCollision;
    
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Harvester" || col.gameObject.tag == "Opponent" || col.gameObject.tag == "Wall")
        {
            //Debug.Log($"{transform.name} collided with {col.gameObject.name}.");
            
            inCollision = true;
            StartCoroutine(CollisionWait(col));
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        //Debug.Log($"{transform.name} has exited collision with {col.gameObject.name}.");
        inCollision = false;
    }

    IEnumerator CollisionWait(Collision2D col)
    {
        yield return new WaitForSeconds(1f);

        if(inCollision == true)
        {
            //Debug.Log($"{transform.name} stuck in collision with {col.gameObject.name}, start reversing.");
            opponentManager.StartReversing();
        }

        yield return null;
    }


}
