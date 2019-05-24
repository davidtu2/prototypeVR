//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public class MyVMGValues{
    //hand quaternion representing hand orientation
    public float q0h;
    public float q1h;
    public float q2h;
    public float q3h;

    //wrist quaternion representing wrist rotation
    public float q0w;
    public float q1w;
    public float q2w;
    public float q3w;

    //hand orientation
    public float rollH;
    public float pitchH;
    public float yawH;

    //wrist orientation
    public float rollW;
    public float pitchW;
    public float yawW;

    public int[] SensorValues = new int[Constants.NumSensors];//all values from dataglove sensors

    public int timestamp;//values representing package generation tick in ms

    public void ResetValues(){
        //reset all sensor values
        q0h = 0.0f;
        q1h = 0.0f;
        q2h = 0.0f;
        q3h = 0.0f;

        q0w = 0.0f;
        q1w = 0.0f;
        q2w = 0.0f;
        q3w = 0.0f;

        rollH = 0.0f;
        pitchH = 0.0f;
        yawH = 0.0f;

        rollW = 0.0f;
        pitchW = 0.0f;
        yawW = 0.0f;

        int i = 0;
        for (i = 0; i < Constants.NumSensors; i++){
            SensorValues[i] = 0;
        }

        timestamp = 0;
    }
}