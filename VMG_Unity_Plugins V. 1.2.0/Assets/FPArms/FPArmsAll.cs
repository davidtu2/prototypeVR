using UnityEngine;
using System.Collections;

public class FPArmsAll : MonoBehaviour {
    //decided not to use arrays or any more complicated shader manipulatons 
    //so any newbie that wants to see whats going on in this script can check the code and find related texture2D object

    //These arms will be init in Start()
    public GameObject arms1;
	public GameObject arms2;
	public GameObject arms3;
	public GameObject arms4;

    //These arm meshes will be init in Start()
    public GameObject arms1mesh; 
	public GameObject arms2mesh;
	public GameObject arms3mesh;
	public GameObject arms4mesh;

    //These cameras will be init in Start()
    public Camera mainCameraGO;
	public GameObject mainCameraSnapObject1;
	public GameObject mainCameraSnapObject2;
	public GameObject mainCameraSnapObject3;
	public GameObject mainCameraSnapObject4;

    //Note that all existing are texture maps are found in .../Texture Maps
    public Texture arms1Light;//FPArms_Female-Light_COL

    //Dependant on ToggleCamoCycle()
	public Texture arms2Light;
	public Texture arms2Light2;

	public Texture arms3Light;

    //Dependant on ToggleCamoCycle()
    public Texture arms4Light;
	public Texture arms4Light2;

	public Texture arms1Medium;//FPArms_Female-Medium_COL
    public Texture arms2Medium;
	public Texture arms2Medium2;
	public Texture arms3Medium;
	public Texture arms4Medium;
	public Texture arms4Medium2;

	public Texture arms1Dark;//FPArms_Female-Dark_COL
	public Texture arms2Dark;
	public Texture arms2Dark2;
	public Texture arms3Dark;
	public Texture arms4Dark;
	public Texture arms4Dark2;

	public Texture arms1DarkBlue;//FPArms_Female-Dark_COL_blue
    public Texture arms1DarkPurple;//FPArms_Female-Dark_COL_purple
    public Texture arms1DarkRed;//FPArms_Female-Dark_COL_red
    public Texture arms1DarkWine;//FPArms_Female-Dark_COL_wine

    public Texture arms1LightBlue;//FPArms_Female-Light_COL_blue
    public Texture arms1LightPurple;//FPArms_Female-Light_COL_purple
    public Texture arms1LightRed;//FPArms_Female-Light_COL_red
    public Texture arms1LightWine;//FPArms_Female-Light_COL_wine

    public Texture arms1MediumBlue;//FPArms_Female-Medium_COL_blue
    public Texture arms1MediumPurple;//FPArms_Female-Medium_COL_purple
    public Texture arms1MediumRed;//FPArms_Female-Medium_COL_red
    public Texture arms1MediumWine;//FPArms_Female-Medium_COL_wine

    public Texture arms2DarkBlue;
	public Texture arms2DarkPurple;
	public Texture arms2DarkRed;
	public Texture arms2DarkWine;

	public Texture arms2LightBlue;
	public Texture arms2LightPurple;
	public Texture arms2LightRed;
	public Texture arms2LightWine;

	public Texture arms2MediumBlue;
	public Texture arms2MediumPurple;
	public Texture arms2MediumRed;
	public Texture arms2MediumWine;

	public Texture arms2DarkBlue2;
	public Texture arms2DarkPurple2;
	public Texture arms2DarkRed2;
	public Texture arms2DarkWine2;

	public Texture arms2LightBlue2;
	public Texture arms2LightPurple2;
	public Texture arms2LightRed2;
	public Texture arms2LightWine2;

	public Texture arms2MediumBlue2;
	public Texture arms2MediumPurple2;
	public Texture arms2MediumRed2;
	public Texture arms2MediumWine2;

	public GameObject popup;//informationPlate

	public int skinTone = 0;
	public int nailColor = 0;
	public int camoTone = 0;

	public float cameraNormalFov = 60f;
	public float cameraNearClipping = 0.05f;

