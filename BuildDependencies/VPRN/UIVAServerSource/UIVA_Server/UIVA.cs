///-----------------------------------------------------------------------------------
///-----------------------------------------------------------------------------------
/// UIVA Server - Unity Indie VRPN Adapter 
/// 
/// File: UIVA.cs
/// 
/// Description:
///     Each device has a UIVA_Device class, which contains data buffers and callbacks 
///     functions.
/// 
/// Author: 
///     Jia Wang (wangjia@wpi.edu)
///     Human Interaction in Virtual Enviroments (HIVE) Lab
///     Department of Computer Science
///     Worcester Polytechnic Institute
/// 
/// History: (1.0) 02/05/2011  by  Jia Wang           
///-----------------------------------------------------------------------------------
///-----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vrpn;

namespace UIVA_Server
{
    // Wiimote extension devices
    enum WiimoteExtensionType { WIIMOTE, NUNCHUK, CLASSIC_CONTROLLER, GUITAR_HERO, BALANCE_BOARD, MOTIONPLUS };

    class UIVA_BPack
    {
        // Remote
        AnalogRemote remote;

        // Data buffers
        public double x;    // Acceleration data on X axis
        public double y;    // Acceleration data on X axis
        public double z;    // Acceleration data on X axis
        public DateTime timeStamp;

        public UIVA_BPack(String ID)
        {
            remote = new AnalogRemote(ID);
            // Register the callback function for Analog_Remote
            remote.AnalogChanged += new AnalogChangeEventHandler(BPackAnalogChanged);
            remote.MuteWarnings = true;
        }

        /* Analog event handler */
        private void BPackAnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            timeStamp = e.Time.ToLocalTime();
            x = e.Channels[0];
            y = e.Channels[1];
            z = e.Channels[2];
        }

        /* BPack_Remote mainloop() */
        public void Update()
        {
            remote.Update();
        }

