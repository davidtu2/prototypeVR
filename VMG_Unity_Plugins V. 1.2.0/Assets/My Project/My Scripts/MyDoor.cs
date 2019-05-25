//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class MyDoor : MonoBehaviour {
    public string nametag;
    private Animator animatorDoor;
    public bool locked;
    private UI UI;

    private void Start (){
        //This gets the Transform with the specified name from the object hierarchy
        animatorDoor = transform.Find("Door_01").GetComponent<Animator>();
        UI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<UI>();
        locked = true;
        nametag = gameObject.tag;
    }

	private void OnTriggerExit (Collider other){
        if (other.gameObject.tag == "Body" && animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("OpenDoor")){
            toggleDoorState();
        }
    }

    //Used to check if an object is already inside the trigger zone
    private void OnTriggerStay(Collider other){
        //CASE: Hand is detected within the trigger for some time
        if (other.gameObject.tag == "Hand"){
            //"Empty" means that the door is idle and "0" refers to the layer in the Animator. In this case, it is the Base Layer
            if (animatorDoor.GetCurrentAnimatorStateInfo(0).IsName("Empty") && !locked && !isBehindDoor(other.transform.position) && !animatorDoor.GetBool("IsOpen")){
                toggleDoorState();
            }
        }

        //CASE: If the player is within the trigger and the door is closed
        if (other.gameObject.tag == "Body" && !animatorDoor.GetBool("IsOpen")){
            if (!locked){
                //SUB-CASE: if the player is in front of an unlocked door
                if (!isBehindDoor(other.transform.position)) {
                    Ray ray = new Ray(other.transform.position, other.transform.forward);
                    if (isFacingDoor(ray)) {
                        UI.setDoorStateText("Punch the door to open");
                        UI.setPanel("Door", true);
                    } else {
                        UI.setPanel("Door", false);
                    }

                //SUB-CASE: if the player is behind an unlocked door. I doesn't matter if the player is facing it, the door will open
                } else {
                    toggleDoorState();
                }

            } else {
                //SUB-CASE: When the player is in front of a locked door
                if (!isBehindDoor(other.transform.position)){
                    Ray ray = new Ray(other.transform.position, other.transform.forward);
                    if (isFacingDoor(ray)){
                        UI.setDoorStateText("The door is locked");
                        UI.setPanel("Door", true);
                    } else {
                        UI.setPanel("Door", false);
                    }
                
                // SUB-CASE: When the player is behind a locked door
                } else {
                    //Debug.DrawRay(other.transform.position, other.transform.forward * 5f, Color.blue); //Draws a SCALED ray

                    //This ray will be used to contruct another ray
                    Ray ray0 = new Ray(other.transform.position, other.transform.forward);
                    Vector3 endpoint = ray0.origin + ray0.direction * .8f;
                    //Debug.Log("The endpoint is: " + endpoint);
                    //Debug.DrawRay(endpoint, -other.transform.forward * 10f, Color.red);

                    //Only perform the raycast when the endpoint is in front of the door. Think of the endpoint as a player
                    if (!isBehindDoor(endpoint)){
                        Ray ray = new Ray(endpoint, -other.transform.forward);
                        if (isFacingDoor(ray)){
                            UI.setDoorStateText("The door is locked");
                            UI.setPanel("Door", true);
                        } else {
                            Debug.Log("The raycast isn't hitting " + nametag);
                            UI.setPanel("Door", false);
                        }
                    } else {
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

    private bool isFacingDoor(Ray ray){
        RaycastHit hit;
        float distance = 5;

        if (Physics.Raycast(ray, out hit, distance) && hit.collider.gameObject.CompareTag(nametag)){
            //Debug.Log("TAG: " + hit.collider.gameObject.tag);
            //Debug.Log("NAME: " + hit.collider.gameObject.name);
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