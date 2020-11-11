using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogMovement : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector3 movement = new Vector3(0.0f, 0.0f, 0.0f);

         if (Input.GetKey(KeyCode.A)) {
             rb.AddForce(Vector3.left);
             movement.x = -1.0f;
         }
         if (Input.GetKey(KeyCode.D)) {
             rb.AddForce(Vector3.right);
             movement.x = 1.0f;
         }
         if (Input.GetKey(KeyCode.W)) {
             rb.AddForce(Vector3.up);
             movement.y = 1.0f;
         }
         if (Input.GetKey(KeyCode.S)) {
             rb.AddForce(Vector3.down);  
             movement.y = -1.0f;
         }

         animator.SetFloat("Vertical", movement.y);
         animator.SetFloat("Horizontal", movement.x);
         animator.SetFloat("Magnitude", movement.magnitude);
    }
}
