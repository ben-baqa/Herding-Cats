using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public AnimationCurve speedCurve;
    public GameObject dogBarkObject;
    private AudioSource bark;
    public Vector2 speed = new Vector2(5, 3.5f);
    private Vector2 dir = new Vector2(-1, 0);
    Animator animator;
    Rigidbody2D rbody;

    private bool isBarking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        bark = GetComponent<AudioSource>();
    }

    IEnumerator Bark() {
        isBarking = true;
        bark.Play();
        var offset = Vector2.zero;
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(input.y < 0 && input.x != 0) {
            offset += Vector2.up * 0.5f;
        }
        if(input.y > 0) {
            offset += Vector2.down * 0.2f;
        }
        GameObject dogBarkAnim = GameObject.Instantiate(dogBarkObject, rbody.position + 0.75f * dir + offset, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        isBarking = false;
    }

    void Update()
    {

        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && !isBarking) {
            StartCoroutine(Bark());
        }

        if(isBarking) {
            rbody.velocity = Vector2.zero;

            // animator.SetFloat("Vertical", 0f);
            // animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Magnitude", 0f);
            
            return;
        }

        if(input.magnitude != 0)
            dir = input;

        float multiplier = 1f;
        if(input.x != 0 && input.y != 0) {
            multiplier = 1f / Mathf.Sqrt(2f);
        }

        var t = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // Debug.Log(t % 1.0f);
        rbody.velocity = new Vector2(input.x * speed.x, input.y * speed.y) * multiplier * speedCurve.Evaluate(t % 1.0f);

        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Magnitude", input.magnitude);

        // animator.GetCurrentAnimatorClipInfo()
    }

    public bool IsDogBarking() {
        return isBarking;
    }
}
