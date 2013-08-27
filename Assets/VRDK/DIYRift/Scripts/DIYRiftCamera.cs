/************************************************************
 *                                                          *
 *      VRDK - The Virtual-Reality Development Kit          *
 *                                                          *
 *      Package: Do-it-yourself Rift Kit Camera 1.0         *
 *                                                          *
 *      This package is offered free and open source.       *
 *      Please support us by 'liking' us on facebook at     *
 *      http://www.facebook.com/VRcade                      *
 *                                                          *
 *      Author: Dave Ruddell (dave@vrcade.com)              *
 *              Mind-Games Intermedia, LLC / VRcade         *
 *                                                          *
 ************************************************************/

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

public class DIYRiftCamera : MonoBehaviour 
{
	#region Public variables
	public GameObject GuiCamera;
	
	public float InteraxialDist = 65.0f;
	public float ZeroPrlxDist = 3.0f;
	public float InteraxialDivisor = 2000.0f;
	
	public bool ToedIn = false;
	public bool SwapLeftRight = false;
	public float HIT = 0.0f;

	public int guiOnlyLayer = 22;
	public int RenderOrderDepth = 0;
	
	public bool UseStereoShader = true;
	public Material StereoBarrelWarpMaterial;
	#endregion

	#region Private variables
	private GameObject _leftCamera;
	private GameObject _rightCamera;

	private RenderTexture _leftCamRT;
	private RenderTexture _rightCamRT;

	private float _offAxisFrustum = 0.0f;

	private bool _useStereoShaderPrev = true;
	private bool _initialized = false;
	#endregion

	void Awake()
	{
		initStereoCamera();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update()
	{
		#if UNITY_EDITOR
		if (UseStereoShader) {
			if (_useStereoShaderPrev == false) {
				initStereoCamera();
			}
		} else {
			camera.enabled = !EditorApplication.isPlaying;
			
			if (_useStereoShaderPrev == true) {
				releaseRenderTextures();
				setupStereo();
			}
		}
		_useStereoShaderPrev = UseStereoShader;
		#endif
		
		if (Application.isPlaying) {
			if (!_initialized) {
				_initialized = true;
			}
		} else {
			_initialized = false;
			setupStereo();
		}
		updateView();
	}
	
	#region Main Camera Functions
	private void updateView()
	{
		float axial = InteraxialDist/InteraxialDivisor;
		
		if (!SwapLeftRight) {
			_leftCamera.transform.position = transform.position + transform.TransformDirection(-axial, 0.0f, 0.0f);
			_rightCamera.transform.position = transform.position + transform.TransformDirection(axial, 0.0f, 0.0f);
		}
		else {
			_leftCamera.transform.position = transform.position + transform.TransformDirection(axial, 0.0f, 0.0f);
			_rightCamera.transform.position = transform.position + transform.TransformDirection(-axial, 0.0f, 0.0f);
		}
		
		if (ToedIn) {
			_leftCamera.camera.projectionMatrix = camera.projectionMatrix;
			_rightCamera.camera.projectionMatrix = camera.projectionMatrix;
			
			_leftCamera.transform.LookAt(transform.position + 
				(transform.TransformDirection (Vector3.forward) * ZeroPrlxDist));
			_rightCamera.transform.LookAt(transform.position + 
				(transform.TransformDirection (Vector3.forward) * ZeroPrlxDist));
		}
		else {
			_leftCamera.transform.rotation = transform.rotation; 
			_rightCamera.transform.rotation = transform.rotation;
			
			_leftCamera.camera.projectionMatrix = setProjectionMatrix(!SwapLeftRight, axial);
			_rightCamera.camera.projectionMatrix = setProjectionMatrix(SwapLeftRight, axial);
		}
	}
	
	// Calculate Stereo Projection Matrix
	private Matrix4x4 setProjectionMatrix(bool isLeftCam, float axial)
	{
		float left, right, a, b, FOVrad;
		float tempAspect = camera.aspect / 2.0f;
		FOVrad = camera.fieldOfView / 180.0f * Mathf.PI;

		a = camera.nearClipPlane * Mathf.Tan(FOVrad * 0.5f);
		b = camera.nearClipPlane / ZeroPrlxDist;
		
		if (isLeftCam) {
			left  = (-tempAspect * a) + (axial * b) + (HIT/100) + (_offAxisFrustum/100);
			right =	(tempAspect * a) + (axial * b) + (HIT/100) + (_offAxisFrustum/100);
		} else {
			left  = (-tempAspect * a) - (axial * b) - (HIT/100) + (_offAxisFrustum/100);
			right =	(tempAspect * a) - (axial * b) - (HIT/100) + (_offAxisFrustum/100);
		}
		return createPerspectiveMatrix(left, right, -a, a, camera.nearClipPlane, camera.farClipPlane);
	}
	
	private Matrix4x4 createPerspectiveMatrix(
		float left, float right, float bottom, float top, float near, float far)
	{
		float x =  (2.0f * near) / (right - left);
		float y =  (2.0f * near) / (top - bottom);
		float a =  (right + left) / (right - left);
		float b =  (top + bottom) / (top - bottom);
		float c = -(far + near) / (far - near);
		float d = -(2.0f * far * near) / (far - near);
		float e = -1.0f;
	
		Matrix4x4 m = new Matrix4x4();
		m[0,0] = x;  m[0,1] = 0;  m[0,2] = a;  m[0,3] = 0;
		m[1,0] = 0;  m[1,1] = y;  m[1,2] = b;  m[1,3] = 0;
		m[2,0] = 0;  m[2,1] = 0;  m[2,2] = c;  m[2,3] = d;
		m[3,0] = 0;  m[3,1] = 0;  m[3,2] = e;  m[3,3] = 0;
		return m;
	}
	
	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (UseStereoShader) {
			RenderTexture.active = destination;
			GL.PushMatrix();
			GL.LoadOrtho();
			
			// Draw Left Eye
			StereoBarrelWarpMaterial.SetPass(0);
			drawQuad(0);
			
			// Draw Right Eye
			StereoBarrelWarpMaterial.SetPass(1);
			drawQuad(1);

			GL.PopMatrix();
		}
	}
	
