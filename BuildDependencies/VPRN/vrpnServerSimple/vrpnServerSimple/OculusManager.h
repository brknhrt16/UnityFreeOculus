#ifndef OCULUS_MANAGER_H
#define OCULUS_MANAGER_H
#include <Windows.h>
#include "OVR.h"

struct OculusQuaternion
{
	double x, y, z, w;
};

class OculusManager
{
	public:
	OculusManager();
	~OculusManager();
	OculusQuaternion GetOrientation();
};
#endif