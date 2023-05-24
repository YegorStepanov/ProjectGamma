using Mirror;
using UnityEngine;

internal class Spawner
{
    private static void InitialSpawn()
    {
        if (!NetworkServer.active) return;

        for (int i = 0; i < 10; i++)
            SpawnReward();
    }

    private static void SpawnReward()
    {
        if (!NetworkServer.active) return;

        Vector3 spawnPosition = new Vector3(Random.Range(-19, 20), 1, Random.Range(-19, 20));
        // NetworkServer.Spawn(Object.Instantiate(((RoomManager)NetworkManager.singleton).rewardPrefab, spawnPosition, Quaternion.identity));
    }
}