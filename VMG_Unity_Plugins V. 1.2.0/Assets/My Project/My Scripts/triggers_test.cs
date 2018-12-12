using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggers_test : MonoBehaviour {
    public GameObject arms;

	// Use this for initialization
	void Start () {
        //MeshRenderer meshRend = GetComponent<MeshRenderer>();
        //meshRend.material.color = Color.blue;
        arms = GameObject.FindGameObjectWithTag("ArmsObject1");
        //Debug.Log("arms is: " + arms.name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other){
        // Change the cube color to green.
        if (other.name == "Cylinder"){
            //MeshRenderer meshRend = GetComponent<MeshRenderer>();
            //meshRend.material.color = Color.green;
            //Debug.Log(other.name);
            arms.GetComponent<Animation>().Play("FPArms_Unarmed_Punch");
        }
    }

    private void OnTriggerExit(Collider other){
        // Revert the cube color to white.
        //MeshRenderer meshRend = GetComponent<MeshRenderer>();
        //meshRend.material.color = Color.white;
        arms.GetComponent<Animation>().Play("FPArms_Unarmed_Idle");
    }
}
