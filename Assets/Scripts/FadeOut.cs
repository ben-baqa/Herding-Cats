using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour
{
    public int transitionFrames;
    public string scene;

    private SpriteRenderer rend;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        rend.color = Color.Lerp(Color.clear, Color.black, timer++ / transitionFrames);
        if (timer >= transitionFrames)
            SceneManager.LoadScene(scene);
    }
}
