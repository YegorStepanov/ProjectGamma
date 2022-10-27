using System;
using UnityEngine;

public interface IPlayerData
{
    public event Action<IPlayer> Changed;

    public string Tag { get; }
    public int Layer { get; }
    public PlayerScoreData ScoreData { get; set; }
    public Color32 Color { get; set; }
}