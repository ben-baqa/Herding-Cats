using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatState{
    Idle,
    GotoRandomLocation,
    RunningAway
}

public class CatMovement : MonoBehaviour {
    
    [Header("External References")]
    public GameObject dogObject;

    [Header("Movement Variables")]
    public Vector2 movementForce;
    public Vector2 maxVelocity;
    private Vector2 target;         //coordinates the cat is attempting to move towards
    public float stateChangeDelay;
    public float chaseThreshold;    //distance dog has to be to chase this cat

    [Header("Map Boundaries")]
    public Vector2 xRange;
    public Vector2 yRange;

    //Internal References/Variables
    private Animator anim;
    private Rigidbody2D rb;
    private CatState state;
    private float timeToChangeState;
    private Random rand;
    private Vector2 currentRandomTarget; //this is set each time the cat exits idle mode

    void Start(){
        anim = GetComponent<Animator>(); 
        rb = GetCOmponent<Rigidbody2D>();
        state = CatState.GotoRandomLocation;
        rand = new Random();
    }

    // Update is called once per frame
    void Update() {

        //set state to running if the dog is close no matter what 
        //Magnitude calulations must be based on x/y only, not z!
        Vector2 dogLoc = new Vector2(dog.transform.location.x, dog.transform.location.y);
        Vetcor2 catLoc = new Vector2(transform.location.x, transform.location.y);
        if((dogLoc - catLoc).magnitude < chaseThreshold){
            state = CatState.RunningAway;
        }

        //set the target location based on state
        switch(state){
            case CatState.GotoRandomLocation:
                if(Time.time > timeToChangeState){
                    //TODO set target to the randomly decided location
                    target = currentRandomTarget;
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = CatState.Idle;
                }
                break;

            case CatState.Idle:
                target = transform.location; 
                if(Time.time > timeToChangeState){
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = CatState.GotoRandomLocation;
                }
                break;

            case CatState.RunningAway:
                //if the cat is far enough away from teh dog, stop running and wander-
                if((dogLoc - catLoc).magnitude > chaseThreshold){
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = CatState.GotoRandomLocation;
                }
                target = -(dogLoc - catLoc); 
                break;
        }

        //move towards the target location
        float yDir = (target.y - transform.location.y);
        float xDir = (target.x - transform.location.x);
        rb.AddForce((target - transform.location));

        UpdateAnimator();
    }

    private void SetNewRandomLocation(){
        float xRand = (xRange.x, xRange.y);
        float yRand = (yRange.x, yRange.y);
        currentRandomTarget = new Vector2(xRand, yRand);
    }

    //this is just called at the end of every frame
    private void UpdateAnimator(){
        //TODO change the cat bool flags based on it's current x and y velocity
        //query rb.velocity.x and rb.velocity.y
    } 
}