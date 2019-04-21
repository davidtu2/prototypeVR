using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Needed to reset game
using UnityEngine.UI; //Needed to access Text

//This class handles all of the UI of the player as well as his state
public class ARMSManager : MonoBehaviour {
    private GameObject ARMS;
    private GameObject hand;
    private Animator animatorARMS;

    //private Canvas UI; //This is a canvas component. It's game object could be used instead
    private GameObject UI;
    private GameObject panel;
    private Text doorState;
    private Text punchCounter;
    public int energy;

    private void Start () {
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        hand = GameObject.FindGameObjectWithTag("Hand");
        animatorARMS = ARMS.GetComponent<Animator>();
        //UI = GameObject.FindGameObjectWithTag("InteractionUI").GetComponent<Canvas>();
        UI = GameObject.FindGameObjectWithTag("InteractionUI");
        panel = UI.transform.Find("Panel").gameObject;
        Debug.Log(panel);

        //Perform DFS to find each of the Text components
        foreach (Text text in UI.GetComponentsInChildren<Text>()){
            if (text.gameObject != gameObject){
                switch (text.gameObject.name){
                    case "DoorState":
                        doorState = text;
                        break;
                    case "PunchCounter":
                        punchCounter = text;
                        break;
                    default:
                        Debug.Log("Text couldn't be found");
                        break;
                }
            }
        }

        setPanel(false);
        energy = 20;
        setPunchCounter();
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
        //Perfrom the action only if the user has enough energy to do so
        if (energy > 0){
            if (!animatorARMS.GetCurrentAnimatorStateInfo(0).IsName(state)){
                animatorARMS.SetTrigger(state);

                //Everytime the user performs an action, he will use energy
                energy -= 1;
                setPunchCounter();
            }

            //If the user runs out of energy, the game will restart
            if (energy <= 0){
                SceneManager.LoadScene("MyLevel");
            }
        }
    }

    private void setPunchCounter(){
        punchCounter.text = "Punch Left: " + energy.ToString();
    }

    public void setPanel(bool state){
        if (UI != null){
            //UI.rootCanvas.enabled = state;
            panel.SetActive(state);
        }
    }

    public void setDoorStateText(string state){
        doorState.text = state;
    }

    public Canvas getCanvas(){
        return UI.GetComponent<Canvas>();
        //return UI;
    }
}
