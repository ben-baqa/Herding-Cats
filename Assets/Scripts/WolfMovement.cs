﻿using System.Collections;
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
    public float barkingCooldown;
    public float exitTime;

    [Header("Map Boundaries")]
    public Vector2 xRange;
    public Vector2 yRange;

    //Internal References/Variables
    private Animator anim;
    private Rigidbody2D rb;
    private WolfState state;
    private float timeToChaseCat;
    private float timeToBark;
    private float timeToChangeState;
    private float timeToExit;
    private Vector2 currentRandomTarget; //this is set each time the cat exits idle mode
    private AudioSource bark;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bark = GetComponent<AudioSource>();
        state = WolfState.Wandering;
        timeToChaseCat = Time.time + chaseCooldown;
        timeToBark = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //set state to running if the dog is close no matter what 
        //Magnitude calulations must be based on x/y only, not z!
        Vector2 dogLoc = new Vector2(dog.transform.position.x, dog.transform.position.y);
        Vector2 wolfLoc = new Vector2(transform.position.x, transform.position.y);

        bool hearingDogBark = (dog.GetComponent<DogMovement>().IsDogBarking() && (dogLoc - wolfLoc).magnitude < chaseThreshold * 1.6f);

        if (Time.time > timeToChaseCat && state != WolfState.Approach && state != WolfState.Exit)
        {
            state = WolfState.Approach;
            timeToChangeState = Time.time + stateChangeDelay;
        }

        if (hearingDogBark && state != WolfState.Exit)
        {
            target = GetRandomSpotOutsideCamera();
            state = WolfState.Exit;
            timeToExit = Time.time + exitTime;
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
                Bark();
                break;

            case WolfState.Idle:
                target = transform.position;
                if (Time.time > timeToChangeState)
                {
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = WolfState.Wandering;
                }
                Bark();
                break;

            case WolfState.Approach:
                target = Vector2.zero;

                if ((target - wolfLoc).magnitude < 0.1f)
                {
                    Destroy(gameObject);
                }

                if (Time.time > timeToChangeState)
                {
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = WolfState.Wandering;
                }
                Bark();
                break;

            case WolfState.Exit:
                if ((target - wolfLoc).magnitude < 0.1f || Time.time > timeToExit)
                {
                    Destroy(gameObject);
                }
                break;
        }
        UpdateAnimator();
    }

    private void Bark()
    {
        if (Time.time > timeToBark)
        {
            bark.Play();
            timeToBark = Time.time + barkingCooldown * Random.Range(0.5f, 2);
        }
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

        anim.SetFloat("Vertical", rb.velocity.y);
        anim.SetFloat("Horizontal", rb.velocity.x);
        anim.SetFloat("Magnitude", rb.velocity.magnitude);
    }

    public WolfState GetWolfState() {
        return state;
    }

    public Vector2 GetRandomSpotOutsideCamera()
    {
        if (Random.value > 0.5f)
        {
            return new Vector2(xRange.x - 1, Random.Range(yRange.x, yRange.y));
        }
        else { 
            return new Vector2(xRange.y + 1, Random.Range(yRange.x, yRange.y));
        }
    }
}