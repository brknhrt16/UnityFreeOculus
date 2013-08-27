############################################################
#                                                          #
#      VRDK - The Virtual-Reality Development Kit          #
#                                                          #
#      Package: Do-it-yourself Rift Kit Camera 1.0         #
#                                                          #
############################################################

DESCRIPTION:
This Unity package contains all of the required scripts
and prefabs to start using your DIY Rift kit inside Unity. 
All materials in this package are offered free and open source
so that you may make any changes or improvements required to
get your own DIY Rift up and working inside Unity.

UNITY FREE:
For Unity Free users, you may use the DIYRiftCamera to 
generate the appropriate Right and Left cameras, along with
the GUI Camera to pick up objects you want on your GUI layers.
If you are using Unity Free, I highly recommend getting 
Vireio Perception (a 3D Driver for the Rift) here: 
http://www.vireio.com/


UNITY PRO:
For Unity Pro users, you may enable the UseStereoShader to
create the correct image effect. The Barreling and Warping
effects may be adjusted by also using the WarpFactorAdjuster
script in conjunction to the DIYRiftCamera script. This will
allow you to wear your DIY Rift and adjust the settings until
you get them JUST right for your kit. A good test to make sure
you have it finely tuned is to tip your head side-to-side and
see if there is skewing or sheering of the image in each eye.

SHADER SETTINGS:
There are two main shader settings you may want to adjust
depending on your setup. For lack of better terms, they have
been named barrel factor and warp factor. The barrel factor
is defaulted to (-81.0/10), but may be tweaked to fit your kit.
This value will adjust how much lens distortion compensation the
shader will handle. The warp factor is defaulted to (2.0). This
value will basically adjust the intensity of the barrel factor.
Once dialed in, you may want to "copy and paste" the shader into
a new shader and hard-code the values for optimal performance.

RELEASE NOTES:
This package does NOT contain the scripts and interfaces
required to use head-tracking IMUs. This may come in a later
release with some standardized IMU scripts, but for now, this
just handles outputting the correctly warped image. There is
external support for Hillcret Labs tracking IMUs in the 
Vireio Perception driver: http://www.vireio.com/

