using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using UIVA;

public class Oculus : ScriptableObject{

	private static UIVA_Client UIVAClient;
	private static Quaternion RiftQuaternion = Quaternion.identity;
	private static Quaternion InitialRotation = Quaternion.identity;

	public static bool Connected = false;
	
	private static Quaternion ConvertArrayToQuaternion(double[] quat)
	{
		Quaternion interQuat = new Quaternion((float)quat[0], (float)quat[1], (float)quat[2], (float)quat[3]);
		return interQuat ;// * Quaternion.FromToRotation(Vector3.forward, Vector3.up)) * Quaternion.AngleAxis(90, Vector3.left);/* * Quaternion.AngleAxis(180, Vector3.left);*///Quaternion.Euler(interQuat.eulerAngles.x, interQuat.eulerAngles.y, -interQuat.eulerAngles.z);
		
	}
	
	public static void Connect()
	{
		try
        {
            UIVAClient = new UIVA_Client("localhost");
			double[] quat = new double[4];
			UIVAClient.GetOculusRiftData(ref quat);
			InitialRotation = GetQuaternion();
			RiftQuaternion = InitialRotation;
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

    /// <summary>
    /// Test the functionality of UniWii by a console application
    /// </summary>
    /// TODO: Update to be on prerender
    /*void Update()
    {
		if (RiftConnected)
		{
	        double[] quat = new double[4];
	        UIVAClient.GetOculusRiftData(ref quat);
	        RiftQuaternion = ConvertArrayToQuaternion(quat);
		}
		Quaternion interQuat = InitialRotation * Quaternion.Inverse(RiftQuaternion);
		
		Vector3 euler = interQuat.eulerAngles;
		
		gameObject.transform.rotation = Quaternion.Euler(euler.x, euler.y, -euler.z);// * Quaternion.AngleAxis(90, Vector3.up))) * Quaternion.AngleAxis(-90, Vector3.up);//Quaternion.Euler(interQuat.eulerAngles.x, interQuat.eulerAngles.y, interQuat.eulerAngles.z);
    }*/
	
	public static Quaternion GetQuaternion() {
		if (Connected)
		{
	        double[] quat = new double[4];
	        UIVAClient.GetOculusRiftData(ref quat);
	        RiftQuaternion = ConvertArrayToQuaternion(quat);
		}
		Quaternion interQuat = RiftQuaternion;
		
		Vector3 euler = interQuat.eulerAngles;
		
		return  /*Quaternion.AngleAxis(-90, Vector3.forward) * Quaternion.Inverse(interQuat) * InitialRotation;*/ /*Quaternion.AngleAxis(180, Vector3.left)*/Quaternion.Euler(-euler.x, -euler.y, euler.z)* InitialRotation;
	}
}
