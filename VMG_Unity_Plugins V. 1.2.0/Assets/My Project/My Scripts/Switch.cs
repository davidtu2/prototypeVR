using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for the knockables
public class Switch : MonoBehaviour {
    private Switches manager;
    private bool fading;
    private float fadePerSecond = 0.5f;
    private Material material;
    private Transform trans;
    private Vector3 initialPos;

    private void Start () {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Switches>();
        fading = false;
        material = GetComponent<Renderer>().material;
        trans = GetComponent<Transform>();
        initialPos = trans.position;
    }

    private void Update(){
        if (fading){
            //Get a vector COPY of the current color vals at this time, then use them to calc a new alpha
            Color colorVect = material.color;
            float alpha = colorVect.a - (fadePerSecond * Time.deltaTime);

            if (alpha > 0){
                //Update color with the new vals
                material.color = new Color(colorVect.r, colorVect.g, colorVect.b, alpha);
            } else{
                gameObject.SetActive(false);
            }
        }

        //If the object moved due to physics forces
        if (initialPos != trans.position){
            manager.unlock(tag);
            
            //Start fading
            fading = true;
        }
    }
}
