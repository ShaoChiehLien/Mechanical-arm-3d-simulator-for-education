using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace NetworkTestVSC
{ 
    class Program
    {
        bool socketReady = false;
        TcpClient socket;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        
        static void Main(string[] args)
        {
            
            Program AllFunctions = new Program();
            //connect to server
            AllFunctions.ConnectToServer();
            
            /* ********************************************************** */
            //Please Write Code Below
            /* ********************************************************** */

            PTPJointParams ptpJointParams = new PTPJointParams();
            JOGCmd jogCmd = new JOGCmd();
            PTPCoordinateParams ptpCoordinateParams = new PTPCoordinateParams();
            PTPCmd ptpCmd = new PTPCmd();

            //Set up the velocity of inverse kinematics
            ptpCoordinateParams.xyzVelocity = 100;
            SetPTPCoordinateParams(ptpCoordinateParams);

            //Set up the velocity of forward kinematics
            ptpJointParams.velocity[0] = 50F;
            ptpJointParams.velocity[1] = 50F;
            ptpJointParams.velocity[2] = 50F;
            SetPTPJointParams(ptpJointParams);

            //Use forward kinematics, move to coordinate（-270, 140, 95）
            ptpCmd.ptpMode = 1;
            ptpCmd.x = -270F;
            ptpCmd.y = 140F;
            ptpCmd.z = 95F;
            SetPTPCmd(ptpCmd);

            //Turn on the suction cup
            SetEndEffectorSuctionCup(true);

            //Use forward kinematics, move to coordinate（130, 45, 56.9）
            ptpCmd.ptpMode = 3;
            ptpCmd.x = 130F;
            ptpCmd.y = 45F;
            ptpCmd.z = 56.9F;
            SetPTPCmd(ptpCmd);

            //Turn on the suction cup and release the object
            SetEndEffectorSuctionCup(false);

            /* ********************************************************** */
            //Please Write Code Above
            /* ********************************************************** */

            Console.WriteLine("Program End");
        }
        
        //Reference for the communcation protocol for each function: Dobot-Communication-Protocol-V1.1.5.pdf
        //ID 62, SetEndEffectorSuctionCup
        static void SetEndEffectorSuctionCup(bool isSucked)
        {
            Program AllFunctions = new Program();
            AllFunctions.ConnectToServer();
            byte[] DATA_TO_SEND = new byte[8];
            //info of the functon
            Message message = new Message();

            // set ID 62 INFO
            message.id = 62;
            message.rw = 1;
            message.isQueued = 0;
            message.paramsLen = 4;

            //Start set functions
            //Header
            DATA_TO_SEND[0] = 170;//0xAA
            DATA_TO_SEND[1] = 170;//0xAA

            // // Params
            DATA_TO_SEND[2] = message.paramsLen;//Len
            DATA_TO_SEND[3] = message.id;//ID
            DATA_TO_SEND[4] = 0;//Ctrl
            DATA_TO_SEND[5] = 0;//isCtrlEnabled
            DATA_TO_SEND[6] = Convert.ToByte(isSucked);//isSucked
            
            DATA_TO_SEND[7] = message.paramsLen;

            //Send instruction
            string hexString = string.Empty;
            hexString = ToHexString(DATA_TO_SEND);


            AllFunctions.Send(hexString);

        }

        //ID 73, SetJOGCmd
        static void SetJOGCmd(JOGCmd jogCmd)
        {
            Program AllFunctions = new Program();
            AllFunctions.ConnectToServer();
            byte[] DATA_TO_SEND = new byte[8];
            //info of the functon
            Message message = new Message();

            // set ID 80 INFO
            message.id = 73;
            message.rw = 1;
            message.isQueued = 0;
            message.paramsLen = 4;

            //Start set functions
            //Header
            DATA_TO_SEND[0] = 170;//0xAA
            DATA_TO_SEND[1] = 170;//0xAA

            // Params
            DATA_TO_SEND[2] = message.paramsLen;//Len
            DATA_TO_SEND[3] = message.id;//ID
            DATA_TO_SEND[4] = 0;//Ctrl

            DATA_TO_SEND[5] = jogCmd.isJoint; //ID
            DATA_TO_SEND[6] = jogCmd.cmd;//Ctrl

            DATA_TO_SEND[7] = message.paramsLen;

            //Send instruction
            string hexString = string.Empty;
            hexString = ToHexString(DATA_TO_SEND);

            AllFunctions.Send(hexString);

        }

        //ID 80, SetPTPJointParams
        static void SetPTPJointParams(PTPJointParams ptpJointParams)
        {
            Program AllFunctions = new Program();
            AllFunctions.ConnectToServer();
            byte[] DATA_TO_SEND = new byte[38];
            //info of the functon
            Message message = new Message();

            // set ID 80 INFO
            message.id = 80;
            message.rw = 1;
            message.isQueued = 0;
            message.paramsLen = 34;

            //Start set functions
            //Header
            DATA_TO_SEND[0] = 170;//0xAA
            DATA_TO_SEND[1] = 170;//0xAA

            // // Params
            DATA_TO_SEND[2] = message.paramsLen;//Len
            DATA_TO_SEND[3] = message.id;//ID
            DATA_TO_SEND[4] = 0;//Ctrl
            
            byte[] tempByte = new byte[16];

            ////////////////////
            // CHECK IF NULL
            ///////////////////
            
            
            Buffer.BlockCopy(ptpJointParams.velocity, 0, tempByte, 0, tempByte.Length);//velocity Parameters
            for(int i = 0; i <= 15; i++){
                DATA_TO_SEND[5 + i] = tempByte[i];
            } 

            Buffer.BlockCopy(ptpJointParams.acceleration, 0, tempByte, 0, tempByte.Length);//acceleration Parameters
            for(int i = 0; i <= 15; i++){
                DATA_TO_SEND[21 + i] = tempByte[i];
            } 
            DATA_TO_SEND[37] = message.paramsLen;

            //Send instruction
            string hexString = string.Empty;
            hexString = ToHexString(DATA_TO_SEND);


            AllFunctions.Send(hexString);

        }

        //ID 81, SetPTPCoordinateParams
        static void SetPTPCoordinateParams(PTPCoordinateParams ptpCoordinateParams)
        {
            Program AllFunctions = new Program();
            AllFunctions.ConnectToServer();
            byte[] DATA_TO_SEND = new byte[22];
            //info of the functon
            Message message = new Message();

            // set ID 81 INFO
            message.id = 81;
            message.rw = 1;
            message.isQueued = 0;
            message.paramsLen = 18;

            //Start set functions
            //Header
            DATA_TO_SEND[0] = 170;//0xAA
            DATA_TO_SEND[1] = 170;//0xAA

            // // Params
            DATA_TO_SEND[2] = message.paramsLen;//Len
            DATA_TO_SEND[3] = message.id;//ID
            DATA_TO_SEND[4] = 0;//Ctrl
            
            byte[] tempByte = new byte[16];

            ////////////////////
            // CHECK IF NULL
            ///////////////////
            
            float[] tempFloat = new float[4];
            tempFloat[0] = ptpCoordinateParams.xyzVelocity;
            tempFloat[1] = ptpCoordinateParams.rVelocity;
            tempFloat[2] = ptpCoordinateParams.xyzAcceleration;
            tempFloat[3] = ptpCoordinateParams.rAccleration;

            Buffer.BlockCopy(tempFloat, 0, tempByte, 0, tempByte.Length);//all parameters
            for(int i = 0; i <= 15; i++){
                DATA_TO_SEND[5 + i] = tempByte[i];
            } 

            DATA_TO_SEND[21] = message.paramsLen;

            //Send instruction
            string hexString = string.Empty;
            hexString = ToHexString(DATA_TO_SEND);

            AllFunctions.Send(hexString);
        }

        //ID 84, SetPTPCmd
        static void SetPTPCmd(PTPCmd ptpCmd)
        {
            Program AllFunctions = new Program();
            AllFunctions.ConnectToServer();
            byte[] DATA_TO_SEND = new byte[23];
            //info of the functon
            Message message = new Message();

            // set ID 84 INFO
            message.id = 84;
            message.rw = 1;
            message.isQueued = 0;
            message.paramsLen = 19;

            //Start set functions
            //Header
            DATA_TO_SEND[0] = 170;//0xAA
            DATA_TO_SEND[1] = 170;//0xAA

            // // Params
            DATA_TO_SEND[2] = message.paramsLen;//Len
            DATA_TO_SEND[3] = message.id;//ID
            DATA_TO_SEND[4] = 0;//Ctrl
            DATA_TO_SEND[5] = ptpCmd.ptpMode;
            
            byte[] tempByte = new byte[16];
            
            float[] tempFloat = new float[4];
            tempFloat[0] = ptpCmd.x;
            tempFloat[1] = ptpCmd.y;
            tempFloat[2] = ptpCmd.z;
            tempFloat[3] = ptpCmd.r;

            Buffer.BlockCopy(tempFloat, 0, tempByte, 0, tempByte.Length);//all parameters
            for(int i = 0; i <= 15; i++){
                DATA_TO_SEND[6 + i] = tempByte[i];
            } 

            DATA_TO_SEND[22] = message.paramsLen;

            //Send instruction
            string hexString = string.Empty;
            hexString = ToHexString(DATA_TO_SEND);

            AllFunctions.Send(hexString);
        }

        static string ToHexString(byte[] bytes)
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder str = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    str.Append(bytes[i].ToString("X2"));
                }
                hexString = str.ToString();
            }
            return hexString;
        }

        public void ConnectToServer()
        {
            //If already connected, ignore this function
            if (socketReady)
               return;

            string host = "127.0.0.1";
            int port = 6321;

            //Create the socket
            try
            {
                socket = new TcpClient(host, port);
                stream = socket.GetStream();
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);
                socketReady = true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Socket error:" + e.Message);
            } 

        }
        
        public void Send(string data)
        {
            if (!socketReady)
                return;
            writer.WriteLine(data);
            writer.Flush();
        }

    }

    public class PTPJointParams {
        public float[] velocity = {1, 5, 10, 15};
        public float[] acceleration = {50, 100, 150, 200};
    }

    public class PTPCoordinateParams {
        public float xyzVelocity = 5;
        public float rVelocity = 5;
        public float xyzAcceleration = 5;
        public float rAccleration = 5;
    }

    public class JOGCmd{
        public byte isJoint;
        public byte cmd;
    }

    public class PTPCmd{
        public byte ptpMode;
        public float x;
        public float y; 
        public float z; 
        public float r;
    }
    public class Message {
        public byte id;
        public byte rw;
        public byte isQueued;
        public byte paramsLen;
        public byte[] parameters;
    }

}