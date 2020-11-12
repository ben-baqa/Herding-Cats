using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeveredFate : MonoBehaviour
{
    public RawImage severedFate;
    public bool hasDestroyedTimeline = false;
    private float timer = 0f;
    void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("!");
        if(!hasDestroyedTimeline && col.gameObject.tag == "Player" && severedFate.color.a <= 0) {
            Destroy(col.gameObject.GetComponent<DogMovement>());
            hasDestroyedTimeline = true;
            severedFate.color = new Color(1f, 1f, 1f, 0.01f);
        }
    }

    void Update() {
        if(!hasDestroyedTimeline) return;
        timer += Time.deltaTime;
        severedFate.color = Color.Lerp(new Color(1f, 1f, 1f, 0.0f), new Color(1f, 1f, 1f, 1f), Mathf.Clamp01(timer / 5f));
    }
}
