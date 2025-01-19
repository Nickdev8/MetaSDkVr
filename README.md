# VR Multiplayer Project

This project is a prototype for a **local co-op VR multiplayer experience** for the Oculus Quest 2. It enables multiple users to interact in the same virtual space with synchronized positions and movements. The prototype includes a debugging tool to visualize Guardian boundaries and calculate shared origins for alignment between players.

---

## Features

1. **Local Multiplayer with Shared Space**
   - Multiple players can join the same virtual room and see each other's positions.
   - Player positions are synchronized relative to a shared origin point.

2. **Oculus Guardian Boundary Visualization**
   - Retrieves and displays the Guardian boundary points.
   - Calculates the center of the play area for alignment.

3. **Real-Time Position Synchronization**
   - Synchronizes head and hand positions using a networking framework (e.g., Mirror Networking).
   - Automatic alignment ensures seamless setup.

4. **Debugging Tools**
   - Visualize boundary points and center in the Unity Scene View.
   - Draws boundary lines and marks the center for debugging purposes.

---

## Requirements

- **Hardware**: Oculus Quest 2
- **Software**: Unity 2022.3 or newer
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

### 3. Configure VR Settings
- Go to `Project Settings > XR Plug-in Management`.
- Enable **Oculus** under Android build settings.
- Set **Minimum API Level** to 23.

### 4. Set Up Multiplayer Scene
- Add the `NetworkManager` prefab from Mirror Networking.
- Assign the player prefab to the `NetworkManager` component.
- Ensure the `PlayerAlignment` script is attached to the player prefab.

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

### 1. **PlayerAlignment.cs**
- Synchronizes player positions with the shared world space.
- Calculates offsets to align local spaces with the host’s world space.
- Sends and receives positional data using Mirror Networking.

### 2. **OculusBoundaryDebugger.cs**
- Retrieves Guardian boundary points and calculates the center of the boundary.
- Visualizes the boundary and center point in the Unity Scene View for debugging.

---

## Debugging Tools

- **Boundary Visualization**:
  - View the Guardian boundary and center in the Unity Scene View.
  - Use this for aligning players in co-located multiplayer environments.

- **Logs**:
  - Debug logs output player connections, offsets, and boundary data.

---

## Roadmap

1. Add basic interaction (e.g., object manipulation and shared actions).
2. Implement voice chat or text communication between players.
3. Optimize for performance on Oculus Quest 2.

---

## Contributing
- Fork the repository.
- Create a feature branch (`git checkout -b feature-name`).
- Commit changes and open a pull request.

---

## License
This project is licensed under the MIT License. See `LICENSE` for details.
