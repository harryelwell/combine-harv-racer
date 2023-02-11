using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    [Header("Object References")]
    public GameManager gameManager;
    public PlayerActions playerActions;
    public Rigidbody2D playerRigidbody;

    [Header("Casting Objects")]
    public GameObject castStartPoint;
    public GameObject castFrontLeft;
    public GameObject castFrontRight;
    public GameObject castCheckLeft;
    public GameObject castCheckRight;

    [Header("Player Settings")]
    public float playerSpeed;
    public float turnSpeed;
    public float maxCornValue;
    public float cornValue; // min 0, max 3 (+0.3 each time you hit corn)
    public float raycastDistance;

    [Header("Automated Input Values")]
    public float accelerationValue;
    public float turnValue;

    public int dodgeValue;

    [Header("Gameplay Parameters")]
    public bool movementAllowed;
    public bool needToReverse;

    [Header("Checkpoint Tracking")]
    public float targetCheckpointNumber;
    public GameObject targetCheckpoint;
    public GameObject nearestTargetZone;
    public int lapsComplete;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        if (playerRigidbody == null)
        {
            Debug.Log("Rigidbody2D is null!");
        }

        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }

        SetTargetCheckpoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(gameManager.raceStarted == true && movementAllowed == true)
        {
            SetInputAcceleration();
            SetInputTurning();
            PlayerAcceleration();
            PlayerRotation();
        }
    }

    void SetInputAcceleration()
    {
        if(needToReverse == false)
        {
            accelerationValue = 1f;
        }
        else if(needToReverse == true)
        {
            accelerationValue = -1f;
        }
        else
        {
            accelerationValue = 0;
        }
    }

    void SetInputTurning()
    {
        turnValue = 0;

        // Set a target zone of the current targetCheckpoint
        SetTargetZone();

        // Turn towards targetZone

        Vector3 zoneDirectionLocal = transform.InverseTransformPoint(nearestTargetZone.transform.position);

                if(zoneDirectionLocal.x < -0.1f)
                {
                    //Debug.Log($"{targetCheckpoint.name} is on the left of {transform.name}");
                    turnValue = 1;
                }

                if(zoneDirectionLocal.x > 0.1f)
                {
                    //Debug.Log($"{targetCheckpoint.name} is on the right of {transform.name}");
                    turnValue = -1;
                }

        // Check for blockers in path

        //Vector3 leftCastDirection = (castStartPoint.transform.forward - castStartPoint.transform.right).normalized;

        //Vector3 rightCastDirection = (castStartPoint.transform.forward + castStartPoint.transform.right).normalized;

        // Debug.DrawRay(castStartPoint.transform.position,castStartPoint.transform.forward*raycastDistance,Color.cyan);
        // RaycastHit2D checkpointPathCheck = Physics2D.Raycast(castFrontLeft.transform.position,castFrontLeft.transform.forward,raycastDistance);
        
        // Debug.DrawRay(castFrontLeft.transform.position,castFrontLeft.transform.forward*(raycastDistance*0.8f),Color.cyan);
        // RaycastHit2D checkpointPathCheckL = Physics2D.Raycast(castFrontLeft.transform.position,castFrontLeft.transform.forward,raycastDistance*0.8f);

        // Debug.DrawRay(castFrontRight.transform.position,castFrontRight.transform.forward*(raycastDistance*0.8f),Color.cyan);
        // RaycastHit2D checkpointPathCheckR = Physics2D.Raycast(castFrontRight.transform.position,castFrontRight.transform.forward,raycastDistance*0.8f);

        // if(checkpointPathCheck.collider != null || checkpointPathCheckL.collider != null || checkpointPathCheckR.collider != null)
        // {            
        //     //OldAvoidCollision()

        //     // Figure out which check got a collision
        //     Collider2D obstacle = checkpointPathCheck.collider;

        //     if(checkpointPathCheckL.collider != null)
        //     {
        //         obstacle = checkpointPathCheckL.collider;
        //     }

        //     if(checkpointPathCheckR.collider != null)
        //     {
        //         obstacle = checkpointPathCheckR.collider;
        //     }

        //     if(checkpointPathCheck.collider != null)
        //     {
        //         obstacle = checkpointPathCheck.collider;
        //     }

        //     if (obstacle != null)
        //     {
        //         Vector2 vectorToTarget = nearestTargetZone.transform.position - transform.position;
        //         vectorToTarget.Normalize();
        //         AvoidCollision(vectorToTarget, out vectorToTarget, obstacle);
        //     }
            
        // }
    }

    // void AvoidCollision(Vector2 vectorToTarget, out Vector2 newVectorToTarget, Collider2D obstacle)
    // {
    //     // ADAPT CODE FROM https://www.youtube.com/watch?v=5SJ6AAI6Wcs&ab_channel=PrettyFlyGames

    //     Vector2 avoidanceVector = Vector2.zero;
        
    //     // Calculate the reflecting vector if we were to hit the obstacle
    //     avoidanceVector = Vector2.Reflect((obstacle.gameObject.transform.position - transform.position).normalized, obstacle.transform.right);

    //     // Calculate the distance to the target checkpoint
    //     float distanceToTarget = (nearestTargetZone.transform.position - transform.position).magnitude;
    //     // The close the harvester gets to the checkpoint, the need to reach the checkpoint increases
    //     float driveToTargetInfluence = 6.0f / distanceToTarget;
    //     Debug.Log($"{transform.name} driveToTargetInfluence: {driveToTargetInfluence}");
    //     // Limit the influence to fixed values between 0.3 and 1
    //     driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.30f, 1.0f);
    //     Debug.Log($"{transform.name} Clamped driveToTargetInfluence: {driveToTargetInfluence}");

    //     // Calculate the competing desire to avoid the obstacle (inverse of the driveToTargetInfluence)
    //     float avoidanceInfluence = 1.0f - driveToTargetInfluence;

    //     // Avoidance vector
    //     newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVector * avoidanceInfluence;
    //     newVectorToTarget.Normalize();

    //     // Draw the avoidance vector
    //     Debug.DrawRay(transform.position, avoidanceVector * raycastDistance, Color.green);

    //     // Draw the route the harvester will actually take in yellow
    //     Debug.DrawRay(transform.position, newVectorToTarget * raycastDistance, Color.yellow);

    //     // Log the avoidanceVector
    //     Debug.Log($"{transform.name} avoidanceVector.x: {avoidanceVector.x}, avoidanceVector.y: {avoidanceVector.y}");

    //     // NEXT STEP: Interpret the avoidanceVector into turnValue!
    //     turnValue = -avoidanceVector.x;
    // }

    void PlayerAcceleration()
    {
        float calculatedPlayerSpeed = playerSpeed * (1 + cornValue);
        //Debug.Log($"cornValue is {cornValue}, playerSpeed is {playerSpeed}, calculatedPlayerSpeed {calculatedPlayerSpeed}");

        if(accelerationValue > 0f)
        {
            playerRigidbody.velocity = transform.up * calculatedPlayerSpeed * Time.fixedDeltaTime;
        }
        else if(accelerationValue < 0f)
        {
            playerRigidbody.velocity = -transform.up * (calculatedPlayerSpeed * 0.5f) * Time.fixedDeltaTime;
        }
        else
        {
            //Debug.Log($"No clear acceleration input, do nothing!");
        }
    }

    void PlayerRotation()
    {
        float calculatedTurnValue = turnValue;
        if(accelerationValue == -1f)
        {
            calculatedTurnValue = -turnValue;
        }

        calculatedTurnValue *= (1 + (cornValue * 0.15f));

        transform.Rotate(0,0,calculatedTurnValue * turnSpeed * Time.fixedDeltaTime);
    }

    public void SetTargetCheckpoint()
    {
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        //Debug.Log($"Checkpoint count: {checkpoints.Length}");
        if(targetCheckpointNumber == checkpoints.Length)
        {
            targetCheckpointNumber = 0;
            //lapsComplete += 1; << THIS NEEDS TO BE IN A SEPARATE THING FOR CROSSING CHECKPOINT 1

            // Check whether lapsComplete == totalLaps in GameManager
            // Mark position at race end if so
        }

        targetCheckpointNumber += 1;
        targetCheckpoint = GameObject.Find($"Checkpoint {targetCheckpointNumber}");
    }

    void SetTargetZone()
    {
        Transform[] checkpointZones = targetCheckpoint.GetComponentsInChildren<Transform>();
        //Debug.Log($"Child count: {checkpointZones.Length}");

        GameObject targetZone = null;
        float targetZoneDistance = 0;

        foreach(Transform zone in checkpointZones)
        {
            float distance = Vector3.Distance(transform.position,zone.transform.position);
            //Debug.Log($"{transform.name} distance from {zone.transform.name} is {distance}.");

            if(targetZoneDistance == 0 || targetZoneDistance > distance)
            {
                targetZone = zone.gameObject;
                targetZoneDistance = distance;
            }
        }

        // output the targetZone
        nearestTargetZone = targetZone;
    }

    public IEnumerator CowCollision(GameObject cow)
    {
        movementAllowed = false;

        //trigger spining in a circle twice
        StartCoroutine(SpinOut());

        yield return new WaitForSeconds(3f);

        //Debug.Log("Start moving again!");
        movementAllowed = true;

        Destroy(cow);
    }

    IEnumerator SpinOut()
    {
        float spinDuration = 2f;
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + 1080f;
        float timeElapsed = 0f;

        while(timeElapsed < spinDuration)
        {
            timeElapsed += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, timeElapsed/spinDuration) % 1080f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,zRotation);
            yield return null;
        }

        //transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,startRotation);
    }

    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     StopCoroutine(StopReversing());
        
    //     if(col.gameObject.tag == "Player" || col.gameObject.tag == "Harvester" || col.gameObject.tag == "Opponent" || col.gameObject.tag == "Wall")
    //     {
            
    //         // if raycast forward and see the collision object, do the below
    //         Debug.DrawLine(castStartPoint.transform.position,castStartPoint.transform.forward*300f,Color.red);
    //         RaycastHit2D checkInfront = Physics2D.Raycast(castStartPoint.transform.position,castStartPoint.transform.forward,2f);

    //         if(checkInfront.collider != null)
    //         {
    //             if(checkInfront.collider.tag == "Player" || checkInfront.collider.tag == "Harvester" || checkInfront.collider.tag == "Opponent" || checkInfront.collider.tag == "Wall")
    //             {
    //                 needToReverse = true;
    //                 Debug.Log($"{transform.name} started reversing.");
    //                 StartCoroutine(StopReversing());
    //             }
    //         } 
    //     }
    // }

    IEnumerator StopReversing()
    {   
        yield return new WaitForSeconds(1.5f);

        Debug.Log($"{transform.name} stopping reversing.");

        needToReverse = false;
    }
}
