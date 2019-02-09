using UnityEngine;
using System.Collections;

public class ARMSController : MonoBehaviour{
    public GameObject arms1;
    //public GameObject arms1mesh;

    //For camera switching (was part of base code)
    //public Camera mainCameraGO;
    //public GameObject mainCameraSnapObject1;
    //public float cameraNearClipping = 0.05f;

    //Note that all existing texture maps are found in .../Texture Maps
    public Texture arms1Light;//FPArms_Female-Light_COL

    Animator animeARMS;

    void Start(){
        arms1 = GameObject.FindGameObjectWithTag("ArmsObject1"); //FPArms_Female_Light_LPR_LOD0
        //arms1mesh = GameObject.FindGameObjectWithTag("ArmsMesh1"); //FPArms_Female

        //This gets the Main Camera from the Scene
        //mainCameraGO = Camera.main;
        //mainCameraSnapObject1 = GameObject.FindGameObjectWithTag("mainCameraSnapObject1"); //snapObject1

        //Change attrs of mainCameraGO to that of mainCameraSnapObject's camera
        //As a result, the game defaults to FPArms_Female_Light_LPR_LOD0
        //mainCameraGO.transform.position = mainCameraSnapObject1.transform.position;
        //mainCameraGO.transform.rotation = mainCameraSnapObject1.transform.rotation;
        //mainCameraGO.nearClipPlane = cameraNearClipping;

        animeARMS = arms1.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        //These animations will loop
        if (Input.GetKey("1")){
            playIdle();
        }

        if (Input.GetKey("2")){
            playJump();
        }

        if (Input.GetKey("3")){
            playPunch();
        }

        if (Input.GetKey("4")){
            playPushDoor();
        }

        if (Input.GetKey("5")){
            playSprint();
        }

        if (Input.GetKey("6")){
            playThrow();
        }
    }

    public void playIdle(){
        //arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Idle");

        protag("Idle");
    }

    public void playJump(){
        //arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Jump");

        protag("Jump");
    }

    public void playPunch(){
        //arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Punch");

        protag("Attack");
    }

    public void playPushDoor(){
        //arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Push-Door");

        protag("Push");
    }

    public void playSprint(){
        //arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Sprint");

        protag("Run");
    }

    public void playThrow(){
        //arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Throw");

        protag("Throw");
    }

    /*private void OnTriggerEnter(Collider other){
        // Change the cube color to green.
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = Color.green;
        Debug.Log(other.name);
    }*/

    void protag(string state){
        animeARMS.SetTrigger(state);
    }
}