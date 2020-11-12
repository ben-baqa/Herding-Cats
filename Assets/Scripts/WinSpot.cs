using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class WinSpot : MonoBehaviour
{

    public float timerMilestone;

    public GameObject wolfPrefab;

    public GameObject catPrefab;
    public GameObject orangeCatPrefab;
    public GameObject orangeStripedCatPrefab;
    public GameObject stripedCatPrefab;
    public float catSpawnRadius = 3f;
    public int catCount = 1;
    private GameObject[] allCats;
    public List<GameObject> containedCats;
    public Text catLabel;
    public Text timerLabel;
    public Text timeLeftLabel;
    public GameObject fadeOut;
    public GameObject progressBar;

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
        timerMilestone = 25;
        player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < catCount; i++)
        {
            float rand = Random.Range(0f, 1f);
            GameObject instance;
            if (rand < 0.3f) {
                instance = GameObject.Instantiate(catPrefab);
            } else if (rand > 0.25f && rand <= 0.50f) {
                instance = GameObject.Instantiate(stripedCatPrefab);
            } else if (rand > 0.5f && rand < 0.75f) {
                instance = GameObject.Instantiate(orangeStripedCatPrefab);
            } else {
                instance = GameObject.Instantiate(orangeCatPrefab);
            }
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

        timer += Time.deltaTime * (1f / Mathf.Pow(2, allCats.Length - containedCats.Count));


        catLabel.text = containedCats.Count + " / " + allCats.Length;

        timerLabel.text = Mathf.Floor(timer) + "%";
        timeLeftLabel.text = Mathf.Ceil(timeLeft) + "s Left";

        progressBar.transform.localScale = new Vector2(timer * 1.28f, 1);

        if (timer > timerMilestone) {
            if (GameObject.FindGameObjectWithTag("Wolf") == null)
            {
                GameObject wolf = Instantiate(wolfPrefab);
                wolf.GetComponent<WolfMovement>().dog = player;
                wolf.transform.position = wolf.GetComponent<WolfMovement>().GetRandomSpotOutsideCamera();
            }
            timerMilestone += 25;
        }

        if(timer >= 100) {
            hasWon = true;
        }
    }
}