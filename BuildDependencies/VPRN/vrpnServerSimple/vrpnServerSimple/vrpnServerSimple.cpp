// VRPN Server tutorial
// by Sebastien Kuntz, for the VR Geeks (http://www.vrgeeks.org)
// August 2011

#include <stdio.h>
#include <tchar.h>

#include <math.h>

#include "vrpn_Text.h"
#include "vrpn_Tracker.h"
#include "vrpn_Analog.h"
#include "vrpn_Button.h"
#include "vrpn_Connection.h"
#include "OculusManager.h"

#include <iostream>
using namespace std;

/////////////////////// TRACKER /////////////////////////////

// your tracker class must inherit from the vrpn_Tracker class
class OculusRiftTracker : public vrpn_Tracker
{
public:
	OculusRiftTracker( vrpn_Connection *c = 0 );
	virtual ~OculusRiftTracker() {cout << "Deconstructing RiftTracker" << endl;};

	virtual void mainloop();

protected:
	OculusManager * _oculusManager;
	struct timeval _timestamp;
};

OculusRiftTracker::OculusRiftTracker( vrpn_Connection *c /*= 0 */ ) :
	vrpn_Tracker( "Tracker0", c )
{
	cout << "Constructing RiftTracker" << endl;
	_oculusManager = new OculusManager();
}

void
OculusRiftTracker::mainloop()
{
	vrpn_gettimeofday(&_timestamp, NULL);

	vrpn_Tracker::timestamp = _timestamp;

	// We will just put a fake data in the position of our tracker
	static float angle = 0; angle += 0.001f;

	// the pos array contains the position value of the tracker
	// XXX Set your values here
	pos[0] = sinf( angle ); 
	pos[1] = 0.0f;
	pos[2] = 0.0f;

	// the d_quat array contains the orientation value of the tracker, stored as a quaternion
	// XXX Set your values here
	OculusQuaternion ocuQuat = _oculusManager->GetOrientation();

	d_quat[0] = ocuQuat.x;
	d_quat[1] = ocuQuat.y;
	d_quat[2] = ocuQuat.z;
	d_quat[3] = ocuQuat.w;

	//cout << d_quat[0] << d_quat[1] << d_quat[2] << d_quat[3] << endl;

	char msgbuf[1000];

	d_sensor = 0;

	int  len = vrpn_Tracker::encode_to(msgbuf);

	if (d_connection->pack_message(len, _timestamp, position_m_id, d_sender_id, msgbuf,
		vrpn_CONNECTION_LOW_LATENCY))
	{
		fprintf(stderr,"can't write message: tossing\n");
	}

	server_mainloop();
}

/////////////////////// ANALOG /////////////////////////////

// your analog class must inherin from the vrpn_Analog class
class myAnalog : public vrpn_Analog
{
public:
	myAnalog( vrpn_Connection *c = 0 );
	virtual ~myAnalog() {};

	virtual void mainloop();

protected:
	struct timeval _timestamp;
};


myAnalog::myAnalog( vrpn_Connection *c /*= 0 */ ) :
	vrpn_Analog( "Analog0", c )
{
	vrpn_Analog::num_channel = 10;

	vrpn_uint32	i;

	for (i = 0; i < (vrpn_uint32)vrpn_Analog::num_channel; i++) {
		vrpn_Analog::channel[i] = vrpn_Analog::last[i] = 0;
	}
}

void
myAnalog::mainloop()
{
	vrpn_gettimeofday(&_timestamp, NULL);
	vrpn_Analog::timestamp = _timestamp;

	// forcing values to change otherwise vrpn doesn't report the changes
	static float f = 0; f+=0.001;

	for( unsigned int i=0; i<vrpn_Analog::num_channel;i++)
	{
		// XXX Set your values here !
		channel[i] = i / 10.f + f;
	}

	// Send any changes out over the connection.
	vrpn_Analog::report_changes();

	server_mainloop();
}

/////////////////////// BUTTON /////////////////////////////

// your button class must inherit from the vrpn_Button class
class myButton : public vrpn_Button
{
public:
	myButton( vrpn_Connection *c = 0 );
	virtual ~myButton() {};

	virtual void mainloop();

protected:
	struct timeval _timestamp;
};


myButton::myButton( vrpn_Connection *c /*= 0 */ ) :
	vrpn_Button( "Button0", c )
{
	// Setting the number of buttons to 10
	vrpn_Button::num_buttons = 10;

	vrpn_uint32 i;

	// initializing all buttons to false
	for (i = 0; i < (vrpn_uint32)vrpn_Button::num_buttons; i++) {
		vrpn_Button::buttons[i] = vrpn_Button::lastbuttons[i] = 0;
	}
}

void
myButton::mainloop()
{
	vrpn_gettimeofday(&_timestamp, NULL);
	vrpn_Button::timestamp = _timestamp;

	// forcing values to change otherwise vrpn doesn't report the changes
	static int b=0; b++;

	for( unsigned int  i=0; i<vrpn_Button::num_buttons;i++)
	{
		// XXX Set your values here !
		buttons[i] = (i+b)%2;
	}

	// Send any changes out over the connection.
	vrpn_Button::report_changes();

	server_mainloop();
}


int _tmain(int argc, _TCHAR* argv[])
{
	// Creating the network server
	vrpn_Connection_IP* m_Connection = new vrpn_Connection_IP();

	// Creating the tracker
	OculusRiftTracker* serverTracker = new OculusRiftTracker(m_Connection );
	myAnalog*  serverAnalog  = new myAnalog(m_Connection );
	myButton*  serverButton  = new myButton(m_Connection );

	cout << "Created VRPN server." << endl;

	while(true)
	{
		serverTracker->mainloop();
		//serverAnalog->mainloop();
		//serverButton->mainloop();

		m_Connection->mainloop();

		// Calling Sleep to let the CPU breathe.
		SleepEx(1,FALSE);
	}
}

