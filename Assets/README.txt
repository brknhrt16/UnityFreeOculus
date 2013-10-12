---Enabling the Oculus Rift---
Copy everything in Assets > WebPlayerTemplates > BuildDependencies into your build directory (with your exe).
Add an Assets > UnityFreeOculus > Prefabs > OculusManager to your scene.
Add a Assets > UnityFreeOculus > Prefabs > UfoCamera or UfoPlayer to track head motion.
If you are using your own camera:
	Add an Assets > UnityFreeOculus > Scripts > AddPreRenderEvent to any cameras that will track the Oculus.
	Add a Assets > UnityFreeOculus > Scripts > TrackOclulus to the object containing those cameras which should rotate.

-When developing run Build > OculusSupport > StartOculus.bat
-When running build run Build > Warp.bat

---Setting up Gui3D---
Add MenuOpen and MenuSelect buttons to your Input Manager.
Add a Gui3D prefab:
	It is recommended that you start with a menu incorporating the UfoCamera:
		Assets > Gui3D > Prefabs > Samples > Gui3DExample
		Assets > Gui3D > Prefabs > Samples > Gui3DUfoCamera
	Otherwise add a Assets > Gui3D > Prefabs > Gui3D
		Add cameras to the GuiCameras object.
			Add a Assets > Gui3D > Scripts > Gui3DCamera script to any Camera components.
			Ensure that the render order (depth) is greater than any cameras outside the Gui3D.
		Add a Assets > Gui3D > Prefabs > Gui3DMenu to GuiObjects.
			Add any interactable objects to the menu.