using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using UIVA;

public class Oculus : MonoBehaviour {

	private static UIVA_Client UIVAClient;
	private static Quaternion RiftQuaternion = Quaternion.identity;
	private static Quaternion InitialRotation = Quaternion.identity;

	public static bool Connected = false;
	
	public static void ResetForward()
	{
		InitialRotation = GetQuaternion();
	}
	
	private static Quaternion ConvertArrayToQuaternion(double[] quat)
	{
		Quaternion interQuat = new Quaternion((float)quat[0], (float)quat[1], (float)quat[2], (float)quat[3]);
		return interQuat ;
	}
	
	public static void Connect()
	{
		try
        {
            UIVAClient = new UIVA_Client("localhost");
			Connected = true;
        }
        catch (Exception se)
        {
            Debug.LogWarning(se.ToString());
			Connected = false;
        }	
	}
	
	public static void Disconnect()
	{
		if(UIVAClient != null)
		{
			UIVAClient.Disconnect();
			Connected = false;
		}
	}
	
	public static Quaternion GetQuaternion() {
		if (Connected)
		{
	        double[] quat = new double[4];
	        UIVAClient.GetOculusRiftData(ref quat);
	        RiftQuaternion = ConvertArrayToQuaternion(quat);
		}
		
		Vector3 euler = RiftQuaternion.eulerAngles;
		
		return Quaternion.Euler(-euler.x, -euler.y, euler.z) * Quaternion.Inverse(InitialRotation);
	}
}
