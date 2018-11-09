using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;
using UnityEngine;

namespace VML{
    public class VMG30_Controller : MonoBehaviour{
        private bool debugSkeletonLeft = true;
        private bool debugSkeletonRight = true;

        //For this machine, the correct ports are 5 and 4, respectively... Which is set in the INSPECTOR
        public int COMPORT_LeftGlove = 1;
        public int COMPORT_RightGlove = 2;

        //Root is common to both left and right hand. These will be used for future purposes (i.e. They are unused)
        public Transform root;
        public Transform spine;

        //right hand joints
        public Transform clavicleR;
        public Transform upperArmR;
        public Transform lowerArmR;
        public Transform handR;
        public Transform[] ThumbR = new Transform[3];
        public Transform[] IndexR = new Transform[3];
        public Transform[] MiddleR = new Transform[3];
        public Transform[] RingR = new Transform[3];
        public Transform[] LittleR = new Transform[3];

        //right hand fingers joint angles
        Vector3[] thumbFlexAnglesR = new Vector3[3];
        Vector3[] indexFlexAnglesR = new Vector3[3];
        Vector3[] middleFlexAnglesR = new Vector3[3];
        Vector3[] ringFlexAnglesR = new Vector3[3];
        Vector3[] littleFlexAnglesR = new Vector3[3];

        Vector3 handAnglesR = new Vector3(0, 0, 0);

        //left hand joints
        public Transform clavicleL;
        public Transform upperArmL;
        public Transform lowerArmL;
        public Transform handL;
        public Transform[] ThumbL = new Transform[3];
        public Transform[] IndexL = new Transform[3];
        public Transform[] MiddleL = new Transform[3];
        public Transform[] RingL = new Transform[3];
        public Transform[] LittleL = new Transform[3];

        //left hand fingers joint angles
        Vector3[] thumbFlexAnglesL = new Vector3[3];
        Vector3[] indexFlexAnglesL = new Vector3[3];
        Vector3[] middleFlexAnglesL = new Vector3[3];
        Vector3[] ringFlexAnglesL = new Vector3[3];
        Vector3[] littleFlexAnglesL = new Vector3[3];

        Vector3 handAnglesL = new Vector3(0, 0, 0);

        //communicaton between glove and unity
        VMG30_Driver gloveL = new VMG30_Driver();
        VMG30_Driver gloveR = new VMG30_Driver();

        // Use this for initialization
        void Start(){
            Debug.Log("Start\n");
            Debug.Log("Initial Left Glove Comport: " + COMPORT_LeftGlove + "\n");
            Debug.Log("Initial Right Glove Comport: " + COMPORT_RightGlove + "\n");

            gloveL.Init(COMPORT_LeftGlove, Constants.LeftHanded, Constants.PKG_QUAT_FINGER);
            gloveL.StartCommunication();

            gloveR.Init(COMPORT_RightGlove, Constants.RightHanded, Constants.PKG_QUAT_FINGER);
            gloveR.StartCommunication();

            Debug.Log(spine.name);
            Debug.Log(root.name);

            Debug.Log(clavicleR.tag);
            Debug.Log(upperArmR.tag);
            Debug.Log(lowerArmR.tag);
            Debug.Log(handR.tag);

            Debug.Log(clavicleL.tag);
            Debug.Log(upperArmL.tag);
            Debug.Log(lowerArmL.tag);
            Debug.Log(handL.tag);
        }

        void OnApplicationQuit(){
            Debug.Log("Close app\n");
            gloveR.StopCommunication();
            gloveL.StopCommunication();
        }

        //modulate (change?) the angle from sensor values (0 = valmin  1000 = valmax)
        float ModulateAngle(VMGValues values, int sensorindex, float valmin, float valmax){
            return valmin + (valmax - valmin) * values.SensorValues[sensorindex] / 1000.0f;
        }

