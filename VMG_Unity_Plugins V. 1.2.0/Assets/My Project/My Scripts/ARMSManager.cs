//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //For resetting

//This class handles all of the UI of the User as well as his state
public class ARMSManager : MonoBehaviour {
    private GameObject player;
    private Vector3 initPos;
    private Eyes cam;
    public int energy;
    private Animator animatorARMS;
    private GameObject ARMS;
    private SphereCollider hand;
    private UI UI;

    //For debugging. These will instantiated in the inspector instead of Start()
    public bool debug;
    public GameObject controller;
    public GameObject rightARM;
    public GameObject leftARM;
    public GameObject plane;

    private void Start () {
        player = GameObject.FindGameObjectWithTag("Body");
        initPos = player.transform.position;
        cam = player.GetComponentInChildren<Eyes>();
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        animatorARMS = ARMS.GetComponent<Animator>();
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<SphereCollider>();
        UI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<UI>();

        resetGame();

        //Debugging
        if (debug){
            //controller.GetComponent<SkinnedMeshRenderer>().enabled = true;
            //leftARM.GetComponent<MeshRenderer>().enabled = true;
            rightARM.GetComponent<MeshRenderer>().enabled = true;
            plane.GetComponent<MeshRenderer>().enabled = true;
        }
    }
	
	// Update is called once per frame
	private void Update () {
        //It is better to enable/disable components rather than enabling/disabling game objects,
        //as it destroys game objects and will lose the reference!
        if (isState("Attack")){
            if (hand.enabled == false){
                hand.enabled = true;
            }
        } else {
            if (hand.enabled == true){
                hand.enabled = false;
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
                UI.setPunchCounter(energy); //Updates display
            }

            //If the user runs out of energy, the game will restart
            if (energy <= 0){
                resetGame();
                SceneManager.LoadScene("MyLevel");
            }
        }
    }

    private void resetGame(){
        player.transform.position = initPos;
        cam.resetCamera();
        energy = 20;
        UI.setPunchCounter(energy);
        UI.setPanel("Door", false);
        UI.setPanel("Win", false);
    }
}