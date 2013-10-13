///-----------------------------------------------------------------------------------
///-----------------------------------------------------------------------------------
/// UIVA Server - Unity Indie VRPN Adapter 
/// 
/// Function:
/// 
///     UIVA is a middle-ware between VRPN and Unity. It enables games developed by Unity3D INDIE
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
/// History: (1.0) 02/05/2011  by  Jia Wang
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

//#define __VERBOSE__

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;            // For configuration file
using System.Net.Sockets;   // For communicating with Unity3D
using System.Net;
using Vrpn;                 // For communicating with VRPN server

namespace UIVA_Server      // Unity Indie VRPN Adapter
{
    class UIVA
    {
        private bool stopAll = false;

        // How many certain devices are alive.
        // If 2 mice are alive, UIVA_Client can request data from mouse 1 and 2
        // If miceAlive < the # of mouse request, then return a error
        private int miceAlive = 0;
        private int bpacksAlive = 0;
        private int kinectsAlive = 0;
        private int wiifitsAlive = 0;
        private int wiimotesAlive = 0;
        private int fusionsAlive = 0;
        private int phasespaceAlive = 0;

        // UIVA_Device objects
        private List<UIVA_Mouse> mice = new List<UIVA_Mouse>();
        private List<UIVA_BPack> bpacks = new List<UIVA_BPack>();
        private List<UIVA_Kinect> kinects = new List<UIVA_Kinect>();
        // WiiFit is essentially a Wiimote parasitized by a WiiFit,
        // and we don't know whether it is wm or wf until we receive a message from it
        // So we need to put them all together
        private List<UIVA_Wiimote> wiimotes = new List<UIVA_Wiimote>();
        private List<UIVA_SpacePointFusion> fusions = new List<UIVA_SpacePointFusion>();
        private List<UIVA_PhaseSpace> phasespaces = new List<UIVA_PhaseSpace>();

        // Socket stuff used for handling Unity communication
        private Socket socListener;                 // Socket listen to client request
        private Socket socWorker;                   // Socket handle connection with client
        private byte[] recBuffer = new byte[100];   // Receive buffer
        private string recStr = "";                 // Deciphered receive buffer
        private byte[] sendBuffer = new byte[100];  // Ciphered send buffer
        private string sendStr = "";                // String going to be send
        private ASCIIEncoding encoder;              // Encoding, decoding

        public UIVA()
        {
            // 1. Parse config file and wake up selected devices
            ParseConfigFile();

            // 2. Initiate Unity server and start its thread
            //Create a socket for listening connections
            socListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            //Create a socket for message transmissions
            socWorker = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            //Create a encoder to translate string into byte stream
            encoder = new ASCIIEncoding();
            //Port: 8881
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 8881);
            socListener.Bind(ipLocal);

            // 3. Start listening to Unity3D
            TalkToUnity();
        }

        /// <summary>
        /// Parse config file and activate the devices selected
        /// </summary>
        /// <returns></returns>
        private void ParseConfigFile()
        {
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine("                  UIVA 1.0: Unity Indie VRPN Adapter (Server)             ");
            Console.WriteLine("                                                                          ");
            Console.WriteLine("                      by Jia Wang (wangjia@wpi.edu)                       ");
            Console.WriteLine("                  HIVE Lab in Worcester Polytechnic Institute             ");
            Console.WriteLine("--------------------------------------------------------------------------\n");

            Console.WriteLine("Parsing configuration file...");
            StreamReader cfgFile;
            String line;
            cfgFile = File.OpenText(".\\UIVA_Server.cfg");
            line = cfgFile.ReadLine();
            while(line != null)
            {
                if(line.Length != 0 && line[0] != '#')
                {
                    String[] tokens;
                    tokens = line.Split(new char[] { ' ' });
                    String name = tokens[1];
                    switch(tokens[0])
                    {
                        case "DEV_BPACK":
                            bpacks.Add(new UIVA_BPack(name));
                            bpacksAlive++;
                            Console.WriteLine("^_^ < VRPN > --- BPack # {0} tracking started...", bpacksAlive);
                            break;
                        case "DEV_MOUSE":
                            mice.Add(new UIVA_Mouse(name));
                            miceAlive++;
                            Console.WriteLine("^_^ < VRPN > --- Mouse # {0} tracking started...", miceAlive);
                            break;
                        case "DEV_WIIMOTE":
                            wiimotes.Add(new UIVA_Wiimote(name));
                            wiimotesAlive++;
                            Console.WriteLine("^_^ < VRPN > --- Wiimote # {0} tracking started...", wiimotesAlive);
                            break;
                        case "DEV_WIIFIT":
                            wiimotes.Add(new UIVA_Wiimote(name));
                            wiifitsAlive++;
                            Console.WriteLine("^_^ < VRPN > --- WiiFit # {0} tracking started...", wiifitsAlive);
                            break;
                        case "DEV_KINECT":
                            kinects.Add(new UIVA_Kinect(name));
                            kinectsAlive++;
                            Console.WriteLine("^_^ < VRPN > --- Kinect # {0} tracking started...", kinectsAlive);
                            break;
                        case "DEV_FUSION":
                            fusions.Add(new UIVA_SpacePointFusion(name));
                            fusionsAlive++;
                            Console.WriteLine("^_^ < VRPN > --- SpacePoint Fusion # {0} tracking started...", fusionsAlive);
                            break;
                        case "DEV_PHASESPACE":
                            phasespaces.Add(new UIVA_PhaseSpace(name));
                            phasespaceAlive++;
                            Console.WriteLine("^_^ < VRPN > --- PhaseSpace # {0} tracking started...", phasespaceAlive);
                            break;
                        default:
                            throw new Exception("Config file error: invalid device name");
                    }
                }

                line = cfgFile.ReadLine();
            }
            Console.WriteLine("DONE!");
            cfgFile.Close();
        }

