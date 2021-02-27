using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Threading;


public class MainControl : MonoBehaviour
{

    public Slider sliderTheta0; //base motor rotation
    public Slider sliderTheta1; //lower motor rotation
    public Slider sliderTheta2; //upper motor rotation

    public GameObject UpperArm;
    public GameObject objectSphere;
    public GameObject objectSphere1;
    public GameObject dontmove;
    public Rigidbody objectSphererigid;
    public Rigidbody objectSphererigid1;
    public GameObject Check_suck;
    public const double PI = 3.14159265358979;
    bool checkkey = true;
    public int NewDataSign = 0;
    public Text Xposition;
    public Text Yposition;
    public Text Zposition;
    public int invisibleCount = 0;

    public Server serverScript;//use value in server


    public byte[] instructionsBytes = new byte[10000]; // Assume there are 10000 bytes of data

    public float originx, originy, originz; //Coordincate of the suction cup
    public float motor1x = 0, motor1y = 138, motor1z = -60; //Coordincate of motor 1
    public float V80x, V80y, V80z, A80x, A80y, A80z;//Velocity and Accelerated Velocity of three motors
    public float V81, A81;//Velocity and Accelerated Velocity of suction cup
    float V84x, V84y, V84z;



    float TargetCoorX, TargetCoorY, TargetCoorZ; //Use the angle of each motor to caculate the target coordinate



    int RDpointer = 0; //RDpointer is an instructionsBytes' pointer
    bool sucked;

    

    void Start()
    {
        sucked = false;
        sliderTheta0.value = 60;
        sliderTheta1.value = 90;
        sliderTheta2.value = 0;
        originx = -150;
        originy = 245;
        originz = 23;

    }

