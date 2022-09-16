using System;
using UnityEngine;

public interface IPlayerData
{
    public event Action<IPlayer> Changed;

    public string Tag { get; }
    public int Layer { get; }
    public string Name { get; set; }
    public uint Score { get; set; }
    public Color32 Color { get; set; }
}
