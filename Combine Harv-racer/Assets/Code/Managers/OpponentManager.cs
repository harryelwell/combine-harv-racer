using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    [Header("Object References")]
    public GameManager gameManager;
    public PlayerActions playerActions;
    public Rigidbody2D playerRigidbody;
    public GameObject castStartPoint;

    [Header("Player Settings")]
    public float playerSpeed;
    public float turnSpeed;
    public float cornValue; // min 0, max 3 (+0.3 each time you hit corn)

    [Header("Automated Input Values")]
    public float accelerationValue;
    public float turnValue;

    [Header("Gameplay Parameters")]
    public bool movementAllowed;
    public bool needToReverse;

    [Header("Checkpoint Tracking")]
    public float targetCheckpointNumber;
    public GameObject targetCheckpoint;
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

        // Turn towards checkpoint

        Vector3 checkpointDirectionLocal = transform.InverseTransformPoint(targetCheckpoint.transform.position);

                if(checkpointDirectionLocal.x < -0.1f)
                {
                    //Debug.Log($"{targetCheckpoint.name} is on the left of {transform.name}");
                    turnValue = 1;
                }

                if(checkpointDirectionLocal.x > 0.1f)
                {
                    //Debug.Log($"{targetCheckpoint.name} is on the right of {transform.name}");
                    turnValue = -1;
                }

        // Check for blockers in path

        Debug.DrawLine(castStartPoint.transform.position,targetCheckpoint.transform.position,Color.cyan);
        RaycastHit2D checkpointPathCheck = Physics2D.Linecast(castStartPoint.transform.position,targetCheckpoint.transform.position);

        if(checkpointPathCheck.collider != null)
        {            
            //Debug.Log($"{checkpointPathCheck.collider.name} blocking path to {targetCheckpoint.name}");

            if(checkpointPathCheck.collider.tag == "Player" || checkpointPathCheck.collider.tag == "Harvester" || checkpointPathCheck.collider.tag == "Cow" || checkpointPathCheck.collider.tag == "Opponent")
            {
                Vector3 blockerDirectionLocal = transform.InverseTransformPoint(checkpointPathCheck.collider.transform.position);

                if(blockerDirectionLocal.x < 0)
                {
                    //Debug.Log($"{checkpointPathCheck.collider.name} is on the left of {transform.name}");
                    turnValue = -1;
                }

                if(blockerDirectionLocal.x > 0)
                {
                    //Debug.Log($"{checkpointPathCheck.collider.name} is on the right of {transform.name}");
                    turnValue = 1;
                }
            }

            // readjust for walls

            if(checkpointPathCheck.collider.tag == "Wall")
            {
                Vector3 blockerDirectionLocal = transform.InverseTransformPoint(checkpointPathCheck.collider.transform.position);

                if(blockerDirectionLocal.x < 0)
                {
                    //Debug.Log($"{checkpointPathCheck.collider.name} is on the left of {transform.name}");
                    turnValue = -1;
                }

                if(blockerDirectionLocal.x > 0)
                {
                    //Debug.Log($"{checkpointPathCheck.collider.name} is on the right of {transform.name}");
                    turnValue = 1;
                }
            }
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

    void OnCollisionEnter2D(Collision2D col)
    {
        StopCoroutine(StopReversing());
        
        if(col.gameObject.tag == "Player" || col.gameObject.tag == "Harvester" || col.gameObject.tag == "Opponent" || col.gameObject.tag == "Wall")
        {
            needToReverse = true;
            Debug.Log($"{transform.name} started reversing.");
            StartCoroutine(StopReversing());
        }
    }

    IEnumerator StopReversing()
    {   
        yield return new WaitForSeconds(1.5f);

        Debug.Log($"{transform.name} stopping reversing.");

        needToReverse = false;
    }
}
