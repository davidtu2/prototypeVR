//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class MyDoor : MonoBehaviour {
    public string nametag;
    private Animator animatorDoor;
    public bool locked;
    private UI UI;

    private GameObject subDoor;

    private void Start (){
        //This gets the Transform with the specified name from the object hierarchy
        animatorDoor = transform.Find("Door_01").GetComponent<Animator>();
        UI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<UI>();
        locked = true;

        nametag = gameObject.tag;
        subDoor = transform.Find("Door_01").gameObject;
    }

    //This control struc be enabled if isTrigger = true
    private void OnTriggerEnter (Collider other){
        //"Empty" means that the door is idle and "0" refers to the layer in the Animator. In this case, it is the Base Layer
        /*if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty")) {
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
        }*/
	}

	private void OnTriggerExit (Collider other){
        if (UI.getCanvas() != null){
            UI.setPanel("Door", false);
        }

        if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")){
            toggleDoorState();
        }
    }

    //Used to check if an object is already inside the trigger zone
    private void OnTriggerStay(Collider other){
        if (other.gameObject.tag == "Hand"){ //Hand will generate upon punching
            if (animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty") && !locked && !isBehindDoor(other.transform.position) && !animatorDoor.GetBool("IsOpen")){
                toggleDoorState();
            }
        }

        //CASE: If the player is within the trigger zone and the door is closed
        if (other.gameObject.tag == "Body" && !animatorDoor.GetBool("IsOpen")){
            if (!locked){
                if (isFacingDoor(other.transform)){
                    if (!isBehindDoor(other.transform.position)){
                        UI.setDoorStateText("Punch the door to open");
                        UI.setPanel("Door", true);
                    } else {
                        //The player is facing the back of the door and is waiting for it to re-open
                        toggleDoorState();
                    }
                } else {
                    UI.setPanel("Door", false);
                }
            } else {
                if (!isBehindDoor(other.transform.position)){
                    if (isFacingDoor(other.transform)){
                        UI.setDoorStateText("The door is locked");
                        UI.setPanel("Door", true);
                    } else {
                        UI.setPanel("Door", false);
                    }
                } else {//TODO: Buggy: If player is behind a locked door and NOT facing it, the notification should disappear. Works with OnTriggerExit() to turn off the notification. Perhaps put a collider within the collider?
                    //UI.setDoorStateText("The door is locked");
                    //UI.setPanel("Door", true);
                    Debug.DrawRay(other.transform.position, other.transform.forward * 5f, Color.blue); //Draws a SCALED ray

                    Ray ray = new Ray(other.transform.position, other.transform.forward);
                    Vector3 endpoint = ray.origin + ray.direction * .8f;
                    //Debug.Log("The endpoint is: " + endpoint);
                    Debug.DrawRay(endpoint, -other.transform.forward * 10f, Color.red);

                    if (!isBehindDoor(endpoint)){
                        Ray ray2 = new Ray(endpoint, -other.transform.forward);
                        RaycastHit hit;
                        float distance = 5;
                        if (Physics.Raycast(ray2, out hit, distance))
                        {
                            Debug.Log("TAG: " + hit.collider.gameObject.tag);
                            //Debug.Log("NAME: " + hit.collider.gameObject.name);
                            if (hit.collider.gameObject.CompareTag(nametag))
                            {
                                UI.setDoorStateText("The door is locked");
                                UI.setPanel("Door", true);
                            }
                            else
                            {
                                UI.setPanel("Door", false);
                            }

                        }
                    } else{
                        UI.setPanel("Door", false);
                    }


                }
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

    private bool isFacingDoor(Transform player){
        float distance = 5;
        RaycastHit hit;

        if (Physics.Raycast(player.position, player.forward, out hit, distance) && hit.collider.gameObject.CompareTag(nametag)){
            Debug.DrawLine(player.position, hit.transform.position);
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