using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WolfState
{
    Idle,
    Wandering,
    Approach,
    Exit
}

public class WolfMovement : MonoBehaviour
{

    [Header("External References")]
    public GameObject dog;

    [Header("Movement Variables")]
    public float movementSpeed;
    private Vector2 target;
    public float stateChangeDelay;
    public float chaseCooldown;
    public float chaseThreshold;

    [Header("Map Boundaries")]
    public Vector2 xRange;
    public Vector2 yRange;

    //Internal References/Variables
    private Animator anim;
    private Rigidbody2D rb;
    private WolfState state;
    private float timeToChaseCat;
    private float timeToChangeState;
    private Vector2 currentRandomTarget; //this is set each time the cat exits idle mode

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        state = WolfState.Wandering;
        timeToChaseCat = Time.time + chaseCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //set state to running if the dog is close no matter what 
        //Magnitude calulations must be based on x/y only, not z!
        Vector2 dogLoc = new Vector2(dog.transform.position.x, dog.transform.position.y);
        Vector2 wolfLoc = new Vector2(transform.position.x, transform.position.y);

        bool hearingDogBark = (dog.GetComponent<DogMovement>().IsDogBarking() && (dogLoc - wolfLoc).magnitude < chaseThreshold * 1.6f);

        //if (Time.time > timeToChaseCat && state != WolfState.Approach) {
        //    state = WolfState.Approach;
        //    timeToChangeState = Time.time + stateChangeDelay;
        //}

        //set the target location based on state
        switch (state)
        {
            case WolfState.Wandering:
                target = currentRandomTarget;
                if (Time.time > timeToChangeState)
                {
                    //TODO set target to the randomly decided location
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = WolfState.Idle;
                }
                break;

            case WolfState.Idle:
                target = transform.position;
                if (Time.time > timeToChangeState)
                {
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = WolfState.Wandering;
                }
                break;

            case WolfState.Approach:
                target = Vector2.zero;
                if (Time.time > timeToChangeState)
                {
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = WolfState.Wandering;
                }
                break;
            case WolfState.Exit:
                target = new Vector2(xRange.x - 1, yRange.x - 1);
                if ((target - wolfLoc).magnitude < 0.1f)
                {
                    Destroy(gameObject);
                }
                break;
        }
        UpdateAnimator();
    }

    private void SetNewRandomLocation()
    {
        float xRand = Random.Range(xRange.x, xRange.y);
        float yRand = Random.Range(yRange.x, yRange.y);
        currentRandomTarget = new Vector2(xRand, yRand);
    }

    //this is just called at the end of every frame
    private void UpdateAnimator()
    {
        Vector2 distance = target - (Vector2)transform.position;
        if (distance.magnitude > 0)
        {
            if (state == WolfState.Wandering)
            {
                rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? movementSpeed / distance.magnitude : 0);
            }
            else if (state == WolfState.Approach)
            {
                rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? .8f * movementSpeed / distance.magnitude : 0);
            }
            else
            {
                rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? 6.4f * movementSpeed / distance.magnitude : 0);
            }
        }
        else {
            rb.velocity = Vector2.zero;
        }
    }

    public WolfState GetWolfState() {
        return state;
    }
}