	// Use this for initialization
	void Start () {
		mainCameraGO = Camera.main;
		popup = GameObject.FindGameObjectWithTag("InformationPlate");//informationPlate
        mainCameraSnapObject1 = GameObject.FindGameObjectWithTag("mainCameraSnapObject1");//snapObject1
        mainCameraSnapObject2 = GameObject.FindGameObjectWithTag("mainCameraSnapObject2");//snapObject2
        mainCameraSnapObject3 = GameObject.FindGameObjectWithTag("mainCameraSnapObject3");//snapObject3
        mainCameraSnapObject4 = GameObject.FindGameObjectWithTag("mainCameraSnapObject4");//snapObject4

        arms1 = GameObject.FindGameObjectWithTag("ArmsObject1");//FPArms_Female_Light_LPR_LOD0
        arms2 = GameObject.FindGameObjectWithTag("ArmsObject2");//FPArms_Female-Military_Light_LPR_LOD0
        arms3 = GameObject.FindGameObjectWithTag("ArmsObject3");//FPArms_Male_Light_LPR_LOD0
        arms4 = GameObject.FindGameObjectWithTag("ArmsObject4");//FPArms_Male_Military_Light_LPR_LOD0

        arms1mesh = GameObject.FindGameObjectWithTag("ArmsMesh1");//FPArms_Female
        arms2mesh = GameObject.FindGameObjectWithTag("ArmsMesh2");//FPArms_Female_Military
        arms3mesh = GameObject.FindGameObjectWithTag("ArmsMesh3");//FPArms_Male
        arms4mesh = GameObject.FindGameObjectWithTag("ArmsMesh4");//FPArms_Male_Military_LOD0

        //Change attrs of mainCameraGO to that of mainCameraSnapObject's camera
        //As a result, the game defaults to FPArms_Female_Light_LPR_LOD0
        mainCameraGO.transform.position = mainCameraSnapObject1.transform.position;
		mainCameraGO.transform.rotation = mainCameraSnapObject1.transform.rotation;
		mainCameraGO.nearClipPlane = cameraNearClipping;

		Debug.Log (mainCameraSnapObject1);
		Debug.Log (mainCameraSnapObject2);
		Debug.Log (mainCameraSnapObject3);
		Debug.Log (mainCameraSnapObject4);

		mainCameraGO.nearClipPlane = cameraNearClipping;
	}

	// Update is called once per frame
    //Only checks input
	void Update () {

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

        //These will switch cameras, which will essentially switch the arm models
		if (Input.GetKey("q")){
			switch1();//FPArms_Female_Light_LPR_LOD0
        }

		if (Input.GetKey("w")){
			switch2();//FPArms_Female-Military_Light_LPR_LOD0
        }

		if (Input.GetKey("e")){
			switch4();//FPArms_Male_Light_LPR_LOD0
        }

		if (Input.GetKey("r")){
			switch3();//FPArms_Male_Military_Light_LPR_LOD0
        }

        //The following will toggle various aspects of the model, essentially, "customizing" it
        //Change skin tone
		if (Input.GetKeyDown("a")){
			toggleColorCycle();
		}

        //Change nail color if the model is female
		if (Input.GetKeyDown("d")){
			toggleNailCycle();
		}

        //Change camo if the model is military
		if (Input.GetKeyDown("s")){
			toggleCamoCycle();
		}

		if (Input.GetKeyDown("0")){
            //First, check if the GameObject is active
            if (popup.activeInHierarchy){
			popup.SetActive(false);
			} else {
                popup.SetActive(true);
            }
		}
	}

	void toggleColorCycle(){
		skinTone++;

		if (skinTone >= 3){
			skinTone = 0;
		}

		switch (skinTone){
		case 0:
			toggleColor0();
			break;
		case 1:
			toggleColor1();
			break;
		case 2:
			toggleColor2();
			break;
		case 3:
			skinTone = 0;
			break;
		}
	}

	void toggleCamoCycle(){
		camoTone++;

		if (camoTone >= 2){
			camoTone = 0;
		}

        //Change the textures
		if (camoTone == 1){
			arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Light2;//FPArms_Female_Military...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Light2;//FPArms_Male_Military_LOD0...
        }

		else if (camoTone == 0){
			arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Light;//FPArms_Female_Military...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Light;//FPArms_Male_Military_LOD0...
        }
	}

	void toggleNailCycle(){
		nailColor++;

		if (nailColor >= 5){
			nailColor = 0;
		}

		switch (nailColor){
		case 0:
			toggleNails0();
			break;
		case 1:
			toggleNails1();
			break;
		case 2:
			toggleNails2();
			break;
		case 3:
			toggleNails3();
			break;
		case 4:
			toggleNails4();
			break;
		case 5:
			nailColor = 0;
			break;
		}
	}

