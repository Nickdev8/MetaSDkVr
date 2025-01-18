using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public GameObject dummyPrefab; // Assign the dummy prefab in the Inspector

    public void OnPlayerJoined(PlayerNetwork player)
    {
        if (isServer)
        {
            SpawnDummyForPlayer(player);
        }
    }

    private void SpawnDummyForPlayer(PlayerNetwork player)
    {
        GameObject dummy = Instantiate(dummyPrefab);
        NetworkServer.Spawn(dummy);

        DummyUpdater updater = dummy.GetComponent<DummyUpdater>();
        updater.Initialize(player);
    }
}