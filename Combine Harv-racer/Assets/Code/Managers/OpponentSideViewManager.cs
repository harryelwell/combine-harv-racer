using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentSideViewManager : MonoBehaviour
{
    public List<CornManager> cornManagers;
    public float cornValue;
    public float obstacleValue;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCornValue();
    }

    void GetCornValue()
    {
        // Reset corn value
        cornValue = 0;

        // Iterate through the cornManagers and set the combined cornValue
        foreach(CornManager cornManager in cornManagers)
        {
            if(cornManager.cornLevel >= 2)
            {
                cornValue += (cornManager.cornLevel - 1) * 0.25f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            obstacleValue += 0.5f;
        }

        if(col.tag == "Opponent")
        {
            obstacleValue += 0.5f;
        }

        if(col.tag == "Cow")
        {
            obstacleValue += 1.0f;
        }

        if(col.tag == "Wall")
        {
            obstacleValue += 2.5f;
        }

        if(col.tag == "Corn")
        {
            cornManagers.Add(col.gameObject.GetComponent<CornManager>());
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            obstacleValue -= 0.5f;
        }

        if(col.tag == "Opponent")
        {
            obstacleValue -= 0.5f;
        }

        if(col.tag == "Cow")
        {
            obstacleValue -= 1.0f;
        }

        if(col.tag == "Wall")
        {
            obstacleValue -= 2.5f;
        }

        if(col.tag == "Corn")
        {
            cornManagers.Remove(col.gameObject.GetComponent<CornManager>());
        }
    }
}
