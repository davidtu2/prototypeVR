using UnityEngine;
using System.Collections;

public class ARMSController : MonoBehaviour{
    private ARMSManager manager;

    private void Start(){
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<ARMSManager>();
    }

    // Update is called once PER FRAME
    private void Update(){
        if (Input.GetKey("1")){
            manager.protag("Idle");
        }

        if (Input.GetKey("2")){
            manager.protag("Jump");
        }

        if (Input.GetKey("3")){
            manager.protag("Attack");
        }

        if (Input.GetKey("4")){
            manager.protag("Push");
        }

        if (Input.GetKey("5")){
            manager.protag("Run");
        }

        if (Input.GetKey("6")){
            manager.protag("Throw");
        }
    }
}