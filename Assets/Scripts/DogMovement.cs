using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    public Vector2 speed = new Vector2(5, 3.5f);
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

        rbody.velocity = new Vector2(input.x * speed.x, input.y * speed.y);

        animator.SetFloat("Vertical", dir.y);
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Magnitude", input.magnitude);
    }
}
