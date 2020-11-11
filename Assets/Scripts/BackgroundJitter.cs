using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundJitter : MonoBehaviour
{
    public List<Vector2> positions;
    public float timeInterval;
    private float timer;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeInterval)
        {
            timer = 0;
            index = (index + 1) % positions.Count;
            transform.position = positions[index];
        }
    }
}
