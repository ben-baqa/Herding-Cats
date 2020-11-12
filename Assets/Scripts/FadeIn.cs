using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public int transitionFrames;

    private SpriteRenderer rend;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        rend.color = Color.Lerp(Color.black, Color.clear, timer++ / transitionFrames);
        if (timer >= transitionFrames)
            Destroy(gameObject);
    }
}
