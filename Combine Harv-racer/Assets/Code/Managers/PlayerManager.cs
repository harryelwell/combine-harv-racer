using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Object References")]
    public GameManager gameManager;
    public PlayerActions playerActions;
    public Rigidbody2D playerRigidbody;
    [Header("Player Settings")]
    public float playerSpeed;
    public float turnSpeed;
    public float cornValue; // min 0, max 3 (+0.3 each time you hit corn)
    [Header("Gameplay Parameters")]
    public bool movementAllowed;
    
    void Awake()
    {
        playerActions = new PlayerActions();

        playerRigidbody = GetComponent<Rigidbody2D>();
        if (playerRigidbody == null)
        {
            Debug.Log("Rigidbody2D is null!");
        }

        if (gameManager == null)
        {
            gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        }
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(gameManager.raceStarted == true && movementAllowed == true)
        {
            PlayerVelocityAcceleration();
            PlayerVelocityRotation();
        }
    }

    void PlayerVelocityAcceleration()
    {
        float calculatedPlayerSpeed = playerSpeed * (1 + cornValue);
        //Debug.Log($"cornValue is {cornValue}, playerSpeed is {playerSpeed}, calculatedPlayerSpeed {calculatedPlayerSpeed}");

        float accelerationValue = playerActions.PlayerMap.Accelerate.ReadValue<float>();

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

    void PlayerVelocityRotation()
    {
        float turnValue = playerActions.PlayerMap.Turn.ReadValue<float>();

        float calculatedTurnValue = turnValue;
        if(playerActions.PlayerMap.Accelerate.ReadValue<float>() == -1f)
        {
            calculatedTurnValue = -turnValue;
        }

        calculatedTurnValue *= (1 + (cornValue * 0.15f));

        transform.Rotate(0,0,calculatedTurnValue * turnSpeed * Time.fixedDeltaTime);
    }

    public IEnumerator CowCollision(GameObject cow)
    {
        movementAllowed = false;

        //trigger spining in a circle twice
        StartCoroutine(SpinOut());

        yield return new WaitForSeconds(3f);

        Debug.Log("Start moving again!");
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
}
