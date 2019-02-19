using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoor : MonoBehaviour {
    //Animators
	private Animator animatorDoor;
    private GameObject ARMS; //Game object for the ARMS. Need it to access it's animator
    private Animator animatorARMS;

    //Determines which direction the player is facing the door
    //For instance, if the player is exiting, he is facing the BACK of the door
    bool exiting;

    //Initialization
    void Start (){
		animatorDoor = transform.Find("Door_01").GetComponent<Animator>(); //Get the TRANSFORM's name from the tree
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        animatorARMS = ARMS.GetComponent<Animator>();
        exiting = false;
    }

    //Will be enabled if isTrigger = true
    void OnTriggerEnter (Collider other){
        //Case: The player is facing the front of the door
        //"Empty" means that the door is idle and "0" refers to the layer in the Animator. In this case, it is the Base Layer
        if (other.gameObject.tag == "Hand" &&
            animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty") && animatorARMS.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            //door("DoorOpen");
            OpenDoor();
            exiting = true;

            //Once the door opens, immediately stop the punching animation
            if (animatorARMS.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
                protag("Idle");
            }
        }

        //Case: The player is facing the back of the door and is waiting for it to re-open
        else if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty") && exiting){
            //door("DoorOpen");
            OpenDoor();
            exiting = false;
        }
	}

	void OnTriggerExit (Collider other){
        if (other.gameObject.tag == "Body" &&
            animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")){
            //door("DoorClose");
            OpenDoor();
        }
    }

	// Update is called once per frame
	void Update (){
        //Once the door opens, immediately stop the punching animation
        /*if (animatorARMS.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")){
            protag("Idle");
        }*/

        /*if (OpenTrigger) {
			OpenDoor();
			OpenTrigger = false;
		}

		//if (interactionUI.isActiveAndEnabled) {
			//if (Input.GetKeyDown (KeyCode.E)) {
				if (PlayerInCollider){
					OpenTrigger = true;
				}
			//}
		//}*/
    }

    void protag(string state){
        if (!animatorARMS.GetCurrentAnimatorStateInfo(0).IsName(state)){
            animatorARMS.SetTrigger(state);
        }
    }

    /*void door(string state){
        animatorDoor.SetTrigger(state);
    }*/

    void OpenDoor(){
        if (animatorDoor != null){
            //Will switch between true and false
            animatorDoor.SetBool("IsOpen", !animatorDoor.GetBool("IsOpen"));
        }
    }
}