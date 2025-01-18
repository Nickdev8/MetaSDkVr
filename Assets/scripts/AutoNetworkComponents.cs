using UnityEngine;
using UnityEngine.Events;
using Mirror;

public class AutoNetworkComponents : MonoBehaviour
{
    [Header("Debug")][SerializeField] private bool UseClintAndHost = false;
    
    [Header("Events")] 
    public UnityEvent onHostStart;
    public UnityEvent onClientStart;

    void Start()
    {
        if (UseClintAndHost)
        {
            Debug.Log("Starting as Both Client (Android) and Host (PC)");
            NetworkManager.singleton.StartHost();
            NetworkManager.singleton.StartClient();
            Debug.Log("OnClientStart");
            onClientStart.Invoke();
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // Set as Host for PC
            Debug.Log("Starting as Host (PC)");
            NetworkManager.singleton.StartHost();
            onHostStart.Invoke();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            // Set as Client for Android
            Debug.Log("Starting as Client (Android)");
            NetworkManager.singleton.StartClient();
            onClientStart.Invoke();
        }
    }
}