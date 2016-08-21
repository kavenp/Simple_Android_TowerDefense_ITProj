COMP30022 IT Project - Team Aqua
================================

Requirements
------------
- Unity 5.4 or greater
- Android Studio (if an emulator is to be used)


Build Instructions
------------------
1. Clone the repository or download as a zip file
2. Open the project in Unity
3. If no scene is open, open Game.unity in the Assets directory
   in the Project view
4. Go to File -> Build Settings...

Of the following three, only two need to be performed.  
These are ordered from most to least time consuming to setup.

* To run on an Android emulator, select Android and press the Build button
    1. Open AVD Manager
    2. If no AVDs are available, press Create... and create an AVD
    3. Select an AVD and press Start..., then Launch
    4. Wait for the AVD to turn on
    5. Copy the apk file that Unity built into the platform-tools
       directory in the Android sdk directory
    6. Open a terminal and navigate to the platform-tools directory
    7. Type into the terminal: adb install -r FileName.apk
    8. After the terminal indicates success, navigate to the app menu
       on the AVD and open the itproject app

* To run on a desktop, select Standalone, choose the relevant
    Target Platform for your computer
    1. Press Build And Run, save the file
    2. Set Graphics as desired, tick the Windowed option and press Play!

* In Unity, press the play button
