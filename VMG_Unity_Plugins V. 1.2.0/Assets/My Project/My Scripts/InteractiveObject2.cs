﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Treat this as a COMPONENT of the attached object (i.e. like a new instance of a class)
public class InteractiveObject2 : MonoBehaviour{

    public GameObject interactiveObject;
    Animator anime;

    private void Start(){
        interactiveObject = GameObject.FindGameObjectWithTag("DoorController");
        anime = interactiveObject.GetComponent<Animator>();
    }

    //Seems to only work if I set isTrigger = True
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.tag == "Hand" && anime.GetCurrentAnimatorStateInfo(0).IsName("Door_Idle")){ //"0" refers to the layer in the Animator. In this case, it is the Base Layer
            doors("Open");
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.gameObject.tag == "Body" && anime.GetCurrentAnimatorStateInfo(0).IsName("Door_Open")){
            doors("Close");
        }
    }

    void doors(string state){
        anime.SetTrigger(state);
    }
}