    //These animations will continuously loop
	public void playIdle(){
		arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Idle");
		arms2.GetComponent<Animation>().Play("FPArms_Unarmed_Idle");
		arms3.GetComponent<Animation>().Play("FPArms_Unarmed_Idle");
		arms4.GetComponent<Animation>().Play("FPArms_Unarmed_Idle");
	}

	public void playJump(){
		arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Jump");
		arms2.GetComponent<Animation>().Play("FPArms_Unarmed_Jump");
		arms3.GetComponent<Animation>().Play("FPArms_Unarmed_Jump");
		arms4.GetComponent<Animation>().Play("FPArms_Unarmed_Jump");
	}

	public void playPunch(){
		arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Punch");
		arms2.GetComponent<Animation>().Play("FPArms_Unarmed_Punch");
		arms3.GetComponent<Animation>().Play("FPArms_Unarmed_Punch");
		arms4.GetComponent<Animation>().Play("FPArms_Unarmed_Punch");
	}

	public void playPushDoor(){
		arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Push-Door");
		arms2.GetComponent<Animation>().Play("FPArms_Unarmed_Push-Door");
		arms3.GetComponent<Animation>().Play("FPArms_Unarmed_Push-Door");
		arms4.GetComponent<Animation>().Play("FPArms_Unarmed_Push-Door");
	}

	public void playSprint(){
		arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Sprint");
		arms2.GetComponent<Animation>().Play("FPArms_Unarmed_Sprint");
		arms3.GetComponent<Animation>().Play("FPArms_Unarmed_Sprint");
		arms4.GetComponent<Animation>().Play("FPArms_Unarmed_Sprint");
	}

	public void playThrow(){
		arms1.GetComponent<Animation>().Play("FPArms_Unarmed_Throw");
		arms2.GetComponent<Animation>().Play("FPArms_Unarmed_Throw");
		arms3.GetComponent<Animation>().Play("FPArms_Unarmed_Throw");
		arms4.GetComponent<Animation>().Play("FPArms_Unarmed_Throw");
	}