	// Draw Render Textures Quads
	private void drawQuad(int passNumber)
	{
		// Handle Left Camera
		if (passNumber == 0) {
	   		GL.Begin (GL.QUADS);      
	      	GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex3( 0.0f, 0.0f, 0.1f );
	      	GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex3( 0.5f, 0.0f, 0.1f );
	      	GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex3( 0.5f, 1.0f, 0.1f );
	      	GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex3( 0.0f, 1.0f, 0.1f );
	   		GL.End();
		}
		// Handle Right Camera
		else if (passNumber == 1) {
	   		GL.Begin (GL.QUADS);      
	      	GL.TexCoord2( 0.0f, 0.0f ); GL.Vertex3( 0.5f, 0.0f, 0.1f );
	      	GL.TexCoord2( 1.0f, 0.0f ); GL.Vertex3( 1.0f, 0.0f, 0.1f );
	      	GL.TexCoord2( 1.0f, 1.0f ); GL.Vertex3( 1.0f, 1.0f, 0.1f );
	      	GL.TexCoord2( 0.0f, 1.0f ); GL.Vertex3( 0.5f, 1.0f, 0.1f );
	   		GL.End();
		}
	}
	#endregion
	
	#region Initialization and Setup functions
	private void initStereoCamera()
	{
		setupCameras();
		setupStereo();
	}
	
