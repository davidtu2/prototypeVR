using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public float spd = 5F;

	// Use this for initialization
	void Start () {
        //Turns off the cursor so you can't see it on the screen. Will also lock it so it will stay inside the game window
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        float forwards_backwards = Input.GetAxis("Vertical") * spd;
        float left_right = Input.GetAxis("Horizontal") * spd;

        //deltaTime = the time between Update calls
        forwards_backwards *= Time.deltaTime;
        left_right *= Time.deltaTime;

        //Since this script is attached to the Capsule, it will affect it's translation
        transform.Translate(left_right, 0, forwards_backwards);

        if (Input.GetKeyDown("escape")){
            //Turn off the cursor's locked state
            Cursor.lockState = CursorLockMode.None;
        }
	}
}
