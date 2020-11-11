using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{
    public Vector2 xRange;
    public Vector2 yRange;
    public GameObject[] grassPrefabs;
    public List<GameObject> grasses;
    public int grassCount;
    void Start() {
        for (int i = 0; i < grassCount; i++)
        {
            var instance = GameObject.Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Length - 1)]);
            Vector2 temp;
            int j = 0;
            do {
                j++;
                instance.transform.position = new Vector2(Random.Range(xRange.x, xRange.y), Random.Range(yRange.x, yRange.y));
                temp = instance.transform.position;
                temp.y *= 2f;

                bool shouldContinue = false;

                if(Vector2.Distance(temp, Vector2.zero) < 1f) {
                    shouldContinue = true;
                }

                foreach (var g in grasses)
                {
                    if(Vector2.Distance(g.transform.position, temp) < 1f) {
                        shouldContinue = true;
                    }
                }

                if(!shouldContinue) {
                    grasses.Add(instance);
                    break;
                }

            } while(j <= 50);
            
        }
    }
}
