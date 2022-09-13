using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerBlockingManager : Mirror.NetworkBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private Color color = Color.red;

    private HashSet<IPlayer> _blockedPlayers;

    private void Awake()
    {
        _blockedPlayers = new HashSet<IPlayer>();
    }

    public bool IsBlocked(IPlayer player) =>
        _blockedPlayers.Contains(player);

    public void Block(IPlayer player)
    {
        _blockedPlayers.Add(player);
        StartCoroutine(BlockRoutine(player, duration, color));
    }

    private IEnumerator BlockRoutine(IPlayer player, float duration, Color color)
    {
        yield return StartCoroutine(SetColorRoutine(player, duration, color));

        _blockedPlayers.Remove(player);
    }

    private static IEnumerator SetColorRoutine(IPlayer player, float duration, Color color)
    {
        Color oldColor = player.Color;
        player.Color = color;
        yield return new WaitForSeconds(duration);
        player.Color = oldColor;
    }
}
