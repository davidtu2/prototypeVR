using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARMSManager : MonoBehaviour {
    private GameObject ARMS;
    private GameObject hand;
    private Animator animatorARMS;

    // Use this for initialization
    private void Start () {
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        hand = GameObject.FindGameObjectWithTag("Hand");
        animatorARMS = ARMS.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	private void Update () {
        if (isState("Attack")){
            if (hand.activeSelf == false){
                hand.SetActive(true);
            }
        } else {
            if (hand.activeSelf == true){
                hand.SetActive(false);
            }
        }
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
