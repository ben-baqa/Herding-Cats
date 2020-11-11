using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatState
{
    Idle,
    Walk,
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

    [Header("Behaviour")]
    public float annoyingness;

    [HideInInspector]
    public bool insideZone = false;
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
        state = CatState.Walk;
    }

    void AdjustTime() {
        timeToChangeState = Time.time + stateChangeDelay * Random.Range(0.8f, 1.2f);
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
            case CatState.Walk:
                target = currentRandomTarget;
                if (Time.time > timeToChangeState)
                {
                    AdjustTime();
                    //TODO set target to the randomly decided location
                    state = CatState.Idle;
                }
                break;

            case CatState.Idle:
                target = transform.position;
                if (Time.time > timeToChangeState)
                {
                    AdjustTime();
                    if(Random.Range(0f, 1f) < annoyingness) {
                        SetNewRandomLocation();
                        state = CatState.Walk;
                    }
                }
                break;

            case CatState.RunningAway:
                //if the cat is far enough away from teh dog, stop running and wander-
                if ((dogLoc - catLoc).magnitude > chaseThreshold)
                {
                    // SetNewRandomLocation();
                    AdjustTime();
                    state = CatState.Idle;
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
        if (distance.magnitude > 0)
        {
            if (state == CatState.Walk)
            {
                rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? movementSpeed / distance.magnitude : 0);
            }
            else
            {
                rb.velocity = distance * (distance.magnitude > movementSpeed / 10 ? 1.6f * movementSpeed / distance.magnitude : 0);
            }
        }
        else {
            rb.velocity = Vector2.zero;
        }

        switch (state)
        {
            case CatState.Walk:
                anim.SetFloat("SpeedMult", 0.5f);
                break;
            case CatState.RunningAway:
                anim.SetFloat("SpeedMult", 1.0f);
                break;
        }

         anim.SetFloat("Vertical", rb.velocity.y);
         anim.SetFloat("Horizontal", rb.velocity.x);
         anim.SetFloat("Magnitude", rb.velocity.magnitude);
    }

    public CatState GetCatState() {
        return state;
    }
}