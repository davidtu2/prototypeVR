using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //For resetting
using UnityEngine.UI; //Needed to access Text

//This class handles all of the UI of the User as well as his state
public class ARMSManager : MonoBehaviour {
    //Arms
    private Animator animatorARMS;
    private GameObject ARMS;
    private SphereCollider hand;

    //UI
    private GameObject UI;
    private Image panel;
    private Text doorState;
    private Text winMsg;
    private Text punchCounter;

    //User properties
    private Transform player;
    private Vector3 initPos;
    public int energy;

    //For debugging. These will instantiated in the inspector instead of Start()
    public bool debug;
    public GameObject controller;
    public GameObject rightARM;
    public GameObject leftARM;

    private void Start () {
        //Arms
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        hand = GameObject.FindGameObjectWithTag("Hand").GetComponent<SphereCollider>();
        animatorARMS = ARMS.GetComponent<Animator>();

        //UI
        UI = GameObject.FindGameObjectWithTag("InteractionUI"); //Used to find Text components
        panel = UI.transform.Find("Panel").gameObject.GetComponent<Image>();

        //Find each of the Text components
        foreach (Text text in UI.GetComponentsInChildren<Text>()){
            if (text.gameObject != gameObject){
                switch (text.gameObject.name){
                    case "DoorState":
                        doorState = text;
                        break;
                    case "PunchCounter":
                        punchCounter = text;
                        break;
                    case "YouWin":
                        winMsg = text;
                        break;
                    default:
                        Debug.Log("Text couldn't be found");
                        break;
                }
            }
        }

        setPanel("Door", false);
        setPanel("Win", false);

        //User properties
        player = GameObject.FindGameObjectWithTag("Body").GetComponent<Transform>();
        initPos = player.position;
        energy = 20;
        setPunchCounter();

        //Debugging
        if (debug){
            controller.GetComponent<SkinnedMeshRenderer>().enabled = true;
            leftARM.GetComponent<MeshRenderer>().enabled = true;
            rightARM.GetComponent<MeshRenderer>().enabled = true;
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
                setPunchCounter(); //Updates display
            }

            //If the user runs out of energy, the game will restart
            if (energy <= 0){
                energy = 5;
                player.position = initPos;
                SceneManager.LoadScene("MyLevel");
            }
        }
    }

    private void setPunchCounter(){
        punchCounter.text = "Punches Left: " + energy.ToString();
    }

    public void setPanel(string panelType, bool state){
        if (UI != null){
            switch (panelType){
                case "Door": //Build a door panel
                    panel.enabled = state; //Enable the panel for the text
                    doorState.enabled = state; //Text should already have the latest info
                    break;
                case "Win": //Build a win message panel
                    panel.enabled = state;
                    winMsg.enabled = state;
                    break;
                default:
                    Debug.Log("The panel cannot be found");
                    break;
            }
        }
    }

    public void setDoorStateText(string state){
        doorState.text = state;
    }

    public Canvas getCanvas(){
        return UI.GetComponent<Canvas>();
    }
}