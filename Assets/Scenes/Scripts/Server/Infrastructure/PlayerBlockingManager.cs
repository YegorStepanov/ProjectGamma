﻿using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class PlayerBlockingManager : NetworkBehaviour
{
    [SerializeField] private RoomData _data;
    
    private HashSet<IPlayer> _blockedPlayers;

    private void Awake()
    {
        Debug.Log("awake");
        _blockedPlayers = new HashSet<IPlayer>();
    }

    public bool IsBlocked(IPlayer player)
    {
        return _blockedPlayers.Contains(player);
    }

    public void Block(IPlayer player)
    {
        _blockedPlayers.Add(player);
        StartCoroutine(SetColorRoutine(player));
    }

    private IEnumerator SetColorRoutine(IPlayer player)
    {
        Color oldColor = player.Color;

        player.Color = _data.BlockingColor;
        yield return new WaitForSeconds(_data.BlockingTime);
        player.Color = oldColor;

        _blockedPlayers.Remove(player);
    }
}
