using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Room
{
    // Exclude the points after creation so that another player cannot be spawned on the same point
    public sealed class FreeStartPositions
    {
        private readonly List<Transform> _allPoints;
        private readonly List<Transform> _availablePoints;
        private readonly PlayerSpawnMethod _playerSpawnMethod;

        public int Available => _availablePoints.Count;

        public FreeStartPositions(List<Transform> points, PlayerSpawnMethod playerSpawnMethod)
        {
            _allPoints = points;
            _availablePoints = new List<Transform>(points);
            _playerSpawnMethod = playerSpawnMethod;
        }

        public void Reset()
        {
            _availablePoints.Clear();
            _availablePoints.AddRange(_allPoints);
        }

        public Vector3 Pop()
        {
            int index = GetIndex();
            Transform point = _availablePoints[index];
            _availablePoints.RemoveAt(index);
            return point.position;
        }

        private int GetIndex()
        {
            switch (_playerSpawnMethod)
            {
                case PlayerSpawnMethod.Random:
                    return Random.Range(0, _availablePoints.Count);
                case PlayerSpawnMethod.RoundRobin:
                    return _availablePoints.Count - 1;
                default:
                    Debug.LogError($"Incorrect {nameof(_playerSpawnMethod)} value");
                    break;
            }

            return 0;
        }
    }
}