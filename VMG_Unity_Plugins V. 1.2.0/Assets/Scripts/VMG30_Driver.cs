using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

public class VMGValues{
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

    public int [] SensorValues = new int[Constants.NumSensors];//all values from dataglove sensors

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

public class VMG30_Driver {
    private int ComPort = 1;//comport used for dataglove communication (default)
    private int GloveType = Constants.RightHanded;//Right or Left Handed
    private int GloveStreamMode = Constants.PKG_QUAT_FINGER;//streaming package mode

    public VMGValues sensorValues = new VMGValues();

    private bool _newPackageAvailable;

    private bool ComThreadRunning = false;
    private Thread comThread;

    private object _lock = new object();

    private float YAW0;//set initial yaw angle in order to correctly align dataglove to the environment
    private Quaternion q0;

    //imu values mean filtering
    private int IMUFilter;
    List<Quaternion> lQuatW = new List<Quaternion>();
    List<Quaternion> lQuatH = new List<Quaternion>();

    /*driver initialization 
    pars comport dataglove communication port
    pars type dataglove type (RightHanded or LeftHanded)
    pars stream streaming type*/
    public void Init(int comport, int type, int stream){
        Debug.Log(ComPort + " will change to: " + comport + "\n");//ADDED

        ComPort = comport;
        GloveType = type;
        GloveStreamMode = stream;

        sensorValues.ResetValues();
        _newPackageAvailable = false;
        IMUFilter = FilterConst.Filter_High;
    }

    //Start dataglove communication
    public void StartCommunication(){
        //open comport
        
        //start stream reading thread
        ComThreadRunning = true;
        comThread = new Thread(GloveCommunication) {
            Name = "GloveCommunication"
        };

        comThread.Start();
    }

    public void SetYaw0(float yaw0){
        YAW0 = yaw0;
    }

    public float GetYaw0(){
        return YAW0;
    }

    public Quaternion GetQ0(){
        return q0;
    }

    public void StopCommunication(){
        ComThreadRunning = false;
    }

    // return true if a new package is available from the streaming
    public bool NewPackageAvailable(){
        bool retval;

        lock (_lock){
            retval = _newPackageAvailable;
            _newPackageAvailable = false;
        }

        return retval;
    }

    public VMGValues GetPackage(){
        VMGValues ret;

        lock (_lock){
            ret = sensorValues;
        }

        return ret;
    }

    public void SetIMUFilter(int value){
        //Nothing???
    }

