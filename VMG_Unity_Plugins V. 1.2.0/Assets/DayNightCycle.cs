using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour {
    public float speed;

    void Start(){
        speed = 20f; // Controls the speed of the sun and moon
    }

    void Update () {
        transform.Rotate(speed * Time.deltaTime, 0, 0); //Can only rotate on the x or z axis
	}
}
