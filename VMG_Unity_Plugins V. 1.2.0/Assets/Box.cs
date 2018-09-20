using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
    public float spd = 10f;
    private Rigidbody rb;
    private Vector3 input;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        //float moveZ = 1 * spd * Time.deltaTime;
        //rb.AddForce(0f, 0f, moveZ);
        //transform.Translate(0f, 0f, moveZ);
    }

    private void FixedUpdate(){
        //float moveZ = 1 * spd * Time.deltaTime;
        //rb.AddForce(0, 0, moveZ);
        //input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //rb.AddForce(input);
    }

    /*private void OnTriggerEnter(Collider other){
        Debug.Log("Entered into box");
    }

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.name == "Sphere"){
            //float moveZ = 1 * spd * Time.deltaTime;
            //rb.AddForce(0, moveZ, 0);
        }
    }*/
}
