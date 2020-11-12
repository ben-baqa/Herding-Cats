using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinSpot : MonoBehaviour
{
    public GameObject catPrefab;
    public float catSpawnRadius = 3f;
    public int catCount = 1;
    private GameObject[] allCats;
    public List<GameObject> containedCats;
    public Text catLabel;
    public Text timerLabel;
    public Text timeLeftLabel;
    public GameObject fadeOut;

    public float timer = 0f;
    public float timeLeft = 300f;
    private GameObject player;
    // The time all the cats have to be in the circle for
    public float timeToWin = 10;
    public bool hasWon = false;

    public static float CircleProbability() {
        float y = Random.Range(0f, 1f);
        return Mathf.Sqrt(1 - y * y);
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < catCount; i++)
        {
            var instance = GameObject.Instantiate(catPrefab);
            instance.transform.position = Random.insideUnitCircle * catSpawnRadius;
            var cat = instance.GetComponent<CatMovement>();
            cat.dog = player;
            cat.annoyingness = 1.2f - CircleProbability();
        }
        allCats = GameObject.FindGameObjectsWithTag("Cat");
    }
    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Cat") {
            containedCats.Add(col.gameObject);
            col.gameObject.GetComponent<CatMovement>().insideZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.tag == "Cat") {
            containedCats.Remove(col.gameObject);
            col.gameObject.GetComponent<CatMovement>().insideZone = false;
        }
    }

    void Update() {
        if(hasWon) return;
        timeLeft -= Time.deltaTime;
        // if(!hasWon && allCats.Length == containedCats.Count) {
        timer += Time.deltaTime * (1f / Mathf.Pow(2, allCats.Length - containedCats.Count));

            // if(timer >= timeToWin) {
            //     hasWon = true;
            // }
        // }
        // else {
        //     timer = 0f;
        // }

        catLabel.text = containedCats.Count + " / " + allCats.Length;

        // timerLabel.gameObject.SetActive(!hasWon && allCats.Length == containedCats.Count && (timer % 1.0f < 0.75f));
        // timerLabel.text = Mathf.Ceil(10f - timer) + "s";
        timerLabel.text = Mathf.Floor(timer) + "%";
        timeLeftLabel.text = Mathf.Ceil(timeLeft) + "s Left";

        if(timer >= 10) {
            hasWon = true;
            Instantiate(fadeOut).GetComponent<FadeOut>().scene = "You Win";
        }
    }
}