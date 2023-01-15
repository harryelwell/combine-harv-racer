using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowZoneManager : MonoBehaviour
{
    public AssetLibrary assetLibrary;
    public GameObject spawnedCow;
    public CowManager cowManager;

    public bool isCorner;
    
    // Start is called before the first frame update
    void Start()
    {
        if(assetLibrary == null)
        {
            assetLibrary = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetLibrary>();
        }
        StartSpawnTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawnTimer()
    {
        StartCoroutine(SpawnTimer());
    }

    public IEnumerator SpawnTimer()
    {
        float waitPeriod = Random.Range(30f,120f);

        yield return new WaitForSeconds(waitPeriod);

        SpawnCow();
    }

    Vector3 SelectSpawnLocation()
    {
        Vector3 spawnLocation = transform.position;

        float xPosition = Random.Range(-3.4f,3.4f);
        float yPosition = Random.Range(-3.4f,3.4f);

        if(isCorner == true)
        {
            xPosition = Random.Range(-2.2f,2.2f);
            yPosition = Random.Range(-2.2f,2.2f);
        }        

        spawnLocation = new Vector3(transform.position.x + xPosition, transform.position.y + yPosition, transform.position.z);
        return spawnLocation;
    }

    void SpawnCow()
    {
        spawnedCow = Instantiate(assetLibrary.cowPrefab,SelectSpawnLocation(),Quaternion.identity, gameObject.transform);
        cowManager = spawnedCow.GetComponent<CowManager>();
        cowManager.cowZone = this.gameObject;
        cowManager.cowZoneManager = this;
    }
}