        /* Encode data to UIVA_Client */
        public String Encode()
        {
            return String.Format("BPACK,,{0},{1},{2},{3}\n", x, y, z, timeStamp.ToString("o"));
        }
    }

    class UIVA_Mouse
    {
        // Remote
        AnalogRemote anaRemote;
        ButtonRemote buttRemote;

        // Data buffers
        public double x;    // Coordinate data on the X axis
        public double y;    // Coordinate data on the Y axis
        public bool buttLeft;   // Left mouse button
        public bool buttRight;  // Right mouse button
        public bool buttMiddle; // Middle mouse button
        public DateTime anaTimeStamp;   // Time event for analog events
        public DateTime buttTimeStamp;   // Time event for button events

        public UIVA_Mouse(String ID)
        {
            anaRemote = new AnalogRemote(ID);
            anaRemote.AnalogChanged += new AnalogChangeEventHandler(MouseAnalogChanged);
            anaRemote.MuteWarnings = true;

            buttRemote = new ButtonRemote(ID);
            buttRemote.ButtonChanged += new ButtonChangeEventHandler(MouseButtonChanged);
            buttRemote.MuteWarnings = true;
        }

        /* Analog event handler */
        private void MouseAnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            anaTimeStamp = e.Time.ToLocalTime();
            x = e.Channels[0];
            y = e.Channels[1];
        }

        /* Button event handler */
        private void MouseButtonChanged(object sender, ButtonChangeEventArgs e)
        {
            buttTimeStamp = e.Time.ToLocalTime();
            switch (e.Button)
            {
                case 0:
                    if (e.IsPressed) { buttLeft = true; } else { buttLeft = false; }
                    break;
                case 1:
                    if (e.IsPressed) { buttMiddle = true; } else { buttMiddle = false; }
                    break;
                case 2:
                    if (e.IsPressed) { buttRight = true; } else { buttRight = true; }
                    break;
                default:
                    throw new Exception("ERROR: UIVA_Mouse: MouseButtonChanged: Mouse button unsupported.");
            }
        }

        /* Mouse_Remote mainloop() */
        public void Update()
        {
            anaRemote.Update();
            buttRemote.Update();
        }

        /* Encode data to UIVA_Client */
        public String Encode()
        {
            String buttStr = "";
            if (buttLeft) { buttStr += ",L"; } else { buttStr += ",l"; }
            if (buttMiddle) { buttStr += "M"; } else { buttStr += "m"; }
            if (buttRight) { buttStr += "R,"; } else { buttStr += "r,"; }
            buttStr += buttTimeStamp.ToString("o");
            buttStr += "\n";
            String anaStr = String.Format("MOUSE,,{0},{1},{2}", x, y, anaTimeStamp.ToString("o"));
            return anaStr + buttStr;
        }
    }

    class UIVA_Wiimote
    {
        // Remote
        AnalogRemote anaRemote;
        ButtonRemote buttRemote;

        // If it is parasitized by WiiFit
        private bool parasitized = false;

        // Data buffers
        public double battery;    // Battery level
        public double accelX;    // Acceleration on the X axis
        public double accelY;    // Acceleration on the Y axis
        public double accelZ;    // Acceleration on the Z axis
        public DateTime anaTimeStamp;   // Timestamp for analog data

        public bool buttA;       // Button A
        public bool buttB;       // Button B
        public bool buttUp;      // Button Up
        public bool buttDown;    // Button Down
        public bool buttLeft;    // Button Left
        public bool buttRight;   // Button Right
        public bool buttPlus;    // Button Plus
        public bool buttMinus;   // Button Minus
        public bool buttHome;    // Button Home
        public bool buttOne;     // Button One
        public bool buttTwo;     // Button Two
        public DateTime buttTimeStamp;  // Timestamp for button data

        public double bbTopLeft;     // Top left pressure sensor value of the balance board
        public double bbTopRight;    // Top right pressure sensor value of the balance board
        public double bbBottomLeft;  // Bottom left pressure sensor value of the balance board
        public double bbBottomRight; // Bottom right pressure sensor value of the balance board

        public UIVA_Wiimote(String ID)
        {
            anaRemote = new AnalogRemote(ID);
            // Register the callback function for AnalogRemote
            anaRemote.AnalogChanged += new AnalogChangeEventHandler(WiimoteAnalogChanged);
            anaRemote.MuteWarnings = true;

            buttRemote = new ButtonRemote(ID);
            // Register the callback function for ButtonRemote
            buttRemote.ButtonChanged += new ButtonChangeEventHandler(WiimoteButtonChanged);
            buttRemote.MuteWarnings = true;
        }

        /* Analog event handler */
        private void WiimoteAnalogChanged(object sender, AnalogChangeEventArgs e)
        {
            anaTimeStamp = e.Time.ToLocalTime();
            if (e.Channels[0] == -999.0f)
            {
                parasitized = true;
                // This Wiimote is parasitized by Wiifit
                bbTopLeft = e.Channels[1];      // WiiFit pressure sensor data (top left corner)
                bbTopRight = e.Channels[2];     // WiiFit pressure sensor data (top right corner)
                bbBottomLeft = e.Channels[3];   // WiiFit pressure sensor data (bottom left corner)
                bbBottomRight = e.Channels[4];  // WiiFit pressure sensor data (bottom right corner)
            }
            else
            {
                parasitized = false;
                battery = e.Channels[0];    // Wiimote battery info
                accelX = e.Channels[1];     // Wiimote acceleration on X axis
                accelY = e.Channels[2];     // Wiimote acceleration on Y axis
                accelZ = e.Channels[3];     // Wiimote acceleration on Z axis
            }
        }

        /* Button event handler */
        private void WiimoteButtonChanged(object sender, ButtonChangeEventArgs e)
        {
            buttTimeStamp = e.Time.ToLocalTime();
            switch (e.Button)
            {
                case 0: // Home or A (if paratisized by WiiFit)
                    if (e.IsPressed) { buttHome = true; } else { buttHome = false; }
                    break;
                case 1: // One
                    if (e.IsPressed) { buttOne = true; } else { buttOne = false; }
                    break;
                case 2: // Two
                    if (e.IsPressed) { buttTwo = true; } else { buttTwo = false; }
                    break;
                case 3: // A
                    if (e.IsPressed) { buttA = true; } else { buttA = false; }
                    break;
                case 4: // B
                    if (e.IsPressed) { buttB = true; } else { buttB = false; }
                    break;
                case 11: // Minus
                    if (e.IsPressed) { buttMinus = true; } else { buttMinus = false; }
                    break;
                case 6: // Plus
                    if (e.IsPressed) { buttPlus = true; } else { buttPlus = false; }
                    break;
                case 7: // Left
                    if (e.IsPressed) { buttLeft = true; } else { buttLeft = false; }
                    break;
                case 8: // Right
                    if (e.IsPressed) { buttRight = true; } else { buttRight = false; }
                    break;
                case 9: // Down
                    if (e.IsPressed) { buttDown = true; } else { buttDown = false; }
                    break;
                case 10: // Up
                    if (e.IsPressed) { buttUp = true; } else { buttUp = false; }
                    break;
                default:
                    break;
            }
        }

        /* Wiimote_Remote mainloop() */
        public WiimoteExtensionType Update()
        {
            anaRemote.Update();
            buttRemote.Update();
            if (parasitized)
            {
                return WiimoteExtensionType.BALANCE_BOARD;
            }
            else
            {
                return WiimoteExtensionType.WIIMOTE;
            }
        }

        /* Encode Wiimote data to UIVA_Client */
        public String Encode()
        {
            String anaStr = "";
            String buttStr = "";
            if (parasitized)
            {
                anaStr = String.Format("WIIFIT,,{0},{1},{2},{3},{4}", bbTopLeft, bbTopRight,
                    bbBottomLeft, bbBottomRight, anaTimeStamp.ToString("o"));
                if (buttHome) { buttStr += ",A,"; } else { buttStr += ",a,"; }
                buttStr += buttTimeStamp.ToString("o");
                buttStr += "\n";
            }
            else
            {
                // Encode the Wiimote acceleromter data
                anaStr = String.Format("WIIMOTE,,{0},{1},{2},{3},{4}",
                    battery, accelX, accelY, accelZ, anaTimeStamp.ToString("o"));

                // Encode Wiimote states of accelerometer data and button data
                // Uppercase for button press, lowercase for button release
                if (buttA) { buttStr += ",A"; } else { buttStr += ",a"; }
                if (buttB) { buttStr += "B"; } else { buttStr += "b"; }
                if (buttOne) { buttStr += "O"; } else { buttStr += "o"; }
                if (buttTwo) { buttStr += "T"; } else { buttStr += "t"; }
                if (buttUp) { buttStr += "U"; } else { buttStr += "u"; }
                if (buttDown) { buttStr += "D"; } else { buttStr += "d"; }
                if (buttLeft) { buttStr += "L"; } else { buttStr += "l"; }
                if (buttRight) { buttStr += "R"; } else { buttStr += "r"; }
                if (buttPlus) { buttStr += "P"; } else { buttStr += "p"; }
                if (buttMinus) { buttStr += "M"; } else { buttStr += "m"; }
                if (buttHome) { buttStr += "H,"; } else { buttStr += "h,"; }
                buttStr += buttTimeStamp.ToString("o");
                buttStr += "\n";
            }

            return anaStr + buttStr;
        }

        /* Encode WiiFit data to UIVA_Client */
        public String Encode_Extension(WiimoteExtensionType ext)
        {
            String ret = "";
            switch (ext)
            {
                case WiimoteExtensionType.BALANCE_BOARD:
                    // Encode Wii Fit balance board data
                    ret = String.Format("WIIFIT,,{0},{1},{2},{3},{4}\n",
                        bbTopLeft, bbTopRight, bbBottomLeft, bbBottomRight, anaTimeStamp.ToString("o"));
                    break;

                //--------------- TO_DO: Implement other extension devices support --------------//
                case WiimoteExtensionType.NUNCHUK:
                    ret = String.Format("NUNCHUK_NOT_SUPPORTED_YET,,");
                    break;
                case WiimoteExtensionType.CLASSIC_CONTROLLER:
                    ret = String.Format("CLASSIC_NOT_SUPPORTED_YET,,");
                    break;
                case WiimoteExtensionType.MOTIONPLUS:
                    ret = String.Format("MOTIONPLUS_NOT_SUPPORTED_YET,,");
                    break;
                case WiimoteExtensionType.GUITAR_HERO:
                    ret = String.Format("GUITAR_HERO_NOT_SUPPORTED_YET,,");
                    break;
                default:
                    break;
            }
            return ret;
        }
    }

    class UIVA_SpacePointFusion
    {
        // Tracker Remote
        TrackerRemote trkRemote;
        ButtonRemote buttRemote;

        // Quaternion data and buttons data
        public Quaternion quat;
        public DateTime trkTimeStamp;
        public bool buttA;
        public bool buttB;
        public DateTime buttTimeStamp;

        public UIVA_SpacePointFusion(String ID)
        {
            trkRemote = new TrackerRemote(ID);
            // Register the callback function for TrackerRemote
            trkRemote.PositionChanged += new TrackerChangeEventHandler(FusionTrackerChanged);
            trkRemote.MuteWarnings = true;

            buttRemote = new ButtonRemote(ID);
            // Register the callback function for ButtonRemote
            buttRemote.ButtonChanged += new ButtonChangeEventHandler(FusionButtonChanged);
            buttRemote.MuteWarnings = true;
        }

        /* Tracker event handler */
        private void FusionTrackerChanged(object sender, TrackerChangeEventArgs e)
        {
            trkTimeStamp = e.Time.ToLocalTime();
            quat = e.Orientation;
        }

        /* Button event handler */
        private void FusionButtonChanged(object sender, ButtonChangeEventArgs e)
        {
            buttTimeStamp = e.Time.ToLocalTime();
            switch (e.Button)
            {
                case 0: // Left button
                    if (e.IsPressed) { buttA = true; } else { buttA = false; }
                    break;
                case 1: // Right button
                    if (e.IsPressed) { buttB = true; } else { buttB = false; }
                    break;
                default:
                    break;
            }
        }

        /* Tracker_Remote mainloop() */
        public void Update()
        {
            trkRemote.Update();
            buttRemote.Update();
        }

        /* Encode data to UIVA_Client */
        public String Encode()
        {
            String trkStr = "";
            String buttStr = "";
            // Encode the Fusion quaternion data
            trkStr = String.Format("FUSION,,{0},{1},{2},{3},{4}",
                quat.X, quat.Y, quat.Z, quat.W, trkTimeStamp.ToString("o"));

            // Encode Wiimote states of accelerometer data and button data
            // Uppercase for button press, lowercase for button release
            if (buttA) { buttStr += ",A"; } else { buttStr += ",a"; }
            if (buttB) { buttStr += "B,"; } else { buttStr += "b,"; }
            buttStr += buttTimeStamp.ToString("o");
            buttStr += "\n";

            return trkStr + buttStr;
        }
    }

    class UIVA_Kinect
    {
        // Tracker Remote
        TrackerRemote remote;

        // Data buffers for the 24 joints
        public Vector3[] pos;
        public Quaternion[] quat;
        public DateTime[] timeStamp;

        public UIVA_Kinect(String ID)
        {
            pos = new Vector3[24];
            quat = new Quaternion[24];
            timeStamp = new DateTime[24];
            remote = new TrackerRemote(ID);
            // Register the callback function for TrackerRemote
            remote.PositionChanged += new TrackerChangeEventHandler(KinectTrackerChanged);
            remote.MuteWarnings = true;
        }

        /* Analog event handler */
        private void KinectTrackerChanged(object sender, TrackerChangeEventArgs e)
        {
            int i = e.Sensor;
            pos[i] = e.Position;
            quat[i] = e.Orientation;
            timeStamp[i] = e.Time.ToLocalTime();
        }

        /* Tracker_Remote mainloop() */
        public void Update()
        {
            remote.Update();
        }

        /* Encode data to UIVA_Client 
        * 
        * Joint coding for Kinect (from 0 to 23)
        * Head, Neck, Torso, Waist, 
        * Left Collar, Left Shoulder, Left Elbow, Left Wrist, Left Hand, Left Fingertip,
        * Right Collar, Right Shoulder, Right Elbow, Right Wrist, Right Hand, Right Fingertip, 
        * Left Hip, Left Knee, Left Ankle, Left Foot
        * Right Hip, Right Knee, Right Ankle, Right Foot
        * 
        * */
        public String Encode(int joint)
        {
            return String.Format("KINECT,,{0},{1},{2},{3},{4},{5},{6},{7}\n",
                pos[joint].X, pos[joint].Y, pos[joint].Z,
                quat[joint].X, quat[joint].Y, quat[joint].Z, quat[joint].W,
                timeStamp[joint].ToString("o"));
        }
    }

    class UIVA_PhaseSpace
    {
        // Tracker Remote
        TrackerRemote remote;

        // Data buffers for the LED trackers
        public Vector3[] pos;
        public Quaternion[] quat;
        public DateTime[] timeStamp;

        public UIVA_PhaseSpace(String ID)
        {
            pos = new Vector3[32];
            quat = new Quaternion[32];
            timeStamp = new DateTime[32];
            remote = new TrackerRemote(ID);
            // Register the callback function for TrackerRemote
            remote.PositionChanged += new TrackerChangeEventHandler(PhaseSpaceTrackerChanged);
            remote.MuteWarnings = true;
        }

        /* Tracker update event handler */
        private void PhaseSpaceTrackerChanged(object sender, TrackerChangeEventArgs e)
        {
            int i = e.Sensor;
            pos[i] = e.Position;
            quat[i] = e.Orientation;
            timeStamp[i] = e.Time.ToLocalTime();
        }

        /* Tracker_Remote mainloop() */
        public void Update()
        {
            remote.Update();
        }

        /* Encode data to UIVA_Client 
        * LED coding for PhaseSpace (from 0 to 31)
        * */
        public String Encode(int ledID)
        {
            return String.Format("PHASESPACE,,{0},{1},{2},{3},{4},{5},{6},{7}\n",
                pos[ledID].X, pos[ledID].Y, pos[ledID].Z,
                quat[ledID].X, quat[ledID].Y, quat[ledID].Z, quat[ledID].W,
                timeStamp[ledID].ToString("o"));
        }
    }

    class UIVA_OculusRift
    {
        // Tracker Remote
        TrackerRemote remote;

        // Data buffers for the LED trackers
        public Quaternion quat;
        public DateTime timeStamp;

        public UIVA_OculusRift(String ID)
        {
            quat = new Quaternion();
            timeStamp = new DateTime();
            remote = new TrackerRemote(ID);
            // Register the callback function for TrackerRemote
            remote.PositionChanged += new TrackerChangeEventHandler(OculusRiftTrackerChanged);
            remote.MuteWarnings = true;
        }

        /* Tracker update event handler */
        private void OculusRiftTrackerChanged(object sender, TrackerChangeEventArgs e)
        {
            quat = e.Orientation;
            timeStamp = e.Time.ToLocalTime();
        }

        /* Tracker_Remote mainloop() */
        public void Update()
        {
            remote.Update();
        }

        /* Encode data to UIVA_Client 
        * LED coding for PhaseSpace (from 0 to 31)
        * */
        public String Encode()
        {
            return String.Format("OCULUSRIFT,,{0},{1},{2},{3},{4}\n",
                quat.X, quat.Y, quat.Z, quat.W,
                timeStamp.ToString("o"));
        }
    }

}
