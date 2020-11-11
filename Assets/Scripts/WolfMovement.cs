using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WolfState
{
    Idle,
    Wandering,
    Approach,
    Catch
}

public class WolfMovement : MonoBehaviour
{

    [Header("External References")]
    public GameObject cat;

    [Header("Movement Variables")]
    public float movementSpeed;
    private Vector2 target;
    public float stateChangeDelay;
    public float catchCooldown;
    public float chaseThreshold;

    [Header("Map Boundaries")]
    public Vector2 xRange;
    public Vector2 yRange;

    //Internal References/Variables
    private Animator anim;
    private Rigidbody2D rb;
    private WolfState state;
    private float timeToCatch;
    private float timeToChangeState;
    private Vector2 currentRandomTarget; //this is set each time the cat exits idle mode

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        state = WolfState.Wandering;
        timeToCatch = Time.time + catchCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        //set state to running if the dog is close no matter what 
        //Magnitude calulations must be based on x/y only, not z!
        Vector2 catLoc = new Vector2(cat.transform.position.x, cat.transform.position.y);
        Vector2 wolfLoc = new Vector2(transform.position.x, transform.position.y);

        if (Time.time > timeToCatch) {
            state = WolfState.Approach;
        }

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

                if ((catLoc - wolfLoc).magnitude < chaseThreshold && cat.GetComponent<CatMovement>().GetCatState() == CatState.Idle)
                {
                    timeToCatch = Time.time + catchCooldown;
                    state = WolfState.Catch;
                    target = wolfLoc;
                }
                else
                {
                    target = catLoc;
                }
                break;

            case WolfState.Catch:
                target = catLoc;
                if ((wolfLoc - target).magnitude < .1f)
                {
                    state = WolfState.Idle;
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
        if (state == WolfState.Wandering)
        {
            rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? movementSpeed / distance.magnitude : 0);
        }
        else if (state == WolfState.Approach) {
            rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? .8f * movementSpeed / distance.magnitude : 0);
        }
        else
        {
            rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? 6.4f * movementSpeed / distance.magnitude : 0);
        }
    }
}