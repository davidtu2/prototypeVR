using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Needed to access Text

public class MyDoor : MonoBehaviour {
    private Animator animatorDoor;
    private ARMSManager manager;
    private Canvas UI;
    public bool locked;

    //Initialization
    private void Start (){
        //This gets the Transform with the specified name from the object hierarchy
        animatorDoor = transform.Find("Door_01").GetComponent<Animator>();
        //This gets the ARMSManager script
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ARMSManager>();

        UI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<Canvas>();
        if (UI != null){
            UI.rootCanvas.enabled = false;
        }

        locked = true;
    }

    //This control struc be enabled if isTrigger = true
    private void OnTriggerEnter (Collider other){
        //"Empty" means that the door is idle and "0" refers to the layer in the Animator. In this case, it is the Base Layer
        if (UI != null && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty")){
            if (!locked){
                if (!isBehindDoor(other.transform.position)){
                    UI.GetComponentInChildren<Text>().text = "Punch the door to open";
                    UI.rootCanvas.enabled = true;
                } else {
                    //The player is facing the back of the door and is waiting for it to re-open
                    if (other.gameObject.tag == "Body") {
                        toggleDoorState();
                    }
                }
            } else {
                UI.GetComponentInChildren<Text>().text = "The door is locked";
                UI.rootCanvas.enabled = true;
            }
        }
	}

	private void OnTriggerExit (Collider other){
        if (UI != null){
            UI.rootCanvas.enabled = false;
        }

        if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")){
            toggleDoorState();
        }
    }

    //Used to check if an object is already inside the trigger zone
    private void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "Hand"){
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