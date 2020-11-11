using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveAdjustment : MonoBehaviour
{
    
    /* This script ensures that "lower" objects are always in front
     * It sets their depth (Z) to be their height (Y)
     * This way, an object at height 1 will be behind an object of height 0 (higher value means further back)
     */

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}