# VRxEMG

**This game is a multiplayer WarioWare-like that uses VR for the display and EMG for the input ! Here's a little GIF presentation of the game :**  


![ezgif-1-3734a70fa3](https://github.com/user-attachments/assets/8d47364c-e549-408a-89e6-3b2de164ba5f)  


Only Local Multiplayer is available and we have no plan to make an online release, too complex to handle with EMGs and we will need to have intrusive permissions to make this work, which goes agaisnt our principles. 


## Requirements :

- A PC
- 2 VR headsets with the game APK installed (Oculus Quest) or through PCVR with the clients running on different computers
- 5 EMG sensors (with the [OpenBCI app](https://openbci.com/downloads))
- 1 Cyton Board
- 1 OpenBCI Bluetooth Dongle 


## Features :

- Multiplayer minigames (Dino Run, Pong, etc.) that will allow you to compete and experience a WarioWare-like game in VR.
  
- EMG sensors to control the player input (the server receives all the input directly, not through the clients, they are only there as screens for the players.)


## Installation :

1. Download the [OpenBCI application](https://openbci.com/downloads) on your computer.

2. Plug the OpenBCI Bluetooth Dongle in, turn on the Cyton board and attach the sensors to the arms of each players by trying to fix it on the forearm muscles.  
*Channel 0 is for the right arm of P1, Channel 1 is for the right arm of P2, Channel 2 is for the left arm of P1, and Channel 3 is for the left arm of P2.*

3. Run the OpenBCI application and use the networking window to setup the UDP server and start the data streams. 

4. Run the server executable on your computer, **MAKE SURE THE GAME IS FOCUSED ON OR THE INPUT WON'T WORK**.

5. Run the game on the VR headsets through the APKs that were sideloaded on the headsets. (EXE builds will be available for v1.0, but are **NOT RECOMMENDED**).

6. Enjoy !


## Tips :

- You only need controllers to navigate the menu and the main lobby, the rest of the game is controlled by the EMG sensors, so remove them as you wish.

- Make sure that all devices are connected to internet.

- The game is still in development, so some features may not work as intended, we also plan to add more and more microgames to it.

- Feel free to visit the [wiki](https://github.com/idesemi-weekly/EMGVRMINIGAMES/wiki) if you have any questions or if you just want to know more about the game !

- Contact : pur1rin on Discord


## Authors :

- [Dylouwu](https://github.com/Dylouwu) : VR code implementation and microgame developer

- [Valentin](https://github.com/ItsMyRainbow) : EMG implementation and concept


## License :


This project is licensed under the MIT License - see the LICENSE.md file for details.
