using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Treat this as a COMPONENT of the attached object (i.e. like a new instance of a class)
public class InteractiveObject2 : MonoBehaviour{

    //public GameObject interactiveObject;
    Animator anime;
    //Game object for the ARMS. Needed to access it's animator component
    GameObject ARMS;
    Animator animeARMS;
    //Determines which direction the player is facing the door. For instance, if exiting, the player is facing the BACK of the door
    bool exiting;


    private void Start(){
        //interactiveObject = GameObject.FindGameObjectWithTag("Door");
        //anime = interactiveObject.GetComponent<Animator>();

        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        animeARMS = ARMS.GetComponent<Animator>();
        exiting = false;

        anime = GetComponent<Animator>();
    }

    //Seems to only work if I set isTrigger = True
    private void OnTriggerEnter(Collider other){
        //Case: The player is facing the front of the door
        if (other.gameObject.tag == "Hand" &&
            anime.GetCurrentAnimatorStateInfo(0).IsName("Door_Idle") && animeARMS.GetCurrentAnimatorStateInfo(0).IsName("Attack")){ //"0" refers to the layer in the Animator. In this case, it is the Base Layer
            door("Open");

            //Once the door opens, immediately stop the punching animation
            if (animeARMS.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
                protag("Idle");
            }

            exiting = true;
        //Case: The player is facing the back of the door
        } else if (other.gameObject.tag == "Body" && anime.GetCurrentAnimatorStateInfo(0).IsName("Door_Idle") && exiting){
            door("Open");
            exiting = false;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Body" && 
            anime.GetCurrentAnimatorStateInfo(0).IsName("Door_Open")){
            door("Close");
        }
    }

    void door(string state){
        anime.SetTrigger(state);
    }

    void protag(string state){
        animeARMS.SetTrigger(state);
    }
}