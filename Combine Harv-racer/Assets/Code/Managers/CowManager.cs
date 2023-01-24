using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : MonoBehaviour
{
    public AssetLibrary assetLibrary;
    public SoundFXManager soundFXManager;
    public ParticleSystem cowParticle;
    public GameObject cowZone;
    public CowZoneManager cowZoneManager;
    public bool cowTriggered;

    // Start is called before the first frame update
    void Start()
    {
        if(assetLibrary == null)
        {
            assetLibrary = GameObject.FindGameObjectWithTag("GameController").GetComponent<AssetLibrary>();
        }

        if(soundFXManager == null)
        {
            soundFXManager = GameObject.FindGameObjectWithTag("SFXManager").GetComponent<SoundFXManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && cowTriggered == false)
        {
            PlayerManager playerManager = col.gameObject.GetComponent<PlayerManager>();

            // Hide the cow
            this.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);

            StartCoroutine(playerManager.CowCollision(this.gameObject));

            if(playerManager.cornValue > 2f)
            {
                playerManager.cornValue -= 2f;
            }
            else
            {
                playerManager.cornValue = 0f;
            }

            // Trigger particle
            cowParticle.Play();

            soundFXManager.cowHit.Play();

            // Start the Cow Zone coroutine to start spawning a new cow
            cowZoneManager.StartSpawnTimer();

            cowTriggered = true;
        }

        if(col.gameObject.tag == "Opponent" && cowTriggered == false)
        {
            OpponentManager opponentManager = col.gameObject.GetComponent<OpponentManager>();

            // Hide the cow
            this.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0f);

            StartCoroutine(opponentManager.CowCollision(this.gameObject));

            if(opponentManager.cornValue > 2f)
            {
                opponentManager.cornValue -= 2f;
            }
            else
            {
                opponentManager.cornValue = 0f;
            }

            // Trigger particle
            cowParticle.Play();

            soundFXManager.cowHit.Play();

            // Start the Cow Zone coroutine to start spawning a new cow
            cowZoneManager.StartSpawnTimer();

            cowTriggered = true;
        }
    }
}
