//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class MyDoor : MonoBehaviour {
    private Animator animatorDoor;
    public bool locked;
    private UI UI;

    private void Start (){
        //This gets the Transform with the specified name from the object hierarchy
        animatorDoor = transform.Find("Door_01").GetComponent<Animator>();
        UI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<UI>();
        locked = true;
    }

    //This control struc be enabled if isTrigger = true
    private void OnTriggerEnter (Collider other){
        //"Empty" means that the door is idle and "0" refers to the layer in the Animator. In this case, it is the Base Layer
        if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty")) {
            if (!locked) {
                if (!isBehindDoor(other.transform.position)) {
                    if (UI.getCanvas() != null){
                        UI.setDoorStateText("Punch the door to open");
                        UI.setPanel("Door", true);
                    }
                } else {
                    //The player is facing the back of the door and is waiting for it to re-open
                    toggleDoorState();
                }
            } else {
                if (UI.getCanvas() != null){
                    UI.setDoorStateText("The door is locked");
                    UI.setPanel("Door", true);
                }
            }
        }
	}

	private void OnTriggerExit (Collider other){
        if (UI.getCanvas() != null && other.gameObject.tag == "Body"){
            UI.setPanel("Door", false);
        }

        if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")){
            toggleDoorState();
        }
    }

    //Used to check if an object is already inside the trigger zone
    private void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "Hand"){ //Hand will generate upon punching
            if (animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty") && !locked && !isBehindDoor(other.transform.position) && animatorDoor.GetBool("IsOpen") == false){
                toggleDoorState();
            }
        }
    }

    private void toggleDoorState(){
        if (animatorDoor != null){
            //Switches between true and false
            animatorDoor.SetBool("IsOpen", !animatorDoor.GetBool("IsOpen"));
        }

        if (UI.getCanvas() != null && animatorDoor.GetBool("IsOpen")){
            //Turn off the message if the door is open
            UI.setPanel("Door", false);
        }
    }

    private bool isBehindDoor(Vector3 position){
        //Vector pointing from the door to the player
        Vector3 toPlayer = (position - transform.position).normalized;
        //Debug.Log(Vector3.Dot(toPlayer, transform.forward));

        //If the player is behind the door, the dot product should return any value between the range (0, -1]
        if (Vector3.Dot(toPlayer, transform.forward) < 0){
            return true;
        } else {
            return false;
        }
    }

    public void unlock(){
        locked = false;
    }

    public void lockDoor(){
        locked = true;
    }
}