        void UpdateHandAnglesLeft(VMGValues values){
            //thumb1_l
            thumbFlexAnglesL[0][0] = ModulateAngle(values, SensorIndexLeftHanded.AbdThumb, 50.0f, 90.0f); //roll over X is controller by abduction value 
            thumbFlexAnglesL[0][1] = ModulateAngle(values, SensorIndexLeftHanded.AbdThumb, -110.0f, -100.0f); //roll over X is controller by abduction value 
            thumbFlexAnglesL[0][2] = ModulateAngle(values, SensorIndexLeftHanded.PalmArch, -50.0f, -100.0f); //roll over X is controller by abduction value 

            //thumb2_l
            thumbFlexAnglesL[1][0] = 0.0f;
            thumbFlexAnglesL[1][1] = 0.0f;
            thumbFlexAnglesL[1][2] = ModulateAngle(values, SensorIndexLeftHanded.PalmArch, 10.0f, -30.0f); //roll over X is controller by abduction value 

            //thumb3_l
            thumbFlexAnglesL[2][0] = 0.0f;
            thumbFlexAnglesL[2][1] = 0.0f;
            thumbFlexAnglesL[2][2] = ModulateAngle(values, SensorIndexLeftHanded.ThumbPh2, 15.0f, -50.0f); //roll over X is controller by abduction value

            //index1_l
            indexFlexAnglesL[0][0] = 15.0f;
            indexFlexAnglesL[0][1] = ModulateAngle(values, SensorIndexLeftHanded.AbdIndex, 0.0f, -15.0f); //roll over X is controller by abduction value 
            indexFlexAnglesL[0][2] = ModulateAngle(values, SensorIndexLeftHanded.IndexPh1, 0.0f, -50.0f); //roll over X is controller by abduction value 

            //index2_l
            indexFlexAnglesL[1][0] = 0.0f;
            indexFlexAnglesL[1][1] = 0.0f;
            indexFlexAnglesL[1][2] = ModulateAngle(values, SensorIndexLeftHanded.IndexPh2, 0.0f, -110.0f); //roll over X is controller by abduction value 

            //index3_l
            indexFlexAnglesL[2][0] = 0.0f;
            indexFlexAnglesL[2][1] = 0.0f;
            indexFlexAnglesL[2][2] = ModulateAngle(values, SensorIndexLeftHanded.IndexPh2, 0.0f, -55.0f); //roll over X is controller by abduction value 

            //middle1_l
            middleFlexAnglesL[0][0] = 4.50f;
            middleFlexAnglesL[0][1] = 0.0f;
            middleFlexAnglesL[0][2] = ModulateAngle(values, SensorIndexLeftHanded.MiddlePh1, 0.0f, -50.0f); //roll over X is controller by abduction value 

            //middle2_l
            middleFlexAnglesL[1][0] = 0.0f;
            middleFlexAnglesL[1][1] = 0.0f;
            middleFlexAnglesL[1][2] = ModulateAngle(values, SensorIndexLeftHanded.MiddlePh2, 0.0f, -110.0f); //roll over X is controller by abduction value 

            //middle3_l
            middleFlexAnglesL[2][0] = 0.0f;
            middleFlexAnglesL[2][1] = 0.0f;
            middleFlexAnglesL[2][2] = ModulateAngle(values, SensorIndexLeftHanded.MiddlePh2, 0.0f, -55.0f); //roll over X is controller by abduction value 

            //ring1_l
            ringFlexAnglesL[0][0] = 5.0f;
            ringFlexAnglesL[0][1] = ModulateAngle(values, SensorIndexLeftHanded.AbdRing, 0.0f, 15.0f); //roll over X is controller by abduction value 
            ringFlexAnglesL[0][2] = ModulateAngle(values, SensorIndexLeftHanded.RingPh1, 0.0f, -50.0f); //roll over X is controller by abduction value 

            //ring2_l
            ringFlexAnglesL[1][0] = 0.0f;
            ringFlexAnglesL[1][1] = 0.0f;
            ringFlexAnglesL[1][2] = ModulateAngle(values, SensorIndexLeftHanded.RingPh2, 0.0f, -110.0f); //roll over X is controller by abduction value 

            //ring3_l
            ringFlexAnglesL[2][0] = 0.0f;
            ringFlexAnglesL[2][1] = 0.0f;
            ringFlexAnglesL[2][2] = ModulateAngle(values, SensorIndexLeftHanded.RingPh2, 0.0f, -55.0f); //roll over X is controller by abduction value 

            //little1_l
            littleFlexAnglesL[0][0] = 5.0f;
            littleFlexAnglesL[0][1] = ModulateAngle(values, SensorIndexLeftHanded.AbdLittle, 5.0f, 25.0f); //roll over X is controller by abduction value 
            littleFlexAnglesL[0][2] = ModulateAngle(values, SensorIndexLeftHanded.LittlePh1, 0.0f, -50.0f); //roll over X is controller by abduction value 

            //little2_l
            littleFlexAnglesL[1][0] = 0.0f;
            littleFlexAnglesL[1][1] = 0.0f;
            littleFlexAnglesL[1][2] = ModulateAngle(values, SensorIndexLeftHanded.LittlePh2, 0.0f, -110.0f); //roll over X is controller by abduction value 

            //little3_l
            littleFlexAnglesL[2][0] = 0.0f;
            littleFlexAnglesL[2][1] = 0.0f;
            littleFlexAnglesL[2][2] = ModulateAngle(values, SensorIndexLeftHanded.LittlePh2, 0.0f, -55.0f); //roll over X is controller by abduction value 
        }

