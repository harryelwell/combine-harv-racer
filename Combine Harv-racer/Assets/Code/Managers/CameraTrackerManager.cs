using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackerManager : MonoBehaviour
{
    public GameObject player;
    public PlayerManager playerManager;
    // Start is called before the first frame update
    // public float startingDistance;
    // public float calculatedDistance;
    public float minDistance;
    public float maxDistance;
    public float distanceScale;
    public float lerpSpeed;
    public Vector3 velocity;
    public Vector3 motionVector;
    public float motionMagnitude;
    public Vector3 targetPosition;
    public Vector3 calculatedPosition;
    void Start()
    {
        if(player == null || playerManager == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerManager = player.GetComponent<PlayerManager>();
        }

        // if (startingDistance == 0)
        // {
        //     startingDistance = 4f;
        // }

    }

    // Update is called once per frame
    void Update()
    {
        CalculateTargetPosition();
    }
    
    public void CalculateTargetPosition()
    {
        // calculatedDistance = startingDistance + (playerManager.cornValue * 1.2f);

        // Vector3 targetPosition = new Vector3(player.transform.position.x,player.transform.position.y + calculatedDistance,0);

        // SetTargetPosition(targetPosition);

        velocity = playerManager.playerRigidbody.velocity;
        motionVector = velocity.normalized;
        motionMagnitude = velocity.magnitude;
        targetPosition = motionVector*(Mathf.Max(minDistance,Mathf.Min(maxDistance,motionMagnitude) * distanceScale));
        calculatedPosition = player.transform.position + targetPosition;

        SetTargetPosition();
    }

    public void SetTargetPosition()
    {
        transform.position = Vector3.Lerp(transform.position, calculatedPosition, lerpSpeed * Time.fixedDeltaTime);
    }
}
