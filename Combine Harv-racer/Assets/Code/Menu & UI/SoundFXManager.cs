using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource buttonClick;
    public AudioSource trafficLight;
    public AudioSource goGoGo;
    public AudioSource raceOver;
    public AudioSource cowHit;
    public AudioSource hitWall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlaySound(AudioSource audioSource)
    {
        audioSource.Play();
    }
}