    private void GloveCommunication(){
        bool FirstPackage = true;
        byte[] SendBuffer = new byte[256];
        byte[] RecvBuffer = new byte[1024];
        int NumBytesRecv = 0;
        int NumPkgRecv = 0;

        //Is initialized later...
        SerialPort vmgcom;// = new SerialPort("COM2", 230400);

        bool vmgcomOk = false;

        string str;
        str = "COM" + ComPort;
        Debug.Log("Open " + str + "\n");

        vmgcom = new SerialPort(str, 230400);
        vmgcom.Open();

        if (vmgcom.IsOpen){
            Debug.Log("Serial port correctly opened\n");
            vmgcomOk = true;
        }else{
            Debug.Log("Serial port error\n");
            vmgcomOk = false;
        }

        //if comport opened succesfully then send start streaming command
        if (vmgcomOk){
            vmgcom.ReadTimeout = 1;
            
            //send start streaming
            SendBuffer[0] = (byte)'$';
            SendBuffer[1] = (byte)0x0a;
            SendBuffer[2] = (byte)0x03;
            SendBuffer[3] = (byte)Constants.PKG_QUAT_FINGER;
            SendBuffer[4] = (byte)(SendBuffer[0] + SendBuffer[1] + SendBuffer[2] + SendBuffer[3]);
            SendBuffer[5] = (byte)'#';
            vmgcom.Write(SendBuffer, 0, 6);
        }

        Debug.Log("Thread Started\n");

        while (ComThreadRunning){
            Debug.Log("Read Bytes\n");

            try {
                //read bytes from the dataglove stream
                int bytesRead = vmgcom.Read(RecvBuffer, NumBytesRecv, 20);
                if (bytesRead > 0){
                    NumBytesRecv += bytesRead;
                    //check if a valid package is present in the buffer
                    if (NumBytesRecv > Constants.VMG30_PKG_SIZE){
                        //check header
                        int i = 0;
                        byte bcc = 0;
                        bool HeaderFound = false;
                        while ((!HeaderFound) && (i < (NumBytesRecv - 2))){
                            if ((RecvBuffer[i] == '$') && (RecvBuffer[i + 1] == 0x0a)) HeaderFound = true;
                            else i++;
                        }
                        
                        //if header found then parse package
                        if (HeaderFound){
                            //i == pos header
                            //bcc
                            int pospackage = i + 2;
                            bcc = ((byte)'$') + 0x0a;
                            //package len
                            byte pkglen = RecvBuffer[pospackage];

                            if ((pkglen + pospackage) < NumBytesRecv){
                                //package found
                                //see dataglove datasheet for package definitions

                                //check bcc
                                byte bccrecv = RecvBuffer[pospackage + pkglen - 1];
                                for (i = 0; i < pkglen - 1; i++){
                                    bcc += RecvBuffer[pospackage + i];
                                }

                                //if bcc is correct and package termination found then check for sensors values
                                if ((bcc == bccrecv) && (RecvBuffer[pospackage + pkglen] == '#')){
                                    //parse package
                                    int datastart = pospackage + 1;

                                    //check initial information, package type, glove id and package timestamp
                                    int pkgtype = RecvBuffer[datastart]; datastart++;
                                    int id = RecvBuffer[datastart] * 256 + RecvBuffer[datastart + 1]; datastart += 2;
                                    int timestamp = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;

                                    Debug.Log("ID:" + id + " Time:" + timestamp + "\n");

                                    if (pkgtype == Constants.PKG_QUAT_FINGER){
                                        //get quaternion values
                                        int q0w = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;
                                        int q1w = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;
                                        int q2w = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;
                                        int q3w = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;

                                        int q0h = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;
                                        int q1h = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;
                                        int q2h = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;
                                        int q3h = (RecvBuffer[datastart] << 24) + (RecvBuffer[datastart + 1] << 16) + (RecvBuffer[datastart + 2] << 8) + (RecvBuffer[datastart + 3]); datastart += 4;

                                        //get fingers values
                                        int[] sensors = new int[Constants.NumSensors];
                                        for (i = 0; i < Constants.NumSensors; i++){
                                            sensors[i] = (RecvBuffer[datastart] << 8) + RecvBuffer[datastart + 1]; datastart += 2;
                                        }

                                        //convert quaternions to float
                                        float q00H = (float)(q0h / 65536.0);
                                        float q11H = (float)(q1h / 65536.0);
                                        float q22H = (float)(q2h / 65536.0);
                                        float q33H = (float)(q3h / 65536.0);

                                        float q00W = (float)(q0w / 65536.0);
                                        float q11W = (float)(q1w / 65536.0);
                                        float q22W = (float)(q2w / 65536.0);
                                        float q33W = (float)(q3w / 65536.0);

                                        lQuatW.Add(new Quaternion(q00W, q11W, q22W, q33W));
                                        lQuatH.Add(new Quaternion(q00H, q11H, q22H, q33H));

                                        if (IMUFilter > 0){
                                            if (lQuatW.Count > IMUFilter){
                                                lQuatW.RemoveAt(0);
                                            }

                                            if (lQuatH.Count > IMUFilter){
                                                lQuatH.RemoveAt(0);
                                            }

                                            //get new values, filter quaternions for wrist and hand
                                            float q0wsum = 0.0f, q1wsum = 0.0f, q2wsum = 0.0f, q3wsum = 0.0f;
                                            int numval = lQuatW.Count;

                                            for (i = 0; i < numval; i++){
                                                Quaternion q = lQuatW[i];
                                                q0wsum += q.x;
                                                q1wsum += q.y;
                                                q2wsum += q.z;
                                                q3wsum += q.w;
                                            }

                                            q00W = q0wsum / numval;
                                            q11W = q1wsum / numval;
                                            q22W = q2wsum / numval;
                                            q33W = q3wsum / numval;

                                            //hand quanternion
                                            float q0hsum = 0.0f, q1hsum = 0.0f, q2hsum = 0.0f, q3hsum = 0.0f;
                                            numval = lQuatH.Count;
                                            for (i = 0; i < numval; i++){
                                                Quaternion q = lQuatH[i];
                                                q0hsum += q.x;
                                                q1hsum += q.y;
                                                q2hsum += q.z;
                                                q3hsum += q.w;
                                            }

                                            q00H = q0hsum / numval;
                                            q11H = q1hsum / numval;
                                            q22H = q2hsum / numval;
                                            q33H = q3hsum / numval;
                                        }
 
                                        //compute hand roll pitch and yaw
                                        float rollH = -Mathf.Rad2Deg * Mathf.Atan2(2.0f * (q00H * q11H + q22H * q33H), 1.0f - 2.0f * (q11H * q11H + q22H * q22H));
                                        float pitchH = -Mathf.Rad2Deg*Mathf.Asin(2.0f * (q00H * q22H - q33H * q11H));
                                        float yawH = Mathf.Rad2Deg*Mathf.Atan2(2.0f * (q00H * q33H + q11H * q22H), 1.0f - 2.0f * (q22H * q22H + q33H * q33H));

                                        //compute wrist roll pitch and yaw
                                        float rollW = -Mathf.Rad2Deg*Mathf.Atan2(2.0f * (q00W * q11W + q22W * q33W), 1.0f - 2.0f * (q11W * q11W + q22W * q22W));
                                        float pitchW = -Mathf.Rad2Deg*Mathf.Asin(2.0f * (q00W * q22W - q33W * q11W));
                                        float yawW = Mathf.Rad2Deg*Mathf.Atan2(2.0f * (q00W * q33W + q11W * q22W), 1.0f - 2.0f * (q22W * q22W + q33W * q33W));

                                        //bound on yaw (drift problem)
                                        if (yawW >= 180.0f) yawW = -360.0f + yawW;
                                        if (yawW <= -180.0f) yawW = 360.0f + yawW;

                                        if (yawH >= 180.0f) yawH = -360.0f + yawH;
                                        if (yawH <= -180.0f) yawH = 360.0f + yawH;

                                        if (FirstPackage){
                                            FirstPackage = false;
                                            q0 = Quaternion.Euler(rollW, pitchW, -yawW);
                                            SetYaw0(yawW);
                                        }

                                        //update sensor values (protected)
                                        lock(_lock){
                                            sensorValues.timestamp = timestamp;

                                            sensorValues.pitchH = pitchH;
                                            sensorValues.rollH = rollH;
                                            sensorValues.yawH = yawH;// -YAW0;

                                            sensorValues.pitchW = pitchW;
                                            sensorValues.rollW = rollW;
                                            sensorValues.yawW = yawW;// -YAW0;

                                            sensorValues.q0h = q00H;
                                            sensorValues.q1h = q11H;
                                            sensorValues.q2h = q22H;
                                            sensorValues.q3h = q33H;

                                            sensorValues.q0w = q00W;
                                            sensorValues.q1w = q11W;
                                            sensorValues.q2w = q22W;
                                            sensorValues.q3w = q33W;

                                            for (i = 0; i < Constants.NumSensors; i++){
                                                sensorValues.SensorValues[i] = sensors[i];
                                            }

                                            _newPackageAvailable = true;
                                        }
                                    }

                                    NumPkgRecv++;
                                }

                                Debug.Log("PKGRECV: " + NumPkgRecv + "\n");

                                //shift streaming buffer
                                int finpos = pospackage + pkglen;
                                int bytesrem = NumBytesRecv - finpos - 1;

                                for (i = 0; i < bytesrem; i++){
                                    RecvBuffer[i] = RecvBuffer[finpos + 1 + i];
                                }

                                NumBytesRecv = bytesrem;
                            }
                        }else{
                            Debug.Log("Header not found\n");
                            NumBytesRecv = 0;
                        }
                    }
                }
            }catch{
                //serial port generates an exeption, do nothing
                Debug.Log("Unable to read bytes from the dataglove stream");
            }
        }

        //thread completed, send cend streaming and close the port
        if (vmgcomOk){
            vmgcom.ReadTimeout = 1;
            
            //send start streaming
            SendBuffer[0] = (byte)'$';
            SendBuffer[1] = (byte)0x0a;
            SendBuffer[2] = (byte)0x03;
            SendBuffer[3] = (byte)Constants.PKG_NONE;
            SendBuffer[4] = (byte)(SendBuffer[0] + SendBuffer[1] + SendBuffer[2] + SendBuffer[3]);
            SendBuffer[5] = (byte)'#';
            vmgcom.Write(SendBuffer, 0, 6);
        }

        Debug.Log("Thread end\n");
        vmgcom.Close();
    }
}