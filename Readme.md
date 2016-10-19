COMP30022 IT Project - Team Aqua
================================

Project
-------
Team Aqua TD is a cooperative multiplayer tower defense game for
Android. Enemies are spawned periodically and make their way to the
player's base in order to destroy it. In order to defend the base,
players control movable builders which can build towers to shoot at
the enemies.

Key Features
------------
- The ability to play with anyone around the world
- Chatting in-game with your teammate
- Watching a replay of your most recent match
- A record of your high-scores
- Innovative and unique gameplay and design

Team
----
- Lawrence Kuoch: Front-End and UNET Server Developer  
- Matthew Eldridge: Front-End Developer  
- Jason Traill: Full Stack Developer  
- Kaven Peng: Back-End Developer

Requirements
------------
- Unity 5.4 or greater
- Android phone (optional)
- Android Studio (optional, only if an emulator is to be used)
- Two devices, where a device is either a computer or a phone

Build Instructions
------------------
The game needs to be built twice, on two separate devices,
in order to play multiplayer.

1. Clone the repository or download as a zip file
2. Import the project in Unity
3. If no scene is open, open MainMenu.unity in the Scenes directory
   in Unity's Project view window
4. Go to File -> Build Settings...

* To run on an Android phone
    1. Select Android and press the Build button, save the apk file
    2. Connect your phone to the computer, copy the apk onto the device
    3. Locate the apk in your phone's file system, click on it and install
    4. Click open when the installation finishes

* To run on an Android emulator
    1. Select Android and press the Build button, save the apk file
    2. Open AVD Manager
    3. If no AVDs are available, press Create... and create an AVD
       with a 16:9 aspect ratio
    4. Select an AVD and press Start..., then Launch
    5. Wait for the AVD to turn on
    6. Copy the apk file that Unity built into the platform-tools
       directory in the Android sdk directory
    7. Open a terminal and navigate to the platform-tools directory
    8. Type into the terminal: adb install -r FileName.apk
    9. After the terminal indicates success, navigate to the app menu
       on the AVD and open the itproject app
    10. If in portrait mode, press left Ctrl + F12 for Windows,
        Fn + Ctrl + F12 for Mac, to switch to landscape

* To run in Unity
    1. Set the aspect ratio to 16:9 in the Game tab
    2. Press the play button

Running Tests
-------------
Tests are run in separate scenes. These are located in the Tests directory.
Run these scenes in the usual way. Errors indicate that not all tests were
successful.

Some tests are driven by the Unity Test Tools framework.
To run these tests, go to the Integration Tests tab, or click
Unity Test Tools -> Integration Test Runner to open the dockable window.
In this window press Run All. A tick indicates success, a cross indicates
failure.