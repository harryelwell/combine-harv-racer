using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    [Header("Object References")]
    public GameManager gameManager;
    public PlayerActions playerActions;
    public Rigidbody2D playerRigidbody;
    public OpponentFrontViewManager frontViewmanager;
    public OpponentSideViewManager leftViewManager;
    public OpponentSideViewManager rightViewManager;
    public GameObject tempTargetLeft;
    public GameObject tempTargetRight;

    [Header("Player Settings")]
    public float playerSpeed;
    public float turnSpeed;
    public float maxCornValue;
    public float cornValue; // min 0, max 3 (+0.3 each time you hit corn)
    public float raycastDistance;

    [Header("Automated Input Values")]
    public float accelerationValue;
    public float turnValue;

    [Header("Gameplay Parameters")]
    public bool movementAllowed;
    public bool needToReverse;
    public bool avoidingObstacle;

    [Header("Checkpoint Tracking")]
    public float targetCheckpointNumber;
    public GameObject targetCheckpoint;
    public GameObject nearestTargetZone;
    public Vector3 nearestTargetZonePosition;
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
        SetTargetZone();
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

        // Set a target zone of the current targetCheckpoint, as long as we're not currently avoiding an obstacle
        if(avoidingObstacle == true)
        {
            CheckIfObstacleCleared();
        }
        else
        {
            SetTargetZone();
            CheckForCorn();
        }

        // Turn towards targetZone

        Vector3 zoneDirectionLocal = transform.InverseTransformPoint(nearestTargetZonePosition);

        if(zoneDirectionLocal.x < -0.2f)
        {
            //Debug.Log($"{targetCheckpoint.name} is on the left of {transform.name}");
            turnValue = 1;
        }

        if(zoneDirectionLocal.x > 0.2f)
        {
            //Debug.Log($"{targetCheckpoint.name} is on the right of {transform.name}");
            turnValue = -1;
        }
    }

    public void AvoidObstacle()
    {
        // Debug.Log("Avoiding obstacle!");

        // Check obstacleValue of left and right
        float obstacleValueLeft = leftViewManager.obstacleValue;
        float obstacleValueRight = rightViewManager.obstacleValue;

        //Debug.Log($"{transform.name} obstacleValueLeft: {obstacleValueLeft}, obstacleValueRight: {obstacleValueRight}.");

        // Whichever has the lowest value, pick that side and set a new temporary nearestTargetZone in that direction (the automated turning will handle the rest)
        if(obstacleValueLeft == obstacleValueRight)
        {
            // Randomly select left or right
            if(Random.Range(0,2) == 1)
            {
                nearestTargetZonePosition = tempTargetLeft.transform.position;
            }
            else
            {
                nearestTargetZonePosition = tempTargetRight.transform.position;
            }
        }
        else if(obstacleValueLeft < obstacleValueRight)
        {
            // Set new target zone on the left
            nearestTargetZonePosition = tempTargetLeft.transform.position;
        }
        else
        {
            // Set new target zone on the right
            nearestTargetZonePosition = tempTargetRight.transform.position;
        }

        // Check whether player has reached the target location, when it has, set avoidingObstacle to false again
    }

    public void CheckIfObstacleCleared()
    {
        Debug.Log($"{transform.name} tempTargetPosition: {nearestTargetZonePosition}, currentPosition: {transform.position}.");

        Debug.Log($"{transform.name} Distance to tempTargetPosition: {(nearestTargetZonePosition - transform.position).magnitude}.");

        if((nearestTargetZonePosition - transform.position).magnitude < 0.1f || (nearestTargetZonePosition - transform.position).magnitude >= 2.7f)
        {
            avoidingObstacle = false;
        }
    }

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

        calculatedTurnValue *= (1 + (cornValue * 0.3f));

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
        nearestTargetZonePosition = nearestTargetZone.transform.position;
    }

    void CheckForCorn()
    {
        // Check the corn value of front view, left view, right view

        // If left or right view is considerably higher than front view, switch path in the same way the obstacle avoidance stuff works
    }

    public void CrossedLine()
    {
        if(lapsComplete == gameManager.totalLaps - 1)
        {
            Debug.Log($"RACE OVER! {transform.name} wins!");
            
            // Temp stop gameplay
            gameManager.raceStarted = false;
        }
        else
        {
            if(targetCheckpointNumber == 1)
            {
                lapsComplete += 1;
                SetTargetCheckpoint();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Wall")
        {
            StartCoroutine(StopAccelerating(0.7f));
        }
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
    }

    public void StartReversing()
    {   
        needToReverse = true;
        //Debug.Log($"{transform.name} started reversing.");
        StartCoroutine(StopReversing());
    }

    IEnumerator StopReversing()
    {   
        yield return new WaitForSeconds(1.5f);

        //Debug.Log($"{transform.name} stopping reversing.");

        needToReverse = false;
        avoidingObstacle = false;
        SetTargetZone();
    }

    IEnumerator StopAccelerating(float seconds)
    {
        movementAllowed = false;
        
        yield return new WaitForSeconds(seconds);

        movementAllowed = true;
    }
}
