using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0)){

            //Creates a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            //Create a raycast based on the variable, ray. If successfull the method will return true and also populate the variable, hit
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)){
                //Create an instance of my component (class) called "obj" and assign it to the collider's InteractiveObject (The collider that has been hit by "hit")
                InteractiveObject obj = hit.collider.GetComponent<InteractiveObject>();

                //This won't be called if the object the user's clicking doesn't have the InteractiveObject script component attached
                if (obj){
                    obj.triggerInteraction();
                }
            }
        }
	}
}
