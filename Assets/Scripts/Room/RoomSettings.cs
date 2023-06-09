﻿using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    [CreateAssetMenu(menuName = "Settings/Room", fileName = "Room Settings")]
    public sealed class RoomSettings : ScriptableObject
    {
        [SerializeField] private int _pointsToWin = 3;
        [SerializeField] private float _blockingTimeSeconds = 3f;
        [SerializeField] private float _restartTimeSeconds = 5f;
        [SerializeField] private Color32[] _playerColors;

        public int PointsToWin => _pointsToWin;
        public float BlockingTime => _blockingTimeSeconds;
        public float RestartTimeSeconds => _restartTimeSeconds;
        public IReadOnlyList<Color32> PlayerColors => _playerColors;
    }
}