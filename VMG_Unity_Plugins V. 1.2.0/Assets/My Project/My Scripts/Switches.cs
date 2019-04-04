using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switches : MonoBehaviour {
    //Alternative to having EACH switch maintain a collection of MyDoors
    private MyDoor door1;
    private MyDoor door2;
    private MyDoor door3;
    private MyDoor door4;
    private MyDoor door5;
    private MyDoor door6;

    private GameObject switch1;
    private GameObject switch2;
    private GameObject switch3;
    private GameObject switch4;
    private GameObject switch5;
    private GameObject switch6;

	void Start () {
        door1 = GameObject.FindGameObjectWithTag("Door1").GetComponent<MyDoor>();
        door2 = GameObject.FindGameObjectWithTag("Door2").GetComponent<MyDoor>();
        door3 = GameObject.FindGameObjectWithTag("Door3").GetComponent<MyDoor>();
        door4 = GameObject.FindGameObjectWithTag("Door4").GetComponent<MyDoor>();
        door5 = GameObject.FindGameObjectWithTag("Door5").GetComponent<MyDoor>();
        door6 = GameObject.FindGameObjectWithTag("Door6").GetComponent<MyDoor>();

        switch1 = GameObject.FindGameObjectWithTag("Switch1");
        switch2 = GameObject.FindGameObjectWithTag("Switch2");
        switch3 = GameObject.FindGameObjectWithTag("Switch3");
        switch4 = GameObject.FindGameObjectWithTag("Switch4");
        switch5 = GameObject.FindGameObjectWithTag("Switch5");
        switch6 = GameObject.FindGameObjectWithTag("Switch6");

        //Swich 1 will make switch 2 appear and so on...
        switch2.SetActive(false);
        switch3.SetActive(false);
        switch4.SetActive(false);
        switch5.SetActive(false);
        switch6.SetActive(false);
    }

    public void unlock(string switchName){
        //Find out who is the requestor
        switch (switchName){
            case "Switch1":
                door1.unlock();
                switch2.SetActive(true);
                break;
            case "Switch2":
                door2.unlock();
                switch3.SetActive(true);
                break;
            case "Switch3":
                door3.unlock();
                switch4.SetActive(true);
                break;
            case "Switch4":
                door4.unlock();
                switch5.SetActive(true);
                break;
            case "Switch5":
                door5.unlock();
                switch6.SetActive(true);
                break;
            case "Switch6":
                door6.unlock();
                break;
            default:
                Debug.Log(switchName + "isn't associated with a door");
                break;
        }
    }
}