        void UpdateHandAnglesRight(VMGValues values){
            //thumb1_l
            thumbFlexAnglesR[0][0] = ModulateAngle(values, SensorIndexRightHanded.AbdThumb, 50.0f, 90.0f); //roll over X is controller by abduction value 
            thumbFlexAnglesR[0][1] = ModulateAngle(values, SensorIndexRightHanded.AbdThumb, -110.0f, -100.0f); //roll over X is controller by abduction value 
            thumbFlexAnglesR[0][2] = ModulateAngle(values, SensorIndexRightHanded.PalmArch, -50.0f, -100.0f); //roll over X is controller by abduction value 

            //thumb2_l
            thumbFlexAnglesR[1][0] = 0.0f;
            thumbFlexAnglesR[1][1] = 0.0f;
            thumbFlexAnglesR[1][2] = ModulateAngle(values, SensorIndexRightHanded.PalmArch, 10.0f, -30.0f); //roll over X is controller by abduction value 

            //thumb3_l
            thumbFlexAnglesR[2][0] = 0.0f;
            thumbFlexAnglesR[2][1] = 0.0f;
            thumbFlexAnglesR[2][2] = ModulateAngle(values, SensorIndexRightHanded.ThumbPh2, 15.0f, -50.0f); //roll over X is controller by abduction value 

            //index1_l
            indexFlexAnglesR[0][0] = 15.0f;
            indexFlexAnglesR[0][1] = ModulateAngle(values, SensorIndexRightHanded.AbdIndex, 0.0f, -15.0f); //roll over X is controller by abduction value 
            indexFlexAnglesR[0][2] = ModulateAngle(values, SensorIndexRightHanded.IndexPh1, 0.0f, -50.0f); //roll over X is controller by abduction value 

            //index2_l
            indexFlexAnglesR[1][0] = 0.0f;
            indexFlexAnglesR[1][1] = 0.0f;
            indexFlexAnglesR[1][2] = ModulateAngle(values, SensorIndexRightHanded.IndexPh2, 0.0f, -110.0f); //roll over X is controller by abduction value 

            //index3_l
            indexFlexAnglesR[2][0] = 0.0f;
            indexFlexAnglesR[2][1] = 0.0f;
            indexFlexAnglesR[2][2] = ModulateAngle(values, SensorIndexRightHanded.IndexPh2, 0.0f, -55.0f); //roll over X is controller by abduction value 

            //middle1_l
            middleFlexAnglesR[0][0] = 4.50f;
            middleFlexAnglesR[0][1] = 0.0f;
            middleFlexAnglesR[0][2] = ModulateAngle(values, SensorIndexRightHanded.MiddlePh1, 0.0f, -50.0f); //roll over X is controller by abduction value 

            //middle2_l
            middleFlexAnglesR[1][0] = 0.0f;
            middleFlexAnglesR[1][1] = 0.0f;
            middleFlexAnglesR[1][2] = ModulateAngle(values, SensorIndexRightHanded.MiddlePh2, 0.0f, -110.0f); //roll over X is controller by abduction value 

            //middle3_l
            middleFlexAnglesR[2][0] = 0.0f;
            middleFlexAnglesR[2][1] = 0.0f;
            middleFlexAnglesR[2][2] = ModulateAngle(values, SensorIndexRightHanded.MiddlePh2, 0.0f, -55.0f); //roll over X is controller by abduction value 

            //ring1_l
            ringFlexAnglesR[0][0] = 5.0f;
            ringFlexAnglesR[0][1] = ModulateAngle(values, SensorIndexRightHanded.AbdRing, 0.0f, 15.0f); //roll over X is controller by abduction value 
            ringFlexAnglesR[0][2] = ModulateAngle(values, SensorIndexRightHanded.RingPh1, 0.0f, -50.0f); //roll over X is controller by abduction value 

            //ring2_l
            ringFlexAnglesR[1][0] = 0.0f;
            ringFlexAnglesR[1][1] = 0.0f;
            ringFlexAnglesR[1][2] = ModulateAngle(values, SensorIndexRightHanded.RingPh2, 0.0f, -110.0f); //roll over X is controller by abduction value 

            //ring3_l
            ringFlexAnglesR[2][0] = 0.0f;
            ringFlexAnglesR[2][1] = 0.0f;
            ringFlexAnglesR[2][2] = ModulateAngle(values, SensorIndexRightHanded.RingPh2, 0.0f, -55.0f); //roll over X is controller by abduction value 

            //little1_l
            littleFlexAnglesR[0][0] = 5.0f;                                                                                 
            littleFlexAnglesR[0][1] = ModulateAngle(values, SensorIndexRightHanded.AbdLittle, 5.0f, 25.0f); //rotation over Y is controller by abduction value 
            littleFlexAnglesR[0][2] = ModulateAngle(values, SensorIndexRightHanded.LittlePh1, 0.0f, -50.0f); //rotation over Z is controller by little1 flexion 

            //little2_l
            littleFlexAnglesR[1][0] = 0.0f;
            littleFlexAnglesR[1][1] = 0.0f;
            littleFlexAnglesR[1][2] = ModulateAngle(values, SensorIndexRightHanded.LittlePh2, 0.0f, -110.0f); //roll over Z is controller by a2nd phalange flexion

            //little3_l
            littleFlexAnglesR[2][0] = 0.0f;
            littleFlexAnglesR[2][1] = 0.0f;
            littleFlexAnglesR[2][2] = ModulateAngle(values, SensorIndexRightHanded.LittlePh2, 0.0f, -55.0f);
        }

