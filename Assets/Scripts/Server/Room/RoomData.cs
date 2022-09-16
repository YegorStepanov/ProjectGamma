using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Room", fileName = "RoomData")]
public sealed class RoomData : ScriptableObject
{
    [SerializeField] private int _pointsToWin = 3;
    [SerializeField] private float _blockingTimeSeconds = 3f;
    [SerializeField] private Color32 _blockingColor = Color.red;
    [SerializeField] private float _restartTimeSeconds = 5f;
    [SerializeField] private Color32[] _playerColors;

    public int PointsToWin => _pointsToWin;
    public float BlockingTime => _blockingTimeSeconds;
    public Color32 BlockingColor => _blockingColor;
    public float RestartTimeSeconds => _restartTimeSeconds;
    public IReadOnlyList<Color32> PlayerColors => _playerColors;

}
