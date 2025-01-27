using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Discovery;
using TMPro;
using UnityEngine.Events;

public class AutoStart : MonoBehaviour
{
    public NetworkDiscovery networkDiscovery;
    private readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();

    public TMP_Text text;
    private bool isConnected = false;

    public UnityEvent startedHost;
    public UnityEvent startedClient;
    

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Log("Running on Android");
            StartClient();
            startedClient.Invoke();
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            Log("Running on Windows");
            StartHost();
            startedHost.Invoke();
        }
        else
        {
            Log("Running on an unsupported platform");
        }
    }

    public void ServerFound(ServerResponse serverResponse)
    {
        discoveredServers.Add(serverResponse.serverId, serverResponse);
    }
    
    private void StartHost()
    {
        Log("Starting as host");
        discoveredServers.Clear();
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();
        Log("Server is now advertising on the network.");
    }

    private void StartClient()
    {
        Log("Starting as client");
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
        Log("Client started discovery. Searching for servers...");

        // Start a coroutine to check for discovered servers
        StartCoroutine(WaitForServers(5f)); // Wait for 5 seconds
    }

    private IEnumerator WaitForServers(float timeout)
    {
        float startTime = Time.time;
        while (Time.time - startTime < timeout)
        {
            if (discoveredServers.Count > 0)
            {
                Log($"Found {discoveredServers.Count} server(s). Attempting to connect...");
                ConnectToFirstDiscoveredServer();
                yield break;
            }
            yield return null; // Wait for the next frame
        }
        Log("No servers found after timeout. Is the host running?");
    }

    private void ConnectToFirstDiscoveredServer()
    {
        foreach (var server in discoveredServers.Values)
        {
            Log($"Attempting to connect to server: {server.EndPoint.Address}");
            NetworkManager.singleton.networkAddress = server.EndPoint.Address.ToString();
            NetworkManager.singleton.StartClient();
            return;
        }
    }

    private void Awake()
    {
        Log("Awake");


        // Server-side events
        NetworkServer.OnConnectedEvent += OnServerConnected;
        NetworkServer.OnDisconnectedEvent += OnServerDisconnected;
        NetworkServer.OnErrorEvent += OnServerError;

        // Client-side events
        NetworkClient.OnConnectedEvent += OnClientConnected;
        NetworkClient.OnDisconnectedEvent += OnClientDisconnected;
        NetworkClient.OnErrorEvent += OnClientError;
    }

    private void OnServerFound(ServerResponse info)
    {
        Log($"Discovered server at {info.EndPoint.Address}:{info.EndPoint.Port}");
        if (!discoveredServers.ContainsKey(info.serverId))
        {
            discoveredServers[info.serverId] = info;
        }
    }

    //
    // Server-side logging
    //
    private void OnServerConnected(NetworkConnectionToClient conn)
    {
        Log($"[Server] Client connected: {conn.address}");
    }

    private void OnServerDisconnected(NetworkConnectionToClient conn)
    {
        Log($"[Server] Client disconnected: {conn.address}");
    }

    private void OnServerError(NetworkConnectionToClient conn, TransportError error, string message)
    {
        LogError($"[Server] Error with client {conn.address}: {error} - {message}");
    }

    //
    // Client-side logging
    //
    private void OnClientConnected()
    {
        isConnected = true;
        Log("[Client] Successfully connected to the server.");
    }

    private void OnClientDisconnected()
    {
        isConnected = false;
        Log("[Client] Disconnected from the server.");
    }

    private void OnClientError(TransportError error, string message)
    {
        LogError($"[Client] Network error occurred: {error} - {message}");
    }

    //
    // Helper Methods
    //
    private void Log(string message)
    {
        Debug.Log(message);
        if (text != null)
        {
            text.text += "\n" + message;
        }
    }

    private void LogError(string message)
    {
        Debug.LogError(message);
        if (text != null)
        {
            text.text += "\n" + message;
        }
    }
}
