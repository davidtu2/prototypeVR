using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for the knockables
public class Switch : MonoBehaviour {
    private Switches manager;

    private bool fading;
    private float fadePerSecond = 0.5f;
    Material material;

    void Start () {
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<Switches>();

        fading = false;
        material = GetComponent<Renderer>().material;
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
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log(tag);
        manager.unlock(tag);

        //Start fading
        fading = true;
    }
}