	public void toggleNails0(){
		if (skinTone == 0){//If the skin tone is light
            
            //Change textures
		    arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1Light;//FPArms_Female will use FPArms_Female-Light_COL
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Light;//FPArms_Female_Military...
            arms3mesh.GetComponent<Renderer>().material.mainTexture = arms3Light;//FPArms_Male...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Light;//FPArms_Male_Military_LOD0...

            //Check if there is camo
            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Light;//FPArms_Female_Military...
                arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Light;//FPArms_Male_Military_LOD0...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Light2;//FPArms_Female_Military...
                arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Light2;//FPArms_Male_Military_LOD0...
            }

		} else if (skinTone == 1){//If the skin tone is darker
            arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1Medium;//FPArms_Female will use FPArms_Female-Medium_COL
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Medium;//FPArms_Female_Military...
            arms3mesh.GetComponent<Renderer>().material.mainTexture = arms3Medium;//FPArms_Male...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Medium;//FPArms_Male_Military_LOD0...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Medium;//FPArms_Female_Military...
                arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Medium;//FPArms_Male_Military_LOD0...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Medium2;//FPArms_Female_Military...
                arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Medium2;//FPArms_Male_Military_LOD0...
            }

		} else if (skinTone == 2){//darkest
            arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1Dark;//FPArms_Female will use FPArms_Female-Dark_COL
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Dark;//FPArms_Female_Military...
            arms3mesh.GetComponent<Renderer>().material.mainTexture = arms3Dark;//FPArms_Male...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Dark;//FPArms_Male_Military_LOD0...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Dark;//FPArms_Female_Military...
                arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Dark;//FPArms_Male_Military_LOD0...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Dark2;//FPArms_Female_Military...
                arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Dark2;//FPArms_Male_Military_LOD0...
            }
		}
	}

	public void toggleNails1(){
		if (skinTone == 0){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1LightBlue;//FPArms_Female will use FPArms_Female-Light_COL_blue
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightBlue;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightBlue;//FPArms_Female_Military...

            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightBlue2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 1){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1MediumBlue;//FPArms_Female will use FPArms_Female-Medium_COL_blue
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumBlue;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumBlue;//FPArms_Female_Military...

            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumBlue2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 2){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1DarkBlue;//FPArms_Female will use FPArms_Female-Dark_COL_blue
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkBlue;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkBlue;//FPArms_Female_Military...

            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkBlue2;//FPArms_Female_Military...
            }
		}
	}

	public void toggleNails2(){
		if (skinTone == 0){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1LightPurple;//FPArms_Female will use FPArms_Female-Light_COL_purple
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightPurple;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightPurple;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightPurple2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 1){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1MediumPurple;//FPArms_Female will use FPArms_Female-Medium_COL_purple
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumPurple;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumPurple;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumPurple2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 2){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1DarkPurple;//FPArms_Female will use FPArms_Female-Dark_COL_purple
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkPurple;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkPurple;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkPurple2;//FPArms_Female_Military...
            }
		}
	}

	public void toggleNails3(){
		if (skinTone == 0){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1LightRed;//FPArms_Female will use FPArms_Female-Light_COL_red
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightRed;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightRed;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightRed2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 1){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1MediumRed;//FPArms_Female will use FPArms_Female-Medium_COL_red
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumRed;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumRed;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumRed2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 2){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1DarkRed;//FPArms_Female will use FPArms_Female-Dark_COL_red
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkRed;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkRed;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkRed2;//FPArms_Female_Military...
            }
		}
	}

	public void toggleNails4(){
		if (skinTone == 0){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1LightWine;//FPArms_Female will use FPArms_Female-Light_COL_wine
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightWine;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightWine;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2LightWine2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 1){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1MediumWine;//FPArms_Female will use FPArms_Female-Medium_COL_wine
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumWine;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumWine;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2MediumWine2;//FPArms_Female_Military...
            }
		}

        if (skinTone == 2){
			arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1DarkWine;//FPArms_Female will use FPArms_Female-Dark_COL_wine
            arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkWine;//FPArms_Female_Military...

            if (camoTone == 0){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkWine;//FPArms_Female_Military...
            } else if (camoTone == 1){
				arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2DarkWine2;//FPArms_Female_Military...
            }
		}
	}

    //These change skin tone
	public void toggleColor0(){
		arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1Light;//FPArms_Female will use FPArms_Female-Light_COL
        arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Light;//FPArms_Female_Military...
        arms3mesh.GetComponent<Renderer>().material.mainTexture = arms3Light;//FPArms_Male...
        arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Light;//FPArms_Male_Military_LOD0...

        if (camoTone == 1){
			arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Light2;//FPArms_Female_Military...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Light2;//FPArms_Male_Military_LOD0...
        }
	}

	public void toggleColor1(){
		arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1Medium;//FPArms_Female will use FPArms_Female-Medium_COL
        arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Medium;//FPArms_Female_Military...
        arms3mesh.GetComponent<Renderer>().material.mainTexture = arms3Medium;//FPArms_Male...
        arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Medium;//FPArms_Male_Military_LOD0...

        if (camoTone == 1){
			arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Medium2;//FPArms_Female_Military...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Medium2;//FPArms_Male_Military_LOD0...
        }
	}

	public void toggleColor2(){
		arms1mesh.GetComponent<Renderer>().material.mainTexture = arms1Dark;//FPArms_Female will use FPArms_Female-Dark_COL
        arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Dark;//FPArms_Female_Military...
        arms3mesh.GetComponent<Renderer>().material.mainTexture = arms3Dark;//FPArms_Male...
        arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Dark;//FPArms_Male_Military_LOD0...

        if (camoTone == 1){
			arms2mesh.GetComponent<Renderer>().material.mainTexture = arms2Dark2;//FPArms_Female_Military...
            arms4mesh.GetComponent<Renderer>().material.mainTexture = arms4Dark2;//FPArms_Male_Military_LOD0...
        }
	}

	public void switch1(){
		mainCameraGO.transform.position = mainCameraSnapObject1.transform.position;
		mainCameraGO.transform.rotation = mainCameraSnapObject1.transform.rotation;
		Debug.Log("switch Female");
	}

	public void switch2(){
		mainCameraGO.transform.position = mainCameraSnapObject2.transform.position;
		mainCameraGO.transform.rotation = mainCameraSnapObject2.transform.rotation;
		Debug.Log("switch Female Military");
	}

	public void switch3(){
		mainCameraGO.transform.position = mainCameraSnapObject3.transform.position;
		mainCameraGO.transform.rotation = mainCameraSnapObject3.transform.rotation;
		Debug.Log("switch Male");
	}
	public void switch4(){
		mainCameraGO.transform.position = mainCameraSnapObject4.transform.position;
		mainCameraGO.transform.rotation = mainCameraSnapObject4.transform.rotation;
		Debug.Log("switch Male Military");
	}
}