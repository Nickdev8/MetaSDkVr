using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager
{
    public GameObject gameManagerPrefab; // Assign the GameManager prefab in the Inspector

    private GameManager gameManager;

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Spawn the GameManager on the server
        if (gameManagerPrefab != null)
        {
            GameObject gm = Instantiate(gameManagerPrefab);
            NetworkServer.Spawn(gm);
            gameManager = gm.GetComponent<GameManager>();
        }
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);

        // Notify the GameManager about the new player
        if (gameManager != null)
        {
            var player = conn.identity.GetComponent<PlayerNetwork>();
            gameManager.OnPlayerJoined(player);
        }
    }
}