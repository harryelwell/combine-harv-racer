using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CornMeterManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public Slider cornSlider;
    
    // Start is called before the first frame update
    void Start()
    {
        if(playerManager == null)
        {
            playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        }

        if(cornSlider == null)
        {
            cornSlider = this.GetComponent<Slider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        cornSlider.value = playerManager.cornValue / 4.4f;
    }
}
