using System;
using System.Collections;
using System.Collections.Generic;
using kcp2k;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Mirror.Discovery;
using TMPro;
using UnityEngine.SceneManagement;

public class autostart : MonoBehaviour
{
    // public VRNetworkDiscovery networkDiscovery;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    
    public TMP_Text text;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            text.text = text.text + "@\n" + "Running on Android";
            StartClient();
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            text.text = text.text + "@\n" + "Running on Windows";
            StartHost();
        }
        else
        {
            Debug.Log("Running on an unsupported platform");
        }
    }


    private void StartHost()
    {
        Debug.Log("Starting as host");
        text.text = text.text + "@\n" + "Starting as host";
        discoveredServers.Clear();
        //NetworkManager.singleton.onlineScene = SceneManager.GetActiveScene().name;
        NetworkManager.singleton.StartHost();
        //NetworkManager.AdvertiseServer();

    }
    
    private void StartClient()
    {
        Debug.Log("Starting as client.");
        text.text = text.text + "@\n" + "Starting as client";
        discoveredServers.Clear();
        //networkDiscovery.StartDiscovery();
    }
    
    
    //
    // logging
    //
    private void Awake()
    {
        // Register event handlers for server-side events
        NetworkServer.OnConnectedEvent += OnServerConnected;
        NetworkServer.OnDisconnectedEvent += OnServerDisconnected;
        NetworkServer.OnErrorEvent += OnServerError;

        // Register event handlers for client-side events
        NetworkClient.OnConnectedEvent += OnClientConnected;
        NetworkClient.OnDisconnectedEvent += OnClientDisconnected;
        NetworkClient.OnErrorEvent += OnClientError;
    }
    
    private void OnServerConnected(NetworkConnectionToClient conn)
    {
        Debug.Log($"[Server] Client connected: {conn.address}");
    }

    private void OnServerDisconnected(NetworkConnectionToClient conn)
    {
        Debug.Log($"[Server] Client disconnected: {conn.address}");
    }

    private void OnServerError(NetworkConnectionToClient conn, TransportError error, string message)
    {
        Debug.LogError($"[Server] Error occurred with client {conn.address}: {error} - {message}");
    }
    
    //
    // Client-side logging
    //
    private void OnClientConnected()
    {
        Debug.Log("[Client] Successfully connected to the server.");
    }

    private void OnClientDisconnected()
    {
        Debug.Log("[Client] Disconnected from the server.");
    }

    private void OnClientError(TransportError error, string message)
    {
        Debug.LogError($"[Client] Network error occurred: {error} - {message}");
    }
}