        /// <summary>
        /// Unity3D server thread, talk to Unity3D to respond requests
        /// </summary>
        /// <returns></returns>
        private void TalkToUnity()
        {
            bool connected = false;
            while (!stopAll)
            {
                if (!connected)
                {
                    //Can hold 4 client requests in the queue
                    socListener.Listen(4);
                    socWorker = socListener.Accept();
                    connected = true;
                    Console.WriteLine("\n^_^ < Unity3D > --- Connection Erected");
                }
                else
                {
                    try
                    {
                        socWorker.Receive(recBuffer);               //Get request message
                        recStr = encoder.GetString(recBuffer);      //Convert to string
                        int index = recStr.IndexOf('\n');           //Parse string: remove '\n'
                        recStr = recStr.Remove(index);
                        String[] tokens = recStr.Split(new char[] {'?'});
                        int which = 0;
                        if (tokens[0] != "Ready" && tokens[0] != "Bye")
                        {
                            which = System.Convert.ToInt32(tokens[1]);
                        }

                        switch (tokens[0])
                        {
                            case "Ready":          //Ready confirm
                                sendStr = "Ready!\n";
                                break;

                            case "Mouse":
                                if(which <= miceAlive)
                                {
                                    // Encode the data in a message
                                    mice[which - 1].Update();
                                    sendStr = mice[which - 1].Encode();
                                }else
                                {
                                    sendStr = "ERROR: mouse # " + which.ToString() + " is not turned on\n";
                                }
                                break;

                            case "BPack":
                                if(which <= bpacksAlive)
                                {
                                    // Encode the data in a message
                                    bpacks[which - 1].Update();
                                    sendStr = bpacks[which - 1].Encode(); 
                                }else
                                {
                                    sendStr = "ERROR: bpack # " + which.ToString() + " is not turned on\n";
                                }
                                break;

                            case "Wiimote":
                                if(which <= wiimotesAlive)
                                {
                                    int theOne = 0;
                                    // Encode the data in a message
                                    for (int i = 0; i < wiimotes.Count; ++i)
                                    {
                                        if(wiimotes[i].Update() == WiimoteExtensionType.WIIMOTE)
                                        {
                                            sendStr = wiimotes[i].Encode();
                                            if(++theOne == which)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if(theOne != which)
                                    {
                                        sendStr = "ERROR: wiimote # " + which.ToString() + " is not turned on\n";
                                    }

                                }else
                                {
                                    sendStr = "ERROR: wiimote # " + which.ToString() + " is not turned on\n";
                                }
                                break;

                            case "WiiFit":
                                if (which <= wiifitsAlive)
                                {
                                    int theOne = 0;
                                    // Encode the data in a message
                                    for (int i = 0; i < wiimotes.Count; ++i)
                                    {
                                        if (wiimotes[i].Update() == WiimoteExtensionType.BALANCE_BOARD)
                                        {
                                            sendStr = wiimotes[i].Encode();
                                            if (++theOne == which)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                    if (theOne != which)
                                    {
                                        sendStr = "ERROR: wiifit # " + which.ToString() + " is not turned on\n";
                                    }

                                }
                                else
                                {
                                    sendStr = "ERROR: wiifit # " + which.ToString() + " is not turned on\n";
                                }
                                break;

                            case "Kinect":
                                if(which <= kinectsAlive)
                                {
                                    // Encode data of the requested joint in a message,
                                    // as specified in tokens[1]
                                    kinects[which - 1].Update();
                                    sendStr = kinects[which - 1].Encode(System.Convert.ToInt32(tokens[2]));
                                }else
                                {
                                    sendStr = "ERROR: kinect # " + which.ToString() + " is not turned on\n";
                                }
                                break;

                            case "Fusion":
                                if (which <= fusionsAlive)
                                {
                                    // Encode data of the requested device
                                    fusions[which - 1].Update();
                                    sendStr = fusions[which - 1].Encode();
                                }else
                                {
                                    sendStr = "ERROR: SpacePoint Fusion # " + which.ToString() + " is not turned on\n";
                                }
                                break;
                                
                            case "PhaseSpace":
                                if (which <= phasespaceAlive)
                                {
                                    // Encode data of the requested LED in a message,
                                    // as specified in tokens[1]
                                    phasespaces[which - 1].Update();
                                    sendStr = phasespaces[which - 1].Encode(System.Convert.ToInt32(tokens[2]));
                                }
                                else
                                {
                                    sendStr = "ERROR: PhaseSpace # " + which.ToString() + " is not turned on\n";
                                }
                                break;

                            case "Bye":            //Disconnect confirm
                                sendStr = "Bye!\n";
                                connected = false;
                                Console.WriteLine("^_^ < Unity3D > --- Connection Ended");
                                break;

                            default:
                                sendStr = "ERROR: invalid request, what do you mean <" + recStr + ">\n";
                                break;
                        }

                        sendBuffer = encoder.GetBytes(sendStr); //Encode
                        socWorker.Send(sendBuffer);             //Send
                    }catch (Exception e)
                    {
                        Console.WriteLine("*.* < Unity3D > --- Connection Lost");
#if __VERBOSE__
                        Console.WriteLine(e.ToString());
#endif
                        connected = false;
                    }
                }

            }   // End of while(!stopAll) loop

        } // End of thread TalkToUnity()


        public static void Main(string[] argv)
        {
            UIVA theUIVA = new UIVA();
        }

    } // End of class UIVA

}// End of namespace UIVA
