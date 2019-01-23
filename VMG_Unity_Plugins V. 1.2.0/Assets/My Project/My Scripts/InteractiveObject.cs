using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Treat this as a COMPONENT of the attached object (i.e. like a new instance of a class)
//This script is attached to Door Pivot
public class InteractiveObject : MonoBehaviour {

    //My own method
    public void triggerInteraction(){
        Debug.Log("Trigger Interaction has occured");
    }

    //Seems to only work if I set isTrigger = True
    private void OnTriggerEnter(Collider other){
        if (other.name == "Sphere"){
            triggerInteraction();
        }
    }
}
