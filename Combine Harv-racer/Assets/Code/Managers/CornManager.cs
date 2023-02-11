using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornManager : MonoBehaviour
{
    public AssetLibrary assetLibrary;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem cornParticle;
    public int cornLevel; // 0 = cut, 1 = base, 2 = short, 3 = long
    
    // Start is called before the first frame update
    void Start()
    {
        if(assetLibrary == null)
        {
            assetLibrary = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetLibrary>();
        }
        
        //SetCornlevel(Random.Range(0,4));
        SetCornlevel(3);

        if(cornLevel > 0 && cornLevel < 3)
        {
            StartCoroutine(GrowCorn());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetCornlevel(int newLevel)
    {
        // change the int
        cornLevel = newLevel;

        // update the sprite
        if(newLevel == 0)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0);
            StartCoroutine(GrowCorn());
        }

        if(newLevel == 1)
        {
            spriteRenderer.sprite = assetLibrary.cornBase;
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,1);
        }

        if(newLevel == 2)
        {
            spriteRenderer.sprite = assetLibrary.cornShort;
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,1);
        }

        if(newLevel == 3)
        {
            spriteRenderer.sprite = assetLibrary.cornLong;
            spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,1);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && cornLevel >= 2)
        {
            PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();

            // Stop any corn growth
            StopAllCoroutines();

            if(cornLevel == 2 && playerManager.cornValue < playerManager.maxCornValue)
            {
                playerManager.cornValue += 0.0015f;
            }

            if(cornLevel == 3 && playerManager.cornValue < playerManager.maxCornValue)
            {
                playerManager.cornValue += 0.004f;
            }

            // Trigger particle
            cornParticle.Play();
            
            // Start growing from scratch again
            SetCornlevel(0);
        }

        if(col.gameObject.tag == "Opponent" && cornLevel >= 2)
        {
            OpponentManager opponentManager = col.gameObject.GetComponent<OpponentManager>();

            // Stop any corn growth
            StopAllCoroutines();

            if(cornLevel == 2 && opponentManager.cornValue < opponentManager.maxCornValue)
            {
                opponentManager.cornValue += 0.0015f;
            }

            if(cornLevel == 3 && opponentManager.cornValue < opponentManager.maxCornValue)
            {
                opponentManager.cornValue += 0.004f;
            }

            // Trigger particle
            cornParticle.Play();
            
            // Start growing from scratch again
            SetCornlevel(0);
        }
    }

    IEnumerator GrowCorn()
    {
        float growthCycleDuration = Random.Range(15f,45f);

        while(cornLevel < 3)
        {
            yield return new WaitForSeconds(growthCycleDuration);
            //Debug.Log("Timer complete!");
            growthCycleDuration = Random.Range(8f,24f);

            SetCornlevel(cornLevel + 1);
        }

        //Debug.Log("Corn has grown");
    }
}
