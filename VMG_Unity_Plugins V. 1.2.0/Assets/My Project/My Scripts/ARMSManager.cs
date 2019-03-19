using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMSManager : MonoBehaviour {
    private GameObject ARMS;
    private Animator animatorARMS;

    // Use this for initialization
    void Start () {
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        animatorARMS = ARMS.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool isState(string state){
        if (animatorARMS.GetCurrentAnimatorStateInfo(0).IsName(state)) {
            return true;
        } else {
            return false;
        }
    }

    public void protag(string state){
        if (!animatorARMS.GetCurrentAnimatorStateInfo(0).IsName(state)){
            animatorARMS.SetTrigger(state);
        }
    }
}
