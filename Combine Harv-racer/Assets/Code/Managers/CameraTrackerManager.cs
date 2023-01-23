using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTrackerManager : MonoBehaviour
{
    public GameObject player;
    public PlayerManager playerManager;
    public GameObject cameraLock;
    public CinemachineTargetGroup targetGroup;
    public float targetGroupRadius;
    public float radiusModifier;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null || playerManager == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerManager = player.GetComponent<PlayerManager>();
        }

        targetGroup.m_Targets[0].radius = targetGroupRadius;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateRadius();
    }

    public void LockCamera()
    {
        // Set the position of the CameraLock to the current camera position
        cameraLock.transform.position = transform.position;

        // Tell cinemachine to follow the cameralock
        targetGroup.AddMember(cameraLock.transform,1f,targetGroupRadius + radiusModifier);
        targetGroup.RemoveMember(transform);
    }

    public void UnlockCamera()
    {
        // Tell the cinemachine to look back at the cameraTracker again
        targetGroup.AddMember(transform,1f,targetGroupRadius + radiusModifier);
        targetGroup.RemoveMember(cameraLock.transform);
    }

    public void CalculateRadius()
    {
        // radius mofidier += 0.25f for every 0.5f of cornValue
        radiusModifier = 0.25f * Mathf.Floor(playerManager.cornValue/0.5f);

        // radius of targetGroup member = targetGroupRadius + radiusModifier
        targetGroup.m_Targets[0].radius = targetGroupRadius + radiusModifier;
    }
}
