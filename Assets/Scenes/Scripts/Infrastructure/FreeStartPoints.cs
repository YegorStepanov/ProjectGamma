using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class FreeStartPoints
{
    public static FreeStartPoints Create(IEnumerable<Transform> points) =>
        new FreeStartPoints(points);

    private readonly List<Transform> _freePoints;

    private FreeStartPoints(IEnumerable<Transform> points)
    {
        _freePoints = new List<Transform>(points);
    }

    public Transform Pop(PlayerSpawnMethod playerSpawnMethod)
    {
        int index = GetIndex(playerSpawnMethod);
        Transform point = _freePoints[index];
        _freePoints.RemoveAt(index);
        return point;
    }

    private int GetIndex(PlayerSpawnMethod playerSpawnMethod)
    {
        switch (playerSpawnMethod)
        {
            case PlayerSpawnMethod.Random:
                return Random.Range(0, _freePoints.Count);
            case PlayerSpawnMethod.RoundRobin:
                return 0;
            default:
                Debug.LogError($"Incorrect {nameof(playerSpawnMethod)} value");
                break;
        }

        return 0;
    }
}
