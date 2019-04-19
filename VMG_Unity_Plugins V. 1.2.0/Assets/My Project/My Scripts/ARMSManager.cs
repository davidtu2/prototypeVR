using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We need to ensure that we can change a scene using script
using UnityEngine.SceneManagement;

public class ARMSManager : MonoBehaviour {
    private GameObject ARMS;
    private GameObject hand;
    private Animator animatorARMS;
    public int energy;

    // Use this for initialization
    private void Start () {
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        hand = GameObject.FindGameObjectWithTag("Hand");
        animatorARMS = ARMS.GetComponent<Animator>();
        energy = 10;
    }
	
	// Update is called once per frame
	private void Update () {
        if (isState("Attack")){
            if (hand.activeSelf == false){
                hand.SetActive(true);
                //energy -= 1;
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
        //Perfrom the action only if the user has enough energy to do so
        if (energy > 0){
            if (!animatorARMS.GetCurrentAnimatorStateInfo(0).IsName(state)){
                animatorARMS.SetTrigger(state);

                //Everytime the user performs an action, he will use energy
                energy -= 1;
            }

            if (energy <= 0){
                Debug.Log("Game Over");
                //UnityEngine.SceneManagement.SceneManager.LoadScene("MyLevel");
                SceneManager.LoadScene("MyLevel");
            }
        }
    }
}
