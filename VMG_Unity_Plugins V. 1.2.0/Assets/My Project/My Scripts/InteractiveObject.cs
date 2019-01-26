﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Treat this as a COMPONENT of the attached object (i.e. like a new instance of a class)
//This script is attached to Door Pivot
public class InteractiveObject : MonoBehaviour {
    public enum state{
        active, //Open
        inactive //Close
    }

    public GameObject interactiveObject;
    Animator controller;
    private state current;

    private void Start(){
        current = state.inactive;
        interactiveObject = GameObject.FindGameObjectWithTag("DoorPivot");
        controller = interactiveObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other){ //Seems to only work if I set isTrigger = True
        if (other.gameObject.tag == "Hand"){
            //triggerInteractionLegacy(); //To use this, enable Animation and disable the Animator
            triggerInteraction();
        }
    }

    public void triggerInteraction(){
        if (controller.GetCurrentAnimatorStateInfo(0).IsName("DoorIdle")){ //"0" refers to the layer in the Animator. In this case, it is the Base Layer
            doors("Open");
        } else if (controller.GetCurrentAnimatorStateInfo(0).IsName("DoorOpen")){
            doors("Close");
        }
    }

    public void triggerInteractionLegacy(){
        if (!interactiveObject.GetComponent<Animation>().isPlaying){ //This check is necessary to prevent the object from jittering
                switch (current){
                case state.active:
                    interactiveObject.GetComponent<Animation>().Play("Close");
                    current = state.inactive;
                    break;

                case state.inactive:
                    interactiveObject.GetComponent<Animation>().Play("Open");
                    current = state.active;
                    break;

                default:
                    break;
            }
        }
    }

    void doors(string current){
        Debug.Log(controller);
        controller.SetTrigger(current);
    }
}