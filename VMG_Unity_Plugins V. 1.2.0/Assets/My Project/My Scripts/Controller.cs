using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public float spd = 3F;
    private ARMSManager manager;

    //Controller needs to remember the 1st instance of the manager to control it
    private void Awake(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Body");

        if (objs.Length > 1){
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start () {
        //Turns off the cursor so you can't see it on the screen. Will also lock it so it will stay inside the game window
        Cursor.lockState = CursorLockMode.Locked;

        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ARMSManager>();
    }
	
	// Update is called once per frame
	private void Update () {
        float forwards_backwards = Input.GetAxis("Vertical") * spd;
        float left_right = Input.GetAxis("Horizontal") * spd;

        //deltaTime = the time between Update calls
        forwards_backwards *= Time.deltaTime;
        left_right *= Time.deltaTime;

        //Since this script is attached to the Capsule, it will affect it's translation
        transform.Translate(left_right, 0, forwards_backwards);

        //Turn off the cursor's locked state
        if (Input.GetKeyDown("escape")){
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKey("1")){
            manager.protag("Idle");
        }

        if (Input.GetKey("2")){
            manager.protag("Jump");
        }

        if (Input.GetKey("3")){
            manager.protag("Attack");
        }

        if (Input.GetKey("4")){
            manager.protag("Push");
        }

        if (Input.GetKey("5")){
            manager.protag("Run");
        }

        if (Input.GetKey("6")){
            manager.protag("Throw");
        }
    }
}