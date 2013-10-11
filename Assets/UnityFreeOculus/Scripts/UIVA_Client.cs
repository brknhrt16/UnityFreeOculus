using UnityEngine;
using System.Collections;

///-----------------------------------------------------------------------------------
///-----------------------------------------------------------------------------------
/// UIVA (Unity Indie VRPN Adapter) Unity
/// 
/// Function: The client side of UIVA living in Unity as a DLL file.
///           Unity creates a UIVA class and calls its GetXXXData(out X, out X) functions
///           to get the latest data from the sensor devices.
/// 
/// About UIVA:
/// 
///   UIVA is a middle-ware between VRPN and Unity. It enables games developed by Unity3D INDIE
///   to be controlled by devices powered by VRPN. It has a client and a server simultaneously.
///   For VRPN, UIVA is its client which implements several callback functions to receive the 
///   latest data from the devices. For Unity, UIVA is a server that stores the latest sensor
///   data which allows it to query. The framework is shown as below:
///   
///        ~~~Sensor~~~      ~~~VRPN~~~      ~~~~~~~~~~~~UIVA~~~~~~~~~~~~~~~    ~~~Unity3D~~~     
///        
///   Kinect-----(FAAST)---->|--------|    |--------|--------|    |---------|
///    Wii ----(VRPN Wii)--->|        |    |        |        |    |         |--->Object transform
///   BPack --(VRPN BPack)-->|  VRPN  |    |  VRPN  | Unity  |    |  Unity  |
///           ...            |        |===>|  .net  | socket |===>|  socket |--->GUI
///           ...            | server |    |        |        |    |         |
///           ...            |        |    | client | server |    |  client |--->etc. of Unity3D
///           ...            |--------|    |--------|--------|    |---------|
///    
/// Special note: 
///
///      The VRPNWrapper implemented by the AR lab of Georgia Institute of Technology offers
///   a easier to use wrapper of VRPN to be used as a plugin in Unity3D Pro. If you can afford 
///   the Pro version of Unity. I suggest you to use VRPNWrapper. Their website is:
///           https://research.cc.gatech.edu/uart/content/about
///   They also implemented a ARToolkit wrapper which enables AR application in Unity. 
///   Check out their UART project, it is awesome!
///    
/// Author: 
/// 
/// Jia Wang (wangjia@wpi.edu)
/// Human Interaction in Virtual Enviroments (HIVE) Lab
/// Department of Computer Science
/// Worcester Polytechnic Institute
/// 
/// History: (1.0) 01/11/2011  by  Jia Wang
///
/// Acknowledge: Thanks to Chris VanderKnyff for the .NET version of VRPN
///                     to UNC for the awesome VRPN
///                     to Unity3D team for the wonderful engine
///              
///              and above all, special thanks to 
///                 Prof. Robert W. Lindeman (http://www.wpi.edu/~gogo) 
///              for the best academic advice.
///              
///-----------------------------------------------------------------------------------
///-----------------------------------------------------------------------------------

