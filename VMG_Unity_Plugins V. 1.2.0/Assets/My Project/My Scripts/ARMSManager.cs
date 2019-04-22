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
    private GameObject UI;
    private GameObject doorPanel;
    private GameObject winPanel;
    private Text doorState;
    private Text punchCounter;
    public int energy;

    private void Start () {
        ARMS = GameObject.FindGameObjectWithTag("ArmsObject1");
        hand = GameObject.FindGameObjectWithTag("Hand");
        animatorARMS = ARMS.GetComponent<Animator>();
        UI = GameObject.FindGameObjectWithTag("InteractionUI");
        doorPanel = UI.transform.Find("Panel").gameObject;
        winPanel = UI.transform.Find("WinPanel").gameObject;

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

        setPanel("Door", false);
        setPanel("Win", false);
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
        punchCounter.text = "Punches Left: " + energy.ToString();
    }

    public void setPanel(string panel, bool state){
        if (UI != null){
            switch (panel){
                case "Door":
                    doorPanel.SetActive(state);
                    break;
                case "Win":
                    winPanel.SetActive(state);
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
