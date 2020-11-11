using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithSine : MonoBehaviour
{
    public float xMod, yMod;

    private Vector2 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + new Vector2(xMod * Mathf.Sin(Time.time * 2), yMod * Mathf.Cos(Time.time * 3));
    }
}
