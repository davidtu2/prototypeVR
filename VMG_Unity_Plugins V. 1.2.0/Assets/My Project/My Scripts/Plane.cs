﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the Plane object
public class Plane : MonoBehaviour {
    public GameObject arms;
    Animator animeARMS;

    // Use this for initialization
    void Start () {
        //MeshRenderer meshRend = GetComponent<MeshRenderer>();
        //meshRend.material.color = Color.blue;

        //This needs to be instantiated to access the arm's animations
        arms = GameObject.FindGameObjectWithTag("ArmsObject1");
        //Debug.Log("arms is: " + arms.name);
        animeARMS = arms.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other){
        // Change the cube color to green.
        if (other.gameObject.tag == "RightARM"){
            //MeshRenderer meshRend = GetComponent<MeshRenderer>();
            //meshRend.material.color = Color.green;
            //Debug.Log(other.name);
            //arms.GetComponent<Animation>().Play("FPArms_Unarmed_Punch");
            protag("Attack");
        }
    }

    private void OnTriggerExit(Collider other){
        // Revert the cube color to white.
        //MeshRenderer meshRend = GetComponent<MeshRenderer>();
        //meshRend.material.color = Color.white;
        if (other.gameObject.tag == "RightARM"){
            //arms.GetComponent<Animation>().Play("FPArms_Unarmed_Idle");
            protag("Idle");
        }
    }

    void protag(string state){
        if (!animeARMS.GetCurrentAnimatorStateInfo(0).IsName(state)){
            animeARMS.SetTrigger(state);
        }
    }
}