	private void setupCameras()
	{
		Transform lcam = transform.Find("leftCam");
		if (lcam != null) {
			_leftCamera = lcam.gameObject;
			_leftCamera.camera.CopyFrom (camera);
		} else {
			_leftCamera = new GameObject("leftCam", typeof(Camera));
			_leftCamera.AddComponent<GUILayer>();
			_leftCamera.camera.CopyFrom (camera);
			_leftCamera.transform.parent = transform;
		}
	
		Transform rcam = transform.Find("rightCam");
		if (rcam != null) {
			_rightCamera = rcam.gameObject;
			_rightCamera.camera.CopyFrom (camera);
		} else {
			_rightCamera = new GameObject("rightCam", typeof(Camera));
			_rightCamera.AddComponent<GUILayer>();
			_rightCamera.camera.CopyFrom (camera);
			_rightCamera.transform.parent = transform;
		}
		
		Transform gcam = transform.Find("guiCam");
		if (gcam != null) {
			GuiCamera = gcam.gameObject;
		} else {
			GuiCamera = new GameObject("guiCam", typeof(Camera));
			GuiCamera.AddComponent<GUILayer>();
			GuiCamera.camera.CopyFrom (camera);
			GuiCamera.transform.parent = transform;
		}
	
		GUILayer guiComponent = GetComponent<GUILayer>();
		guiComponent.enabled = false;
		
		// rendering order (back to front): centerCam/maskCam/leftCam1/rightCam1/leftCam2/rightCam2/ etc
		camera.depth = -2; 
		
		_leftCamera.camera.depth = camera.depth + (RenderOrderDepth*2) + 2;
		_rightCamera.camera.depth = camera.depth + ((RenderOrderDepth*2)+1) + 3;
		
		_leftCamera.camera.cullingMask = camera.cullingMask;
		_rightCamera.camera.cullingMask = camera.cullingMask;
			
		GuiCamera.camera.depth = _rightCamera.camera.depth+1;
		
		GuiCamera.camera.cullingMask = 1 << guiOnlyLayer;
		GuiCamera.camera.clearFlags = CameraClearFlags.Depth;
		
		#if !UNITY_EDITOR
		if (!UseStereoShader) {
			camera.enabled = false;
		}
		#endif
	}
	
	private void setupStereo()
	{
		if (UseStereoShader) {		
			if (!_leftCamRT) {
				_leftCamRT = new RenderTexture(Screen.width, Screen.height, 24);
			}
			if (!_rightCamRT) {
				_rightCamRT = new RenderTexture(Screen.width, Screen.height, 24);
			}
			
			StereoBarrelWarpMaterial.SetTexture("_LeftTex", _leftCamRT);
			StereoBarrelWarpMaterial.SetTexture("_RightTex", _rightCamRT);
		
			_leftCamera.camera.targetTexture = _leftCamRT;
			_rightCamera.camera.targetTexture = _rightCamRT;
		} else {
			_leftCamera.camera.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
			_rightCamera.camera.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);

			adjustAspect();
		}
	}
	
	private void adjustAspect()
	{
		camera.ResetAspect();
		camera.aspect *= ((_leftCamera.camera.rect.width * 2) / _leftCamera.camera.rect.height);
		_leftCamera.camera.aspect = camera.aspect;
		_rightCamera.camera.aspect = camera.aspect;
	}
	#endregion
	
	#region Helper Functions
	public static Rect Vector4toRect(Vector4 v)
	{
		return new Rect(v.x, v.y, v.z, v.w);
	}
	
	public static Vector4 RectToVector4(Rect r)
	{
		return new Vector4(r.x, r.y, r.width, r.height);
	}

	private void releaseRenderTextures() {
		_leftCamera.camera.targetTexture = null;
		_rightCamera.camera.targetTexture = null;
		if (_leftCamRT != null) {
			_leftCamRT.Release();
		}
		if (_rightCamRT != null) {
			_rightCamRT.Release();
		}
	}	
	#endregion
	
	#region Editor Functions
	void OnDrawGizmos()
	{
		Vector3 gizmoLeft = transform.position + transform.TransformDirection(-InteraxialDist/InteraxialDivisor, 0, 0);
		Vector3 gizmoRight = transform.position + transform.TransformDirection(InteraxialDist/InteraxialDivisor, 0, 0);
		Vector3 gizmoTarget = transform.position + transform.TransformDirection(Vector3.forward) * ZeroPrlxDist;
		
		Gizmos.color = new Color(1,1,1,1);
		
		Gizmos.DrawLine(gizmoLeft, gizmoTarget);
		Gizmos.DrawLine(gizmoRight, gizmoTarget);
		Gizmos.DrawLine(gizmoLeft, gizmoRight);
		
		Gizmos.DrawSphere(gizmoLeft, 0.02f);
		Gizmos.DrawSphere(gizmoRight, 0.02f);
		Gizmos.DrawSphere(gizmoTarget, 0.02f);
	}
	#endregion
}
