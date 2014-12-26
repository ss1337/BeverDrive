BeverDrive
==========

Media application for BMWs, replacing the cd-changer menu. There are few pictures of it in action here: http://imgur.com/a/itsYs

To try it out offline, use left/right arrows and space to simulate the right navigation knob

In order to use it you need:
* a BMW with Ibus (e38, e39, e46 for example) and navigation
* a car PC, preferrably with a car pc power supply
* a serial Ibus-interface, I use Reslers interface you can find here: http://www.reslers.de/IBUS/ 
* a VGA->composite adapter in order to connect the VGA output to the back camera input

Stuff you need to configure in Config.xml
* Serial port for the Ibus-interface
* Path to music library
* Path to video library
* Path to VLC libraries (libvlc.dll and libvlccore.dll)
* Whether you want bluetooth support enabled or not. Bluetooth requires the Broadcomm bluetooth stack

How to Use
* Remove the cd changer
* Connect the PC's power supply to 12V, ignition and GND
* Connect the audio output from the computer to the cd changer audio, fabricating a custom cable may be necessary
* Connect the Ibus interface to the car's Ibus wire, (it's a white one)
* Connect the VGA->composite adapter to the back camera input
* Set BeverDrive to autostart when windows start
* When BeverDrive is running and everything is connected, press Mode to change to cd changer mode, the display should switch to the computer display
* Press Select to enable the audio
* Navigate with the right knob

Note: bluetooth support is buggy, and it needs to have the device capable of auto-pairing to the PC