# EMGVRMINIGAMES

-------------------

## Requirements :

- A PC
- 2 EMG sensors (with the [OpenBCI app](https://openbci.com/downloads)) (Valentin add dongle... what is needed tysm)
- 2 VR headsets with the game APK installed (Oculus Quest) or through the EXE with VR linking.
- Python (ver>=3.8)
- Unity 6 Preview (for now, might not be needed in the future)

## Features :

- Multiplayer minigames (Dino Run, Pong, etc.) that will allow you to compete and experience a WarioWare-like game in VR.
- EMG sensors to control the player input (this is done directly by the server)

## Installation :

1. Clone the repository

```bash
git clone https://github.com/idesemi-weekly/EMGVRMINIGAMES.git
```

2. Open the OpenBCI application and connect the EMG sensors and the dongle, attach the sensors to the arms of each players (depending on the channels need Valentin to fill this part, TBD). 

3. Run emg_input.py (in EMG/emg_input.py) to get the EMG data from the sensors.

4. Open the Unity project and run the game (as a server).

5. Run the game on the VR headsets (as clients, either through the APKs or the EXEs).

6. Enjoy !

## Tips :

- You only need controllers to navigate the menu and the main lobby, the rest of the game is controlled by the EMG sensors, so remove them as you wish.

- The game is still in development, so some features may not work as intended, we also plan to add more and more microgames to it.

## Authors :

- [Dylan THOMAS](https://github.com/Dylouwu) : VR code implementation and microgame developer

- [Valentin CAZIN](https://github.com/ItsMyRainbow) : EMG implementation and microgame developer

## License :

This project is licensed under the MIT License - see the LICENSE.md file for details.

## Estimated pre-release date :

#### - August 2024 (v0.1.0)
