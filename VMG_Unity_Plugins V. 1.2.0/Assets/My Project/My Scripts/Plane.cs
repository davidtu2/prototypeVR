using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour {
    private ARMSManager manager;

    //Initialization
    private void Start () {
        manager = GameObject.FindGameObjectWithTag("Status").GetComponent<ARMSManager>();
    }

    private void OnTriggerEnter(Collider other){
        //If the right arm collides with the plane
        if (other.gameObject.tag == "RightARM"){
            manager.protag("Attack");
        }
    }
}