    void FixedUpdate()
    {
        if (checkkey == true)
        {
            StartCoroutine(MainProgram());
        }
    }
    IEnumerator MainProgram()
    {
        checkkey = false;

        objectSphererigid.AddForce(0, -17000 * Time.deltaTime, 0);
        objectSphererigid1.AddForce(0, -17000 * Time.deltaTime, 0);


        if (sucked == true)
        {
            objectSphererigid.AddForce(0, 17000 * Time.deltaTime, 0);
        }

        CoorDetermine(sliderTheta0.value, sliderTheta1.value, sliderTheta2.value);


        if (NewDataSign != serverScript.byte_stored_pointer)
        {
            for (int i = 0; i < serverScript.byte_stored_pointer; i++)
            {
                Debug.Log(instructionsBytes[i]);
            }
            NewDataSign = serverScript.byte_stored_pointer;
            Debug.Log("-------------------------------------------");
            Debug.Log("-------------------------------------------");
        }

        


        while (((instructionsBytes[RDpointer] == instructionsBytes[RDpointer + 1]) && instructionsBytes[RDpointer] == 170) && ((instructionsBytes[RDpointer + 2]) == instructionsBytes[RDpointer + 2 + 1 + instructionsBytes[RDpointer + 2]]))
        {
            bool CloseCond = suckfunc(objectSphere.transform.position.x, objectSphere.transform.position.y, objectSphere.transform.position.z); // Contantly update the relation between target and current position
            //Reference for the communcation protocol for each function: Dobot-Communication-Protocol-V1.1.5.pdf
            //ID 62, SetEndEffectorSuctionCup
            if (instructionsBytes[RDpointer + 3] == 62)
            {

                if (instructionsBytes[RDpointer + 6] == 0 && sucked == false)
                {
                    Debug.Log("Unsucked");//Didn't get the ubject
                }


                if (instructionsBytes[RDpointer + 6] == 0 && sucked == true)
                {
                    sucked = false;
                    objectSphere.transform.parent = dontmove.transform;
                    Debug.Log("Released!");
                }
                if (instructionsBytes[RDpointer + 6] == 1)
                {
                    if (CloseCond)
                    {
                        sucked = true;
                        objectSphere.transform.parent = UpperArm.transform;
                        Debug.Log("Already Sucked");
                    }
                    else
                    {
                        sucked = true;

                        Debug.Log("Nothing to Suck");
                    }

                }
            }
            //Reference for the communcation protocol for each function: Dobot-Communication-Protocol-V1.1.5.pdf
            //ID 73, SetJOGCmd
            if (instructionsBytes[RDpointer + 3] == 73) // Execute unit movement
            {
                if (instructionsBytes[RDpointer + 5] == 0)
                {// Inverse kinemetic
                    JOGcmdcoor(instructionsBytes[RDpointer + 6]);

                }
                else if (instructionsBytes[RDpointer + 5] == 1)
                {// Forward kinemetic


                    JOGcmdsection(instructionsBytes[RDpointer + 6]);
                }
            }
            //Reference for the communcation protocol for each function: Dobot-Communication-Protocol-V1.1.5.pdf
            //ID 80, SetPTPJointParams
            if (instructionsBytes[RDpointer + 3] == 80) //Setptpjointparameters
            {// Forward kinemetic: set up coordinate

                byte[] Vx = { instructionsBytes[RDpointer + 5], instructionsBytes[RDpointer + 6], instructionsBytes[RDpointer + 7], instructionsBytes[RDpointer + 8] };
                V80x = BitConverter.ToSingle(Vx, 0); // Store motor 0's rotation speed
                byte[] Vy = { instructionsBytes[RDpointer + 9], instructionsBytes[RDpointer + 10], instructionsBytes[RDpointer + 11], instructionsBytes[RDpointer + 12] };
                V80y = BitConverter.ToSingle(Vy, 0); // Store motor 1's rotation speed
                byte[] Vz = { instructionsBytes[RDpointer + 13], instructionsBytes[RDpointer + 14], instructionsBytes[RDpointer + 15], instructionsBytes[RDpointer + 16] };
                V80z = BitConverter.ToSingle(Vz, 0); // Store motor 2's rotation speed

                byte[] Ax = { instructionsBytes[RDpointer + 21], instructionsBytes[RDpointer + 22], instructionsBytes[RDpointer + 23], instructionsBytes[RDpointer + 24] };
                A80x = BitConverter.ToSingle(Ax, 0); // Store motor 0's rotation speed
                byte[] Ay = { instructionsBytes[RDpointer + 25], instructionsBytes[RDpointer + 26], instructionsBytes[RDpointer + 27], instructionsBytes[RDpointer + 28] };
                A80y = BitConverter.ToSingle(Ay, 0); // Store motor 1's rotation speed
                byte[] Az = { instructionsBytes[RDpointer + 29], instructionsBytes[RDpointer + 30], instructionsBytes[RDpointer + 31], instructionsBytes[RDpointer + 32] };
                A80z = BitConverter.ToSingle(Az, 0); // Store motor 2's rotation speed

            }
            //Reference for the communcation protocol for each function: Dobot-Communication-Protocol-V1.1.5.pdf
            //ID 81, SetPTPCoordinateParams
            if (instructionsBytes[RDpointer + 3] == 81) //SetptpCoordinateParams
            {// Inverse kinemetic: set up coordinate

                byte[] V = { instructionsBytes[RDpointer + 5], instructionsBytes[RDpointer + 6], instructionsBytes[RDpointer + 7], instructionsBytes[RDpointer + 8] };
                V81 = BitConverter.ToSingle(V, 0); // Store the velocity of three vectors


                byte[] A = { instructionsBytes[RDpointer + 13], instructionsBytes[RDpointer + 14], instructionsBytes[RDpointer + 15], instructionsBytes[RDpointer + 16] };
                A81 = BitConverter.ToSingle(A, 0); // Store the accelerated velocity of three vectors

            }
            //ID 84, SetPTPCmd
            if (instructionsBytes[RDpointer + 3] == 84) //Setptpcmd
            {
                byte[] Vx = { instructionsBytes[RDpointer + 6], instructionsBytes[RDpointer + 7], instructionsBytes[RDpointer + 8], instructionsBytes[RDpointer + 9] };
                V84x = BitConverter.ToSingle(Vx, 0); //Stere target's x coordinate and motor angle
                byte[] Vy = { instructionsBytes[RDpointer + 10], instructionsBytes[RDpointer + 11], instructionsBytes[RDpointer + 12], instructionsBytes[RDpointer + 13] };
                V84y = BitConverter.ToSingle(Vy, 0); //Stere target's y coordinate and motor angle
                byte[] Vz = { instructionsBytes[RDpointer + 14], instructionsBytes[RDpointer + 15], instructionsBytes[RDpointer + 16], instructionsBytes[RDpointer + 17] };
                V84z = BitConverter.ToSingle(Vz, 0); //Stere target's z coordinate and motor angle

                if (instructionsBytes[RDpointer + 5] == 0 || instructionsBytes[RDpointer + 5] == 1 || instructionsBytes[RDpointer + 5] == 2)
                {

                    float motor0theta, motor1theta, motor2theta;
                    bool OutRGcond1 = Mathf.Sqrt(V84x * V84x + (V84y - 138) * (V84y - 138) + (V84z + 60) * (V84z + 60)) > (135 + 187);
                    bool OutRGcond2 = Mathf.Sqrt(V84x * V84x + (V84y - 273) * (V84y - 273) + (V84z + 60) * (V84z + 60)) < 187;
                    bool OutRGcond3 = Mathf.Sqrt(V84x * V84x + (V84y - 138) * (V84y - 138) + (V84z + 60) * (V84z + 60)) < 210.776;
                    bool OutRGcond4 = Mathf.Sqrt(18225 + V84x * V84x + (138 - V84y) * (138 - V84y) + (V84z + 60) * (V84z + 60) - 2 * 135 * Mathf.Sqrt(V84x * V84x + (V84z + 60) * (V84z + 60))) > 187;
                    bool OutRGcond5 = (-V84x > 322 || -V84x < 0 || V84y > 460 || V84y < 0 || V84z > 262 || V84z < -382);

                    if (false/*OutRGcond1 || OutRGcond2 || OutRGcond3 || (OutRGcond4 && V84y < 138) || OutRGcond5*/)
                    {
                        Debug.Log("Out of Range");
                    }
                    else
                    { //Use the known v81 velocity to apporximate the A81 velocity
                        float TargetDest = (float)Math.Sqrt(V84x * V84x + (V84y - 138) * (V84y - 138) + (V84z + 60) * (V84z + 60));
                        if (V84z > -60)
                        {
                            motor0theta = (float)((Math.Atan((0 + V84x) / (-60 - V84z)) * 180 / PI)) - sliderTheta0.value;
                        }
                        else
                        {
                            motor0theta = 180 + (float)(Math.Atan((0 + V84x) / (-60 - V84z)) * 180 / PI) - sliderTheta0.value;
                        }
                        if (V84y > 138)
                        {
                            motor1theta = (float)((Math.Asin(Mathf.Abs(V84y - 138) / TargetDest) * 180 / PI) + Math.Acos((18225 + TargetDest * TargetDest - 34969) / (2 * 135 * TargetDest)) * 180 / PI) - sliderTheta1.value;
                        }
                        else
                        {
                            motor1theta = (float)(-(Math.Asin(Mathf.Abs(V84y - 138) / TargetDest) * 180 / PI) + Math.Acos((18225 + TargetDest * TargetDest - 34969) / (2 * 135 * TargetDest)) * 180 / PI) - sliderTheta1.value;
                        }
                        motor2theta = (float)(Math.Acos((18225 + 34969 - TargetDest * TargetDest) / (2 * 135 * 187)) * 180 / PI) - 80 - sliderTheta2.value;


                        float TotalTime = (float)(Math.Sqrt((-V84x - originx) * (-V84x - originx) + (V84y - originy) * (V84y - originy) + (V84z - originz) * (V84z - originz)) / V81);
                        float cutpart = 20 * TotalTime;

                        float AlRDmov0 = 0, AlRDmov1 = 0, AlRDmov2 = 0;
                
                        for (float i = 0; i < TotalTime; i = i + (float)(TotalTime / cutpart))
                        {

                            if ((Mathf.Abs(AlRDmov0) < Mathf.Abs(motor0theta)) && Check_suck.transform.position.y > 5)
                            {
                                sliderTheta0.value = sliderTheta0.value + motor0theta / cutpart;
                                AlRDmov0 = Mathf.Abs(AlRDmov0) + Mathf.Abs(motor0theta / cutpart);
                            }
                            else
                            {
                                sliderTheta0.value = sliderTheta0.value;

                            }

                            if ((Mathf.Abs(AlRDmov1) < Mathf.Abs(motor1theta)) && Check_suck.transform.position.y > 5)
                            {
                                sliderTheta1.value = sliderTheta1.value + motor1theta / cutpart;
                                AlRDmov1 = Mathf.Abs(AlRDmov1) + Mathf.Abs(motor1theta / cutpart);
                            }
                            else
                            {
                                sliderTheta1.value = sliderTheta1.value;

                            }

                            if ((Mathf.Abs(AlRDmov2) < Mathf.Abs(motor2theta)) && Check_suck.transform.position.y > 5)
                            {
                                sliderTheta2.value = sliderTheta2.value + motor2theta / cutpart;
                                AlRDmov2 = Mathf.Abs(AlRDmov2) + Mathf.Abs(motor2theta / cutpart);
                            }
                            else
                            {
                                sliderTheta2.value = sliderTheta2.value;

                            }

                            yield return new WaitForSeconds(TotalTime / cutpart);



                        }

                        originx = V84x;
                        originy = V84y;
                        originz = V84z;
                    }



                }

                if (instructionsBytes[RDpointer + 5] == 3 || instructionsBytes[RDpointer + 5] == 4 || instructionsBytes[RDpointer + 5] == 5)
                {// Forward kinemetic

                    float time0 = Mathf.Abs((V84x - sliderTheta0.value) / V80x);
                    float time1 = Mathf.Abs((V84y - sliderTheta1.value) / V80y);
                    float time2 = Mathf.Abs((V84z - sliderTheta2.value) / V80z);
                    float maxtime = time2 > (time0 > time1 ? time0 : time1) ? time2 : (time0 > time1 ? time0 : time1);

                    // motortheta means the angle that suppose to move for each moter
                    float motor0theta = V84x - sliderTheta0.value;
                    float motor1theta = V84y - sliderTheta1.value;
                    float motor2theta = V84z - sliderTheta2.value;

                    float AlRDmov0 = 0, AlRDmov1 = 0, AlRDmov2 = 0; //moved angle
                    float cutpart = 20 * maxtime;

                    for (float i = 0; i < maxtime; i = i + (float)(maxtime / cutpart))
                    {

                        if ((Mathf.Abs(AlRDmov0) < Mathf.Abs(motor0theta)) && Check_suck.transform.position.y > 5)
                        {
                            sliderTheta0.value = sliderTheta0.value + motor0theta / cutpart;
                            AlRDmov0 = Mathf.Abs(AlRDmov0) + Mathf.Abs(motor0theta / cutpart);

                        }
                        else
                        {
                            sliderTheta0.value = sliderTheta0.value;
                        }

                        if ((Mathf.Abs(AlRDmov1) < Mathf.Abs(motor1theta)) && Check_suck.transform.position.y > 5)
                        {
                            sliderTheta1.value = sliderTheta1.value + motor1theta / cutpart;
                            AlRDmov1 = Mathf.Abs(AlRDmov1) + Mathf.Abs(motor1theta / cutpart);
                        }
                        else
                        {
                            sliderTheta1.value = sliderTheta1.value;
                        }

                        if ((Mathf.Abs(AlRDmov2) < Mathf.Abs(motor2theta)) && Check_suck.transform.position.y > 5)
                        {
                            sliderTheta2.value = sliderTheta2.value + motor2theta / cutpart;
                            AlRDmov2 = Mathf.Abs(AlRDmov2) + Mathf.Abs(motor2theta / cutpart);
                        }
                        else
                        {
                            sliderTheta2.value = sliderTheta2.value;
                        }
                        yield return new WaitForSeconds(maxtime / cutpart);
                    }

                    sliderTheta0.value = V84x;
                    sliderTheta1.value = V84y;
                    sliderTheta2.value = V84z;

                }

            }
            RDpointer = RDpointer + instructionsBytes[RDpointer + 2] + 4;
        }

        // Check if the data is sending, if so, wait for another 5 sec
        if (((instructionsBytes[RDpointer] == instructionsBytes[RDpointer + 1]) && instructionsBytes[RDpointer] == 170) && ((instructionsBytes[RDpointer + 2]) != instructionsBytes[RDpointer + 2 + 1 + instructionsBytes[RDpointer + 2]]))
        {
            yield return new WaitForSeconds(5);
        }

        // If reader unrecognized ID, ignore and read the next block of data
        if (((instructionsBytes[RDpointer] == instructionsBytes[RDpointer + 1]) && instructionsBytes[RDpointer] == 170) && ((instructionsBytes[RDpointer + 3] != 62) || (instructionsBytes[RDpointer + 3] != 73) || (instructionsBytes[RDpointer + 3] != 80) || (instructionsBytes[RDpointer + 3] != 81) || (instructionsBytes[RDpointer + 3] != 84)))
        {
            RDpointer = RDpointer + instructionsBytes[RDpointer + 2] + 4;
        }
        checkkey = true;
    }
    void JOGcmdcoor(int a)
    {
        if (a == 1 && Check_suck.transform.position.y > 5)
        {
            originx += 5;
        }
        if (a == 2 && Check_suck.transform.position.y > 5)
        {
            originx -= 5;
        }
        if (a == 3 && Check_suck.transform.position.y > 5)
        {
            originy += 5;
        }
        if (a == 4 && Check_suck.transform.position.y > 5)
        {
            originy -= 5;
        }
        if (a == 5 && Check_suck.transform.position.y > 5)
        {
            originz += 5;
        }
        if (a == 6 && Check_suck.transform.position.y > 5)
        {
            originz -= 5;
        }
    }
    void JOGcmdsection(int a)
    {
        if (a == 1 && Check_suck.transform.position.y > 5)
        {
            sliderTheta0.value += 1;
        }
        if (a == 2 && Check_suck.transform.position.y > 5)
        {
            sliderTheta0.value -= 1;
        }
        if (a == 3 && Check_suck.transform.position.y > 5)
        {
            sliderTheta1.value += 1;
        }
        if (a == 4 && Check_suck.transform.position.y > 5)
        {
            sliderTheta1.value -= 1;
        }
        if (a == 5 && Check_suck.transform.position.y > 5)
        {
            sliderTheta2.value += 1;
        }
        if (a == 6 && Check_suck.transform.position.y > 5)
        {
            sliderTheta2.value -= 1;
        }


    }
    void CoorDetermine(float theta0, float theta1, float theta2)
    {
        float theta0use = (float)(theta0 / 180 * PI); //angle
        float theta1use = (float)(theta1 / 180 * PI);
        float theta2use = (float)((theta2 + 80) / 180 * PI);
        float TD = (float)(Mathf.Sqrt(187 * 187 + 135 * 135 - 2 * 135 * 187 * Mathf.Cos(theta2use)));
        float phi = (float)(Mathf.Acos((TD * TD + 135 * 135 - 187 * 187) / (2 * 135 * TD)));  //angle


        TargetCoorX = -TD * Mathf.Sin(theta0use);
        Xposition.text = TargetCoorX.ToString("0.#");
        TargetCoorZ = TD * Mathf.Cos(theta0use) - 60;
        Zposition.text = TargetCoorZ.ToString("0.#");
        if (theta1use >= phi)
        {

            TargetCoorY = TD * Mathf.Sin(theta1use - phi) + 138;
            Yposition.text = TargetCoorY.ToString("0.#");
        }

        else if (theta1use < phi)
        {
            TargetCoorY = -TD * Mathf.Sin(phi - theta1use) + 138;
            Yposition.text = TargetCoorY.ToString("0.#");

        }


    }
    public bool suckfunc(float coorX, float coorY, float coorZ)  // Check if the suction cup is close enough to the object
    {

        float testtheta3, testtheta1, testtheta2;
        float suckmargin = 30F;

        testtheta1 = Mathf.Abs(coorX - Check_suck.transform.position.x);
        testtheta2 = Mathf.Abs(coorY - Check_suck.transform.position.y);
        testtheta3 = Mathf.Abs(coorZ - Check_suck.transform.position.z);

        if ((testtheta1 < suckmargin) && (testtheta2 < suckmargin) && (testtheta3 < suckmargin))
        {
            return true;
        }
        else
        {

            return false;

        }
    }

    public void Invisible()
    {
        if (invisibleCount % 2 == 0) {
            objectSphere.GetComponent<Renderer>().enabled = true;
            objectSphere1.GetComponent<Renderer>().enabled = true;
            invisibleCount++;
        }
        else
        {
            objectSphere.GetComponent<Renderer>().enabled = false;
            objectSphere1.GetComponent<Renderer>().enabled = false;
            invisibleCount++;
        }
    }
}





