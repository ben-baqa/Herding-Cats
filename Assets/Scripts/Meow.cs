

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Meow : MonoBehaviour
{
    public AudioClip[] meows;
    private AudioSource audioSource;
    private int lastMeow = -1;
    private int lastLastMeow = -1;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(MeowLoop());
    }
    IEnumerator MeowLoop() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            int currentMeow;
            do {
                currentMeow = Random.Range(0, meows.Length);
            } while(currentMeow == lastMeow || currentMeow == lastLastMeow);

            lastLastMeow = lastMeow;
            lastMeow = currentMeow;
            audioSource.PlayOneShot(meows[currentMeow]);
        }
    }
}
