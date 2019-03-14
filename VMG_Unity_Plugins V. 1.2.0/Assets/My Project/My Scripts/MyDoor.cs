using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDoor : MonoBehaviour {
    //GameObject(s)
    private GameObject ARMS; //Needed to access it's animator component

    //Animators
    private Animator animatorDoor;
    private Animator animatorARMS;

    //Initialization
    void Start (){
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");

        animatorDoor = transform.Find("Door_01").GetComponent<Animator>(); //Get the Transform with the specified name from the object hierarchy
        animatorARMS = ARMS.GetComponent<Animator>();
    }

    void OnTriggerEnter (Collider other){ //Will be enabled if isTrigger = true
        //Case 1: The player is facing the front of the door
        //"Empty" means that the door is idle and "0" refers to the layer in the Animator. In this case, it is the Base Layer
        if (other.gameObject.tag == "Hand" &&
            animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty") && animatorARMS.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
            //door("DoorOpen");
            switchDoorState();

            //Once the door opens, immediately stop the punching animation
            /*if (animatorARMS.GetCurrentAnimatorStateInfo(0).IsName("Attack")){//TODO: Also check if the door is opened?
                protag("Idle");
            }*/
        }

        //Case 2: The player is facing the back of the door and is waiting for it to re-open
        else if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty") && 
            behindDoor(other.transform.position)){
            //door("DoorOpen");
            switchDoorState();
        }
	}

	void OnTriggerExit (Collider other){
        if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")){
            //door("DoorClose");
            switchDoorState();
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
        //Check if the player is already in the specified state, otherwise, switch to it
        if (!animatorARMS.GetCurrentAnimatorStateInfo(0).IsName(state)){
            animatorARMS.SetTrigger(state);
        }
    }

    /*void door(string state){
        //Check if the door is already in the specified state, otherwise, switch to it
        if (!animatorDoor.GetCurrentAnimatorStateInfo(0).IsName(state)){
            animatorDoor.SetTrigger(state);
        }
    }*/

    void switchDoorState(){
        if (animatorDoor != null){
            //Will switch between true and false
            animatorDoor.SetBool("IsOpen", !animatorDoor.GetBool("IsOpen"));
        }
    }

    bool behindDoor(Vector3 position){
        Vector3 toPlayer = (position - transform.position).normalized;

        Debug.Log(Vector3.Dot(toPlayer, transform.forward));

        //If the player is behind the door, the dot product should return any value between the range (0, -1]
        if (Vector3.Dot(toPlayer, transform.forward) < 0){
            return true;
        } else{
            return false;
        }
    }
}