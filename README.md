# VR Multiplayer Project

This project is a prototype for a **local co-op VR multiplayer experience** for the Oculus Quest 2. It enables multiple users to interact in the same virtual space with synchronized positions and movements. The prototype includes a networking and the Meta XR Sdk.

---

## Features


2. **XR Sdk and Instactions**
   - phisicla buttons to interact with
   - Able to reposision the player to center the scene to the border later

3. **Real-Time Mirror Networking**
   - Host for hosting it all
   - clients to run on the quests to play


## ToDo

1. **Local Multiplayer with Shared Space**
   - Multiple players can join the same virtual room and see each other's positions.
   - Player positions are synchronized relative to a shared origin point.
   - 
4. **Debugging Tools**
   - Visualize boundary points and center in the Unity Scene View.
   - Draws boundary lines and marks the center for debugging purposes.

---

## Requirements

- **Hardware**: Oculus Quest 2 and Windows pc
- **Software**: Unity 6.000.34f1 or newer
- **Networking Framework**: Mirror Networking
- **SDK**: Meta XR SDK (for Oculus Quest 2 integration)

---

## Setup Instructions

### 1. Clone the Repository
```bash
$ git clone https://github.com/your-username/vr-multiplayer-prototype.git
$ cd vr-multiplayer-prototype
```

### 2. Install Unity Dependencies
- Open the project in Unity.
- Ensure the following packages are installed via the Unity Package Manager:
  - **Mirror Networking**
  - **Meta XR SDK**


### 4. Make a andriod build for the quest
- upload the build to the quest
- and run the unity editor.

### 5. Make a andriod build for the quest
-- connections should be outomatic

---

## How to Test Multiplayer

### 1. Local Testing (Editor and Build)
- Run the Unity Editor as the **host**.
- Build a standalone executable (or APK for Quest 2) for clients.
- Connect the clients to the host using the IP address of the host machine.

### 2. Local Testing with ParrelSync
- Install **ParrelSync** to run multiple instances of the Unity Editor for rapid multiplayer testing.
- Open one instance as the host and the other as a client.

### 3. Cross-Device Testing
- Build and deploy the project to Oculus Quest 2 devices.
- Ensure all devices are on the same network.
- Host the game on a PC or one of the Quest devices.

---

## Key Scripts

### 1. **Autostart.cs**pace.
### 2. **VRNetworkPlayerScript.cs**
### 3. **GameManager.cs**

---

## License
This project is licensed under the MIT License. See `LICENSE` for details.
