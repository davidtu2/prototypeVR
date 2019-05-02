using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the camera script for the player
public class Eyes : MonoBehaviour {
    private Vector2 mouseLook; //Keeps track of the ACCUMULATED mouse movement
    private Vector2 smoothV; //Prevents the camera from "jerking"
    public float sensitivity = 2F; //How much you want to move the mouse on the screen
    public float smoothing = 1F;
    private GameObject character;

    private void Start () {
        //Init the character as the camera's parent (the player)
        character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
	private void Update () {
        var mouse_delta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        
        //Multiply the mouse_delta by the sensitivity and smoothing values
        mouse_delta = Vector2.Scale(mouse_delta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        
        //Lerp performs a linear interpolation, which will make the camera move smoothly between two points
        smoothV.x = Mathf.Lerp(smoothV.x, mouse_delta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouse_delta.y, 1f / smoothing);

        //ACCUMULATE the mouse movement
        mouseLook += smoothV;

        //Clamps up and down to prevent the camera from pitching 360 degrees
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        //Make a local rotation on the camera around it's right axis (which is it's x-axis). This makes the camera pitch
        //Using -mouseLook.y rather than a positive value gives the camera an inverted y-axis
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

        //Rotate around the CHARACTER's up since we want the character to turn around
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);
    }

    public void resetCamera(){
        mouseLook.x = 0;
        mouseLook.y = 0;
    }
}