        // Update is called once per frame
        void Update(){
            int i = 0;
            //update index

            //float[] sensVal = new float[10];
            //WGetWinTracker(ref sensVal, 0);



            if (gloveL.NewPackageAvailable()){
                VMGValues v = gloveL.GetPackage();

                //update values, this depends on the hand bones definition, please change this part in your application
                UpdateHandAnglesLeft(v);

                //update finger rendering position and rotation
                //All of the data from UpdateHandAnglesLeft() seem to be transferred over to their respective parts:
                for (i = 0; i < 3; i++){
                    //Transform.localRotation = the rotation of the transform relative to the transform rotation of the parent
                    ThumbL[i].localRotation = Quaternion.Euler(thumbFlexAnglesL[i]);
                    IndexL[i].localRotation = Quaternion.Euler(indexFlexAnglesL[i]);
                    MiddleL[i].localRotation = Quaternion.Euler(middleFlexAnglesL[i]);
                    RingL[i].localRotation = Quaternion.Euler(ringFlexAnglesL[i]);
                    LittleL[i].localRotation = Quaternion.Euler(littleFlexAnglesL[i]);
                }

                //PITCH = rotation about the x-axis
                //YAW = rotation about the y-axis
                //ROLL = rotation about the z-axis

                //compute wrist orientation vector taking into consideration reset yaw0
                Vector3 Zaxis = new Vector3(0.0f, 0.0f, 1.0f);//Z-axis is pointing towards you
                Vector3 Zrot = Quaternion.Euler(v.pitchW, -v.yawW + gloveL.GetYaw0(), v.rollW) * Zaxis;//Euler = returns a rotation
                float yaw = Mathf.Rad2Deg * Mathf.Atan2(-Zrot[0], Zrot[2]);//atan(-x, z)
                float yawD = 90.0f - yaw;
                float yawUpper = 1.5f * yawD / 3.0f;//upperArmL
                float yawLower = 1.5f * yawD / 3.0f;//lowerArmL

                if (yawLower < 0.0f){
                    yawLower = 0.0f;
                    yawUpper = yawD;
                }

                //TODO: Why is a yaw rotation performed on the z-axis?
                //fix yaw value, rotation along local Z axis
                upperArmL.localRotation = Quaternion.Euler(0.0f, 0.0f, yawUpper);
                lowerArmL.localRotation = Quaternion.Euler(0.0f, 0.0f, yawLower);

                //apply to upperArm ANOTHER rotation around x-axis
                //get x-axis on global coordinate by using TransformVector
                //TransformVector = Transform the given vector from local to world (global) space (from upperArm's local space)
                //From this object's perspecitive, it's x-axis is (1, 0, 0) but it may not be the case in world space
                Vector3 xaxis = upperArmL.TransformVector(new Vector3(1.0f, 0.0f, 0.0f));
                //Rotates the transform about the given axis passing through the given point by angle degrees
                upperArmL.RotateAround(upperArmL.position, xaxis, v.pitchW);

                //roll rotation along x-axis of lowerArm and x-axis of hand
                xaxis = lowerArmL.TransformVector(new Vector3(1.0f, 0.0f, 0.0f));
                upperArmL.RotateAround(upperArmL.position, xaxis, 0.25f * v.rollW);//TODO: Why is a roll rotation performed on the x-axis?
                lowerArmL.RotateAround(lowerArmL.position, xaxis, 0.65f * v.rollW);
                handL.localRotation = Quaternion.Euler(-90.0f + 0.1f * v.rollW, 0.0f, 0.0f);//rotate within local space

                //Compute hand PITCH angle (roll and yaw come from wrist):
                //TODO: Seems to be repeated
                //Zrot = Quaternion.Euler(v.pitchH, -v.yawW + gloveL.GetYaw0(), v.rollW) * Zaxis;
                //get ZRot in the lowerhand reference frame
                //InverseTransformVector = transform a vector from world space into local space (lowerArmL's local space)
                Vector3 ZrotLowerArm = lowerArmL.InverseTransformVector(Zrot);
                float pitchHandRel = Mathf.Rad2Deg * Mathf.Atan2(ZrotLowerArm[2], -ZrotLowerArm[0]);//atan(z, -x)
                //rotate hand along its OWN Z axis (Implies local space)
                Vector3 ZaxisHand = handL.TransformVector(new Vector3(0.0f, 0.0f, 1.0f));
                //TODO: Why is a pitch rotation performed on the z-axis?
                handL.RotateAround(handL.position, ZaxisHand, pitchHandRel);

                if (debugSkeletonLeft){
                    Debug.Log("New package L\n");
                    DrawSkeletonLeft(v);
                }
            }

            //check if a new package is arrived from glove
            if (gloveR.NewPackageAvailable()){
                Debug.Log("New package R\n");
                VMGValues v = gloveR.GetPackage();

                //update values, this depends on the hand bones definition, please change this part in your application
                UpdateHandAnglesRight(v);

                //update fingers rendering position and rotation
                for (i = 0; i < 3; i++){
                    ThumbR[i].localRotation = Quaternion.Euler(thumbFlexAnglesR[i]);
                    IndexR[i].localRotation = Quaternion.Euler(indexFlexAnglesR[i]);
                    MiddleR[i].localRotation = Quaternion.Euler(middleFlexAnglesR[i]);
                    RingR[i].localRotation = Quaternion.Euler(ringFlexAnglesR[i]);
                    LittleR[i].localRotation = Quaternion.Euler(littleFlexAnglesR[i]);
                }

                //compute wrist orientation vector taking into consideration reset yaw0
                Vector3 Zaxis = new Vector3(0.0f, 0.0f, 1.0f);
                Vector3 Zrot = Quaternion.Euler(v.pitchW, -v.yawW + gloveR.GetYaw0(), v.rollW) * Zaxis;

                float yaw = Mathf.Rad2Deg * Mathf.Atan2(-Zrot[0], Zrot[2]);

                float yawD = 90.0f + yaw;
                float yawUpper = 1.5f * yawD / 3.0f;
                float yawLower = 1.5f * yawD / 3.0f;

                //yawLower cannot be less than 0
                if (yawLower < 0.0f){
                    yawLower = 0.0f;
                    yawUpper = yawD;
                }

                //fix yaw value, rotation along local Z axis
                upperArmR.localRotation = Quaternion.Euler(0.0f, 0.0f, yawUpper);
                lowerArmR.localRotation = Quaternion.Euler(0.0f, 0.0f, yawLower);

                //apply to upperArm another rotation around x-axis
                //get x-axis on global coordinate
                Vector3 xaxis = upperArmR.TransformVector(new Vector3(1.0f, 0.0f, 0.0f));
                upperArmR.RotateAround(upperArmR.position, xaxis, v.pitchW);

                //roll, rotation along x-axis of lowerArm and x-axis of hand
                xaxis = lowerArmR.TransformVector(new Vector3(1.0f, 0.0f, 0.0f));
                upperArmR.RotateAround(upperArmR.position, xaxis, -0.25f * v.rollW);
                lowerArmR.RotateAround(lowerArmR.position, xaxis, -0.65f * v.rollW);
                handR.localRotation = Quaternion.Euler(-90.0f - 0.1f * v.rollW, 0.0f, 0.0f);

                //compute hand pitch relative angle
                //get ZRot in the lowerhand reference frame
                Zrot = Quaternion.Euler(v.pitchH, -v.yawW + gloveR.GetYaw0(), v.rollW) * Zaxis;
                Vector3 ZrotLowerArm = lowerArmR.InverseTransformVector(Zrot);
                float pitchHandRel = Mathf.Rad2Deg * Mathf.Atan2(-ZrotLowerArm[2], ZrotLowerArm[0]);
                //rotate hand along its own Z axis
                Vector3 ZaxisHand = handR.TransformVector(new Vector3(0.0f, 0.0f, 1.0f));
                handR.RotateAround(handR.position, ZaxisHand, pitchHandRel);

                if (debugSkeletonRight){
                    Debug.Log("New package R\n");
                    DrawSkeletonRight(v);
                }
            }
        }

