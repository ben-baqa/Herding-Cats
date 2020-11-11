using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatState
{
    Idle,
    GotoRandomLocation,
    RunningAway
}

public class CatMovement : MonoBehaviour
{

    [Header("External References")]
    public GameObject dog;

    [Header("Movement Variables")]
    public float movementSpeed;
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
    private SpriteRenderer spriteRenderer;
    private Sprite sprite;
    private float timeToChangeState;
    private Vector2 currentRandomTarget; //this is set each time the cat exits idle mode

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        state = CatState.GotoRandomLocation;
    }

    // Update is called once per frame
    void Update()
    {
        //set state to running if the dog is close no matter what 
        //Magnitude calulations must be based on x/y only, not z!
        Vector2 dogLoc = new Vector2(dog.transform.position.x, dog.transform.position.y);
        Vector2 catLoc = new Vector2(transform.position.x, transform.position.y);
        if ((dogLoc - catLoc).magnitude < chaseThreshold)
        {
            state = CatState.RunningAway;
        }
        //set the target location based on state
        switch (state)
        {
            case CatState.GotoRandomLocation:
                target = currentRandomTarget;
                if (Time.time > timeToChangeState)
                {
                    //TODO set target to the randomly decided location
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = CatState.Idle;
                }
                break;

            case CatState.Idle:
                target = transform.position;
                if (Time.time > timeToChangeState)
                {
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = CatState.GotoRandomLocation;
                }
                break;

            case CatState.RunningAway:
                //if the cat is far enough away from teh dog, stop running and wander-
                if ((dogLoc - catLoc).magnitude > chaseThreshold)
                {
                    SetNewRandomLocation();
                    timeToChangeState = Time.time + stateChangeDelay;
                    state = CatState.GotoRandomLocation;
                }
                target = catLoc + (catLoc - dogLoc) * movementSpeed / (catLoc - dogLoc).magnitude;
                break;
        }
        UpdateAnimator();

        if (state != CatState.Idle) {
            sprite = spriteRenderer.sprite;
        }
    }

    public void LateUpdate() {
        if (state == CatState.Idle) {
            spriteRenderer.sprite = sprite;
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
        //TODO change the cat bool flags based on it's current x and y velocity
        //query rb.velocity.x and rb.velocity.y
        Vector2 distance = target - (Vector2)transform.position;
        if (state == CatState.GotoRandomLocation)
        {
            rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? movementSpeed / distance.magnitude : 0);
        }
        else
        {
            rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? 1.6f * movementSpeed / distance.magnitude : 0);
        }

         anim.SetFloat("Vertical", rb.velocity.y);
         anim.SetFloat("Horizontal", rb.velocity.x);
         anim.SetFloat("Magnitude", rb.velocity.magnitude);
    }

    public CatState GetCatState() {
        return state;
    }
}