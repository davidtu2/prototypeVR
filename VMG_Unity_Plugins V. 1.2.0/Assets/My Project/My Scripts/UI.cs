//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Needed to access Text

public class UI : MonoBehaviour {
    private Image panel;
    private Text doorState;
    private Text winMsg;
    private Text punchCounter;

    private void Awake(){
        GameObject[] objs = GameObject.FindGameObjectsWithTag("InteractionUI");

        if (objs.Length > 1){
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start(){
        panel = transform.Find("Panel").gameObject.GetComponent<Image>();

        //Find each of the Text components
        foreach (Text text in GetComponentsInChildren<Text>()){
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


    }

    //The Switches script also uses this
    public void setPanel(string panelType, bool state){
        if (this.gameObject != null){
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

    public void setPunchCounter(int energy){
        punchCounter.text = "Punches Left: " + energy.ToString();
    }

    public void setDoorStateText(string state){
        doorState.text = state;
    }

    //This is used in the MyDoor script
    public Canvas getCanvas(){
        return GetComponent<Canvas>();
    }
}