        private void DrawSkeletonLeft(VMGValues v){
            Debug.Log("RPY_W:" + v.rollW + " " + v.pitchW + " " + v.yawW + " YAW0:" + gloveL.GetYaw0() + "\n");//roll, pitch, yaw for the wrist

            //TODO: draws skeletion with one index finger
            Debug.DrawRay(spine.position, new Vector3(0.1f, 0.0f, 0.0f), Color.magenta);
            Debug.DrawRay(spine.position, new Vector3(0.0f, 0.1f, 0.0f), Color.green);
            Debug.DrawRay(spine.position, new Vector3(0.0f, 0.0f, 0.1f), Color.blue);
            Debug.DrawRay(spine.position, clavicleL.position - spine.position, Color.red);
            Debug.DrawRay(clavicleL.position, upperArmL.position - clavicleL.position, Color.red);
            Debug.DrawRay(upperArmL.position, lowerArmL.position - upperArmL.position, Color.red);
            Debug.DrawRay(lowerArmL.position, handL.position - lowerArmL.position, Color.red);
            Debug.DrawRay(handL.position, IndexL[0].position - handL.position, Color.red);
            Debug.DrawRay(IndexL[0].position, IndexL[1].position - IndexL[0].position, Color.red);
            Debug.DrawRay(IndexL[1].position, IndexL[2].position - IndexL[1].position, Color.red);

            Vector3 X = new Vector3(0.0f, 0.0f, 0.3f);
            Vector3 Xrot = Quaternion.Euler(v.pitchW, -v.yawW + gloveL.GetYaw0(), v.rollW) * X;
            Debug.DrawRay(lowerArmL.position, Xrot, Color.magenta);

            //TODO: Seems to be repeated
            //Xrot = Quaternion.Euler(v.pitchH, -v.yawW + gloveL.GetYaw0(), v.rollW) * X;
            Debug.DrawRay(handL.position, Xrot, Color.magenta);

            //get XRot in the lowerhand reference frame
            Vector3 XrotLowerArm = lowerArmL.InverseTransformVector(Xrot);
            float pitchHandRel = Mathf.Rad2Deg * Mathf.Atan2(XrotLowerArm[2], -XrotLowerArm[0]);

            Vector3 xaxis = handL.TransformDirection(new Vector3(0.1f, 0f, 0f));
            Vector3 yaxis = handL.TransformDirection(new Vector3(0.0f, 0.1f, 0.0f));
            Vector3 zaxis = handL.TransformDirection(new Vector3(0.0f, 0.0f, 0.1f));

            Debug.DrawRay(handL.position, xaxis, Color.red);
            Debug.DrawRay(handL.position, yaxis, Color.green);
            Debug.DrawRay(handL.position, zaxis, Color.blue);

            xaxis = lowerArmL.TransformDirection(new Vector3(0.1f, 0f, 0f));
            yaxis = lowerArmL.TransformDirection(new Vector3(0.0f, 0.1f, 0.0f));
            zaxis = lowerArmL.TransformDirection(new Vector3(0.0f, 0.0f, 0.1f));

            Debug.DrawRay(lowerArmL.position, xaxis, Color.red);
            Debug.DrawRay(lowerArmL.position, yaxis, Color.green);
            Debug.DrawRay(lowerArmL.position, zaxis, Color.blue);

            //Debug.Log("Pitch Hand Rel:" + pitchHandRel + "\n");
        }