//The client side is generally used as a DLL file in Unity
//But it can be compiled to a console app as well for debugging

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace UIVA
{
    /// <summary>
    /// UIVA_Client class
    /// </summary>
    public class UIVA_Client
    {
        //Liberal values
        const int X = 0;
        const int Y = 1;
        const int Z = 2;

        Socket socClient;       //Socket deal with communication
        byte[] recBuffer = new byte[256];   //Receive buffer
        string recStr = "";                 //Deciphered receive 

        /// <summary>
        /// Connect and test connection
        /// </summary>
        /// <param name="serverIP">The IP address of the server, 
        /// should be the local IP if used as Unity interface</param>
        public UIVA_Client(string serverIP)
        {
            // If the UIVA server is in the local machine,
            // find its IP address and connect automatically
            if (serverIP == "localhost")
            {
                IPHostEntry host;
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        serverIP = ip.ToString();
                    }
                }
            }

            try
            {
                //Create a client socket
                socClient = new Socket(AddressFamily.InterNetwork,
                                    SocketType.Stream, ProtocolType.IP);
                //Parse the IP address string into an IPAddress object
                IPAddress serverAddr = IPAddress.Parse(serverIP);
                //Port: 8881
                IPEndPoint serverMachine = new IPEndPoint(serverAddr, 8881);
                //Connect
                socClient.Connect(serverMachine);
                //Send a confirmation message
                SendMessage("Ready?\n");
                ReceiveMessage();
                if (recStr != "Ready!")
                {
                    throw new Exception("Not ready?");
                }
            }
            catch (Exception e)
            {
                Exception initError = new Exception(e.ToString()
                                        + "\nClient failed to connect to server. Is your IP correct?"
                                        + "Is your UIVA working\n");
                throw initError;
            }
        }

        /// <summary>
        /// Send a message to the server
        /// </summary>
        /// <param name="msg">message content, end with a '\n'</param>
        private void SendMessage(string msg)
        {
            try
            {
                //Encode a message
                byte[] sendBuffer = Encoding.ASCII.GetBytes(msg);
                socClient.Send(sendBuffer);
            }
            catch (Exception e)
            {
                Exception sendError = new Exception(e.ToString() + "Client failed to send message.\n");
                throw sendError;
            }
        }

        /// <summary>
        /// Receive message from the server, decode and store in "recStr" variable
        /// </summary>
        private void ReceiveMessage()
        {
            try
            {
                socClient.Receive(recBuffer);
                recStr = Encoding.Default.GetString(recBuffer);
                //Remove the tailing '\0's after the '\n' token, caused by the buffer size
                int ixEnd = recStr.IndexOf('\n');
                recStr = recStr.Remove(ixEnd);
            }
            catch (Exception e)
            {
                Exception recError = new Exception(e.ToString()
                                        + "Client failed to receive message.\n");
                throw recError;
            }
        }

        /// <summary>
        /// Get the coordinate data from a general mouse
        /// The mouse data are formated as:
        /// 
        /// "MOUSE,,X,Y,anaTS,L/l,M/m,R/r,buttTS\n"
        /// 
        /// X, Y are double values, 
        /// LMR stand for Left, Middle and Right button, uppercase for pressed, lowercase for released
        /// TS means TimeStamp, for both analog events and button events, formated by option "o"
        /// 
        /// </summary>
        /// <param name="x"> Coordinate data on the X axis</param>
        /// <param name="y"> Coordinate data on the Y axis</param>
        /// <returns></returns>
        public void GetMouseData(int which, out double x, out double y, out string buttons)
        {
            SendMessage(String.Format("Mouse?{0}?\n", which));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });    //Parse
                //Skip "MOUSE,,"
                x = System.Convert.ToDouble(sections[2]);            //Get X axis acceleration
                y = System.Convert.ToDouble(sections[3]);            //Get Y axis acceleration
                //Skip the analog timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //anaTS = DateTime.Parse(sections[4]);
                buttons = sections[5];
                //Skip the button timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //buttTS = DateTime.Parse(sections[6]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one mouse */
        public void GetMouseData(out double x, out double y, out string buttons)
        {
            GetMouseData(1, out x, out y, out buttons);
        }

        /// <summary>
        /// Get the raw data from BPack accelerometer
        /// The BPack data are formated as:
        /// 
        /// "BPACK,,X,Y,Z,anaTS\n"
        /// 
        /// X, Y and Z are double values
        /// TS means timestamp for the analog events, formated by option "o"
        /// 
        /// </summary>
        /// <param name="x"> Acceleration data on the X axis</param>
        /// <param name="y"> Acceleration data on the Y axis</param>
        /// <param name="z"> Acceleration data on the Z axis</param>
        /// <returns></returns>
        public void GetBPackRawData(int which, out double x, out double y, out double z)
        {
            SendMessage(String.Format("BPack?{0}?\n", which));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });
                //Skip "BPACK,,"
                x = System.Convert.ToDouble(sections[2]);
                y = System.Convert.ToDouble(sections[3]);
                z = System.Convert.ToDouble(sections[4]);
                //Skip the analog timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //anaTS = DateTime.Parse(sections[5]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one bpack */
        public void GetBPackRawData(out double x, out double y, out double z)
        {
            GetBPackRawData(1, out x, out y, out z);
        }

        /// <summary>
        /// Get BPack Accelerometer tilt data 
        /// </summary>
        /// <param name="pitch">pitch: down(+) and up(-)</param>
        /// <param name="roll">roll: left(-) and right(+)</param>
        public void GetBPackTiltData(int which, out double pitch, out double roll)
        {
            double x, y, z;
            GetBPackRawData(which, out x, out y, out z);
            //BPack coordinate system: X to the right, Y forward, Z downward
            //So we need to do some convert
            ComputeTilt(x, y, z, out pitch, out roll);
        }

        /* An overload which requires no 'which' parameter, used when there is only one bpack */
        public void GetBPackTiltData(out double pitch, out double roll)
        {
            GetBPackTiltData(1, out pitch, out roll);
        }

        /// <summary>
        /// Get Wiimote acceleration raw data
        /// The Wiimote data are formated as:
        /// 
        /// "WIIMOTE,,battery,X,Y,Z,anaTS,ABOTUDLRPMH,buttTS\n"
        /// 
        /// battery holds the battery level of the Wiimote
        /// X, Y, Z are all double values indicating the acceleration data
        /// irX, irY, irSize refer to the X, Y and size data of the n'th IR sensor spot
        /// TS stands for timestamp for analog and button events
        /// A(button A), B(B), O(One), T(Two), U(Up), D(Down), L(Left), R(Right), P(Plus), M(Minus), H(Home)
        /// Uppercase for pressed, lowercase for released
        /// 
        /// </summary>
        /// <param name="accel"> Acceleratoin data on three axes</param>
        /// <param name="which"> Which Wiimote is it?</param>
        /// <param name="buttons"> Button data of that Wiimote</param>
        public void GetWiimoteRawData(int which, ref double[] accel, out String buttons)
        {
            if (accel.Length != 3)
            {
                throw new Exception("ERROR: \"acceleration length must be 3");
            }
            SendMessage(String.Format("Wiimote?{0}?\n", which));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });
                //Skip "WIIMOTE,,"
                //Skip battery level, uncomment this and add a out double variable to get battery level
                //battery = System.Convert.ToDouble(sections[2]);
                accel[0] = System.Convert.ToDouble(sections[3]);
                accel[1] = System.Convert.ToDouble(sections[4]);
                accel[2] = System.Convert.ToDouble(sections[5]);
                //Skip the analog timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //anaTS = DateTime.Parse(sections[6]);
                buttons = sections[7];
                //Skip the button timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //buttTS = DateTime.Parse(sections[8]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one wiimote */
        public void GetWiimoteRawData(ref double[] accel, out String buttons)
        {
            GetWiimoteRawData(1, ref accel, out buttons);
        }

        /// <summary>
        /// Get Wiimote tilt data and button data
        /// </summary>
        /// <param name="pitch"> Pitch down (+), pitch up (-)</param>
        /// <param name="roll"> Roll left (-), roll right (+)</param>
        /// <param name="buttons"> Uppercase for pressed, lowercase for released</param>
        public void GetWiimoteTiltData(int which, out double pitch, out double roll, out String buttons)
        {
            double[] acceleration = new double[3];
            GetWiimoteRawData(which, ref acceleration, out buttons);
            //BPack coordinate system: X to the right, Y forward, Z downward
            //So we need to do some convert
            ComputeTilt(acceleration[0], acceleration[1], acceleration[2], out pitch, out roll);
        }

        /* An overload which requires no 'which' parameter, used when there is only one wiimote */
        public void GetWiimoteTiltData(out double pitch, out double roll, out String buttons)
        {
            GetWiimoteTiltData(1, out pitch, out roll, out buttons);
        }

        /// <summary>
        /// Get WiiFit Raw data
        /// </summary>
        /// <param name="topLeft"> Pressure sensor data on the top left corner</param>
        /// <param name="topRight"> Pressure sensor data on the top right corner</param>
        /// <param name="bottomLeft"> Pressure sensor data on the bottom left corner</param>
        /// <param name="bottomRight"> Pressure sensor data on the bottom right corner</param>
        /// <param name="buttons"> Button A: "A" for press, "a" for release</param>
        /// <returns></returns>
        public void GetWiiFitRawData(int which, out double topLeft, out double topRight,
            out double bottomLeft, out double bottomRight, out String buttons)
        {
            SendMessage(String.Format("WiiFit?{0}?\n", which));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });
                //Skip "WIIFIT,,"
                topLeft = System.Convert.ToDouble(sections[2]);
                topRight = System.Convert.ToDouble(sections[3]);
                bottomLeft = System.Convert.ToDouble(sections[4]);
                bottomRight = System.Convert.ToDouble(sections[5]);
                //Skip the analog timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //anaTS = DateTime.Parse(sections[6]);
                buttons = sections[7];
                //Skip the button timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //buttTS = DateTime.Parse(sections[8]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one wiifit */
        public void GetWiiFitRawData(out double topLeft, out double topRight,
            out double bottomLeft, out double bottomRight, out String buttons)
        {
            GetWiiFitRawData(1, out topLeft, out topRight, out bottomLeft, out bottomRight, out buttons);
        }

        /// <summary>
        /// Get WiiFit Center of Gravity data
        /// </summary>
        /// <param name="weight"> Weight of the person in Kg</param>
        /// <param name="gravX"> Gravity along the X axis (the longer one), extends to the right</param>
        /// <param name="gravY"> Gravity along the Y axis (the shorter one), extends upwards</param>
        /// <param name="buttons"> Button A: uppercase for press, lowercase for release</param>
        public void GetWiiFitGravityData(int which, out double weight, out double gravX, out double gravY, out String buttons)
        {
            // These computation formulas are borrowed from the WiimoteLib by Brian Peek
            float BSL = 43.0f;   // Length between board censors
            float BSW = 24.0f;   // Width between board censors
            double tl = 0.0, tr = 0.0, bl = 0.0, br = 0.0;
            GetWiiFitRawData(which, out tl, out tr, out bl, out br, out buttons);
            float kx = (float)((tl + bl) / (tr + br));
            float ky = (float)((tl + tr) / (bl + br));
            gravX = (kx - 1.0f) / (kx + 1.0f) * (-BSL / 2.0f);
            gravY = (ky - 1.0f) / (ky + 1.0f) * (-BSW / 2.0f);
            weight = (float)(tl + tr + bl + br) / 4.0;
        }

        /* An overload which requires no 'which' parameter, used when there is only one wiifit */
        public void GetWiiFitGravityData(out double weight, out double gravX, out double gravY, out String buttons)
        {
            GetWiiFitGravityData(1, out weight, out gravX, out gravY, out buttons);
        }

        /// <summary>
        /// Get the joint data from Microsoft Kinect
        /// Using OpenNI, NITE and the VRPN server (FAAST), the Kinect gives position and quaternion data
        ///  of 24 joints, including:
        ///  Head, Neck, Torso, Waist, 
        ///  Left Collar, Left Shoulder, Left Elbow, Left Wrist, Left Hand, Left Fingertip,
        ///  Right Collar, Right Shoulder, Right Elbow, Right Wrist, Right Hand, Right Fingertip,
        ///  Left Hip, Left Knee, Left Ankle, Left Foot,
        ///  Right Hip, Right Knee, Right Ankle, Right Foot.
        ///  
        /// So the format UIVA bounces back is like:
        /// "KINECT,,posX,posY,posZ,quatX,quatY,quatZ,quatW,TS\n"
        /// posX, posY, posZ, quatX, quatY, quatZ, quatW are all double values
        /// TS means timestamp
        /// 
        /// </summary>
        /// <param name="joint"> Which joint are of interest</param>
        /// <param name="positions"> Hold the returned positions value, must be length 3</param>
        /// <param name="quaternions"> Hold the returned quaternions value, must be length 4</param>
        public void GetKinectJointData(int which, int joint, ref double[] positions, ref double[] quaternions)
        {
            if (positions.Length != 3 || quaternions.Length != 4)
            {
                throw new Exception("ERROR: \"positions\" length must be 3, and \"quaternions\" length must be 4");
            }
            SendMessage(String.Format("Kinect?{0}?{1}?\n", which, joint));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });
                //Skip "KINECT,,"
                positions[0] = System.Convert.ToDouble(sections[2]);
                positions[1] = System.Convert.ToDouble(sections[3]);
                positions[2] = System.Convert.ToDouble(sections[4]);
                quaternions[0] = System.Convert.ToDouble(sections[5]);
                quaternions[1] = System.Convert.ToDouble(sections[6]);
                quaternions[2] = System.Convert.ToDouble(sections[7]);
                quaternions[3] = System.Convert.ToDouble(sections[8]);
                //Skip the timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //TS = DateTime.Parse(sections[9]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one kinect */
        public void GetKinectJointData(int joint, ref double[] positions, ref double[] quaternions)
        {
            GetKinectJointData(1, joint, ref positions, ref quaternions);
        }

        /// <summary>
        /// Get data from PNI SpacePoint Fusion 9-axis orientation sensor
        /// The format of received message is:
        /// "FUSION,,quatX,quatY,quatZ,quatW,trkTS,buttons,buttTS\n"
        /// quatX, quatY, quatZ and quatW are all double values
        /// TS means time stamp
        /// </summary>
        /// <param name="which"> which sensor</param>
        /// <param name="quaternions"> quaternion output</param>
        /// <param name="buttons"> button output</param>
        public void GetSpacePointFusionData(int which, ref double[] quaternions, out String buttons)
        {
            if (quaternions.Length != 4)
            {
                throw new Exception("ERROR: \"quaternions\" length must be 4");
            }
            SendMessage(String.Format("Fusion?{0}?\n", which));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });
                //Skip "FUSION,,"
                quaternions[0] = System.Convert.ToDouble(sections[2]);
                quaternions[1] = System.Convert.ToDouble(sections[3]);
                quaternions[2] = System.Convert.ToDouble(sections[4]);
                quaternions[3] = System.Convert.ToDouble(sections[5]);
                //Skip the timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //trkTS = DateTime.Parse(sections[6]);
                buttons = sections[7];
                //Skip the button timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //buttTS = DateTime.Parse(sections[8]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one SpacePoint Fusion */
        public void GetSpacePointFusionData(ref double[] quaternions, out String buttons)
        {
            GetSpacePointFusionData(1, ref quaternions, out buttons);
        }

        /// <summary>
        /// Get the LED position data from PhaseSpace Impulse Motion Capture System
        /// Using OWL Socket, VRPN server, the PhaseSpace gives position data of specified LED
        ///  of up to 32 LEDs.
        ///  
        /// So the format UIVA bounces back is like:
        /// "PHASESPACE,,posX,posY,posZ,quatX,quatY,quatZ,quatW,TS\n"
        /// posX, posY, posZ are all double values
        /// TS means timestamp
        /// 
        /// </summary>
        /// <param name="targetID"> Which target is of interest</param>
        /// <param name="positions"> Hold the returned positions value, must be length 3</param>
        public void GetPhaseSpaceLEDData(int which, int targetID, ref double[] positions, ref double[] quaternions)
        {
            if (positions.Length != 3 || quaternions.Length != 4)
            {
                throw new Exception("ERROR: \"positions\" length must be 3 and \"quaternions\" length must be 4");
            }
            SendMessage(String.Format("PhaseSpace?{0}?{1}?\n", which, targetID));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });
                //Skip "PHASESPACE,,"
                positions[0] = System.Convert.ToDouble(sections[2]);
                positions[1] = System.Convert.ToDouble(sections[3]);
                positions[2] = System.Convert.ToDouble(sections[4]);
                quaternions[0] = System.Convert.ToDouble(sections[5]);
                quaternions[1] = System.Convert.ToDouble(sections[6]);
                quaternions[2] = System.Convert.ToDouble(sections[7]);
                quaternions[3] = System.Convert.ToDouble(sections[8]);
                //Skip the timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //TS = DateTime.Parse(sections[9]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one phasespace */
        public void GetPhaseSpaceLEDData(int ledID, ref double[] positions, ref double[] quaternions)
        {
            GetPhaseSpaceLEDData(1, ledID, ref positions, ref quaternions);
        }

        /// <summary>
        /// Get data from Oculus Rift 3-axis orientation sensor
        /// The format of received message is:
        /// "FUSION,,quatX,quatY,quatZ,quatW,trkTS,buttons,buttTS\n"
        /// quatX, quatY, quatZ and quatW are all double values
        /// TS means time stamp
        /// </summary>
        /// <param name="which"> which sensor</param>
        /// <param name="quaternions"> quaternion output</param>
        /// <param name="buttons"> button output</param>
        public void GetOculusRiftData(int which, ref double[] quaternionParts)
        {
            if (quaternionParts.Length != 4)
            {
                throw new Exception("ERROR: \"quaternionParts\" length must be 4");
            }
            SendMessage(String.Format("OculusRift?{0}?\n", which));
            ReceiveMessage();
            try
            {
                string[] sections = recStr.Split(new char[] { ',' });
                //Skip "FUSION,,"
                quaternionParts[0] = System.Convert.ToDouble(sections[2]);
                quaternionParts[1] = System.Convert.ToDouble(sections[3]);
                quaternionParts[2] = System.Convert.ToDouble(sections[4]);
                quaternionParts[3] = System.Convert.ToDouble(sections[5]);
                //Skip the timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //trkTS = DateTime.Parse(sections[6]);
                //Skip the button timestamp, uncomment this line and add a corresponding out DateTime variable to test time delay
                //buttTS = DateTime.Parse(sections[8]);
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString() + "\n\nRECEIVED FROM UIVA_SERVER: " + recStr);
            }
        }

        /* An overload which requires no 'which' parameter, used when there is only one SpacePoint Fusion */
        public void GetOculusRiftData(ref double[] quaternionParts)
        {
            GetOculusRiftData(1, ref quaternionParts);
        }

        /// <summary>
        /// Compute the tilt given three gravity values
        /// Input values should be converted to the indicated coordinate layout
        /// </summary>
        /// <param name="xToRight">X axis point to the right</param>
        /// <param name="yBackwards">Y axis point backward</param>
        /// <param name="zUpwards">Z axis point upward</param>
        /// <param name="pitch">Pitch down is positive</param>
        /// <param name="roll">Roll left is negative</param>
        private void ComputeTilt(double xToRight, double yBackwards, double zUpwards,
                        out double pitch, out double roll)
        {
            double[] gravs = new double[3];
            gravs[X] = xToRight;
            gravs[Y] = yBackwards;
            gravs[Z] = zUpwards;

            //Compute roll angle
            double sqrtY2Z2 = Math.Sqrt(Math.Pow(gravs[Y], 2.0) +
                Math.Pow(gravs[Z], 2.0));
            //Prevent dividing by zero
            if (sqrtY2Z2 == 0.0)
            {
                if (gravs[X] < 0.0)
                {
                    roll = 90.0;
                }
                else
                {
                    roll = -90.0;
                }
            }
            else
            {
                roll = Math.Atan(gravs[X] / sqrtY2Z2);  //The math that works, compute roll angle
                roll *= (180.0 / Math.PI);              //Convert to degree
            }

            //Compute pitch angle
            double sqrtX2Z2 = Math.Sqrt(Math.Pow(gravs[X], 2.0) +
                Math.Pow(gravs[Z], 2.0));
            //Prevent dividing by zero
            if (sqrtX2Z2 == 0.0)
            {
                if (gravs[Y] < 0.0)
                {
                    pitch = 90.0;
                }
                else
                {
                    pitch = -90.0;
                }
            }
            else
            {
                pitch = -Math.Atan(gravs[Y] / sqrtX2Z2);    //The math that works, compute pitch angle
                pitch *= (180.0 / Math.PI);                 //Convert to degree
            }
        }

        /// <summary>
        /// Disconnect from UIVA
        /// </summary>
        public void Disconnect()
        {
            SendMessage("Bye?\n");      //Send disconnect request
        }
    }
}



