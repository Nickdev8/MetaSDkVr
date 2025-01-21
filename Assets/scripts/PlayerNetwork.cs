using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    public GameObject hostCamera;
    
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartClient();
        }
        else
        {
            StartHost();
        }
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        hostCamera.SetActive(true);
    }

    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        hostCamera.SetActive(false);
    }

}
