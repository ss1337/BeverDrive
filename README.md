BeverDrive
==========

Media application for BMWs, replacing the cd-changer menu. There are few pictures of it in action here: http://imgur.com/a/itsYs

To try it out offline, use left/right arrows and space to simulate the right navigation knob

In order to use it you need:
* a BMW with Ibus (e38, e39, e46 for example) and navigation
* a car PC
* a serial Ibus-interface, I use Reslers interface you can find here: http://www.reslers.de/IBUS/ 
* a VGA->composite adapter in order to connect the VGA output to the back camera input

Stuff you need to configure in Config.xml
* Serial port for the Ibus-interface
* Path to music library
* Path to video library
* Path to VLC libraries (libvlc.dll and libvlccore.dll)
* Whether you want bluetooth support enabled or not. Bluetooth requires the Broadcomm bluetooth stack

Note: bluetooth support is buggy, and it needs to have the device capable of auto-pairing to the PC