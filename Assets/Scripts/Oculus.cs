using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using UIVA;

public class Oculus : MonoBehaviour {

	private UIVA_Client UIVAClient;
	public Quaternion RiftQuaternion = Quaternion.identity;
	private Quaternion InitialRotation = Quaternion.identity;

	public bool RiftConnected = true;
	
	private Quaternion ConvertArrayToQuaternion(double[] quat)
	{
		Quaternion interQuat = new Quaternion((float)quat[0], (float)quat[1], (float)quat[2], (float)quat[3]);
		return interQuat ;// * Quaternion.FromToRotation(Vector3.forward, Vector3.up)) * Quaternion.AngleAxis(90, Vector3.left);/* * Quaternion.AngleAxis(180, Vector3.left);*///Quaternion.Euler(interQuat.eulerAngles.x, interQuat.eulerAngles.y, -interQuat.eulerAngles.z);
		
	}
	
	void Awake()
	{
		try
        {
            UIVAClient = new UIVA_Client("localhost");
			double[] quat = new double[4];
			UIVAClient.GetOculusRiftData(ref quat);
			InitialRotation = ConvertArrayToQuaternion(quat);
			RiftQuaternion = InitialRotation;
        }
        catch (Exception se)
        {
            Debug.LogWarning(se.ToString());
			RiftConnected = false;
        }	
	}
	
	void OnDestroy()
	{
		UIVAClient.Disconnect();
	}

    /// <summary>
    /// Test the functionality of UniWii by a console application
    /// </summary>
    void OnPreRender()
    {
		if (RiftConnected)
		{
	        double[] quat = new double[4];
	        UIVAClient.GetOculusRiftData(ref quat);
	        RiftQuaternion = ConvertArrayToQuaternion(quat);
		}
		//Quaternion interQuat = Quaternion.Inverse(InitialRotation) * RiftQuaternion;
		gameObject.transform.rotation = InitialRotation * Quaternion.Inverse(RiftQuaternion);// * Quaternion.AngleAxis(90, Vector3.up))) * Quaternion.AngleAxis(-90, Vector3.up);//Quaternion.Euler(interQuat.eulerAngles.x, interQuat.eulerAngles.y, interQuat.eulerAngles.z);
    }
}
