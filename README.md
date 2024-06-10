# EMGVRMINIGAMES
Run on Unity 6 Preview only !

Using EMG and VR, we created a multiplayer game composed of microgames.

## Requirements :

- A computer
- 2 EMG sensors (OpenBCI)
- 2 VR headsets with the game APK installed (Oculus Quest) or through the EXE with linking.
- Python (ver>=3.8)
- Unity 6 Preview

## Features :

- Multiplayer minigames (Dino Run, Pong, etc.) that will allow you to compete and experience a WarioWare-like game in VR.
- EMG sensors to control the player input (this is done directly by the server)

## Installation :

1. Clone the repository

```bash
git clone https://github.com/idesemi-weekly/EMGVRMINIGAMES.git
```

2. Open the OpenBCI application and connect the EMG sensors and the dongle, attach the sensors to the arms of each players (depending on the channels, TBD). 

3. Run emg_input.py (in EMG/emg_input.py) to get the EMG data from the sensors.

4. Open the Unity project and run the game (as a server).

5. Run the game on the VR headsets (as clients).

6. Enjoy !

## Tips :

- You only need controllers to navigate the menu and the main lobby, the rest of the game is controlled by the EMG sensors, so remove them as you wish.

- The game is still in development, so some features may not work as intended, we plan to add more and more minigames to it.

## Authors :

- [Dylan THOMAS]
- [Valentin CAZIN]

## License :

This project is licensed under the MIT License - see the LICENSE.md file for details.

## Estimated pre-release date :

- August 2024 (v0.1.0)