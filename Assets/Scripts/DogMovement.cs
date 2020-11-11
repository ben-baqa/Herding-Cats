using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public float speed = 5;
    private Vector2 dir = new Vector2(-1, 0);
    Animator animator;
    Rigidbody2D rbody;

    void Start()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if(input.magnitude != 0)
            dir = input;

        rbody.velocity = input * speed;

        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Magnitude", dir.magnitude);
    }
}