        private void DrawSkeletonRight(VMGValues v){
            Debug.Log("RPY_W:" + v.rollW + " " + v.pitchW + " " + v.yawW + "\n");//Roll, pitch, yaw of the wrist
            Debug.Log("RPY_H:" + v.rollH + " " + v.pitchH + " " + v.yawH + "\n");//Roll, pitch, yaw of the hand
            //Draws a line from start to start + dir in world coordinates
            //TODO: May need to specify length and duration
            Debug.DrawRay(spine.position, new Vector3(0.1f, 0.0f, 0.0f), Color.magenta);//x-axis
            Debug.DrawRay(spine.position, new Vector3(0.0f, 0.1f, 0.0f), Color.green);//y-axis
            Debug.DrawRay(spine.position, new Vector3(0.0f, 0.0f, 0.1f), Color.blue);//z-axis

            //Draw skeleton
            Debug.DrawRay(spine.position, clavicleR.position - spine.position, Color.red);
            Debug.DrawRay(clavicleR.position, upperArmR.position - clavicleR.position, Color.red);
            Debug.DrawRay(upperArmR.position, lowerArmR.position - upperArmR.position, Color.red);
            Debug.DrawRay(lowerArmR.position, handR.position - lowerArmR.position, Color.red);

            //Draw fingers
            Debug.DrawRay(handR.position, ThumbR[0].position - handR.position, Color.red);
            Debug.DrawRay(ThumbR[0].position, ThumbR[1].position - ThumbR[0].position, Color.red);
            Debug.DrawRay(ThumbR[1].position, ThumbR[2].position - ThumbR[1].position, Color.red);

            Debug.DrawRay(handR.position, IndexR[0].position - handR.position, Color.red);
            Debug.DrawRay(IndexR[0].position, IndexR[1].position - IndexR[0].position, Color.red);
            Debug.DrawRay(IndexR[1].position, IndexR[2].position - IndexR[1].position, Color.red);

            Debug.DrawRay(handR.position, LittleR[0].position - handR.position, Color.red);
            Debug.DrawRay(LittleR[0].position, LittleR[1].position - LittleR[0].position, Color.red);
            Debug.DrawRay(LittleR[1].position, LittleR[2].position - LittleR[1].position, Color.red);

            //TODO: draws a line between the index and the pinky?
            Debug.DrawRay(IndexR[0].position, LittleR[0].position - IndexR[0].position, Color.green);

            //TODO: Do these refer to 'middle position'?
            Vector3 medpos = (IndexR[0].position + LittleR[0].position);
            medpos[0] = medpos[0] / 2.0f;
            medpos[1] = medpos[1] / 2.0f;
            medpos[2] = medpos[2] / 2.0f;

            Debug.DrawRay(handR.position, medpos - handR.position, Color.green);

            Vector3 X = new Vector3(0.0f, 0.0f, 0.3f);
            Vector3 Xrot = Quaternion.Euler(v.pitchW, -v.yawW + gloveR.GetYaw0(), v.rollW) * X;

            Debug.DrawRay(lowerArmR.position, Xrot, Color.magenta);

            //TODO: seems to be duplicated:
            //Xrot = Quaternion.Euler(v.pitchH, -v.yawW + gloveR.GetYaw0(), v.rollW) * X;
            Debug.DrawRay(handR.position, Xrot, Color.magenta);

            //get XRot in the lowerhand reference frame
            Vector3 XrotLowerArm = lowerArmR.InverseTransformVector(Xrot);
            float pitchHandRel = Mathf.Rad2Deg * Mathf.Atan2(XrotLowerArm[2], XrotLowerArm[0]);//atan(z, x)
            Debug.Log("Pitch Hand Rel:" + pitchHandRel + "\n");

            Xrot.Normalize();
            float yawRot = 180.0f * ((float)System.Math.Atan2(Xrot[0], Xrot[2])) / 3.14159f;//atan(x, z)

            Vector3 xaxis = lowerArmR.TransformDirection(new Vector3(0.1f, 0f, 0f));
            Vector3 yaxis = lowerArmR.TransformDirection(new Vector3(0.0f, 0.1f, 0.0f));
            Vector3 zaxis = lowerArmR.TransformDirection(new Vector3(0.0f, 0.0f, 0.1f));

            Debug.DrawRay(lowerArmR.position, xaxis, Color.red);
            Debug.DrawRay(lowerArmR.position, yaxis, Color.green);
            Debug.DrawRay(lowerArmR.position, zaxis, Color.blue);

            xaxis = upperArmR.TransformDirection(new Vector3(0.1f, 0f, 0f));
            yaxis = upperArmR.TransformDirection(new Vector3(0.0f, 0.1f, 0.0f));
            zaxis = upperArmR.TransformDirection(new Vector3(0.0f, 0.0f, 0.1f));

            Debug.DrawRay(upperArmR.position, xaxis, Color.red);
            Debug.DrawRay(upperArmR.position, yaxis, Color.green);
            Debug.DrawRay(upperArmR.position, zaxis, Color.blue);

            xaxis = handR.TransformDirection(new Vector3(0.1f, 0f, 0f));
            yaxis = handR.TransformDirection(new Vector3(0.0f, 0.1f, 0.0f));
            zaxis = handR.TransformDirection(new Vector3(0.0f, 0.0f, 0.1f));

            Debug.DrawRay(handR.position, xaxis, Color.red);
            Debug.DrawRay(handR.position, yaxis, Color.green);
            Debug.DrawRay(handR.position, zaxis, Color.blue);

            //wrist quaternion representing wrist rotation (Can be accessed in VMGValues class)
            Debug.Log("QUATW:" + v.q0w.ToString("F3") + " " + v.q1w.ToString("F3") + " " + v.q2w.ToString("F3") + " " + v.q3w.ToString("F3") + "\n");
            //hand quaternion representing hand orientation
            Debug.Log("QUATH:" + v.q0h.ToString("F3") + " " + v.q1h.ToString("F3") + " " + v.q2h.ToString("F3") + " " + v.q3h.ToString("F3") + "\n");
        }
    }
}