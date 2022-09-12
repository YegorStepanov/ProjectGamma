using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerBlockingManager : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private Color color = Color.red;

    private HashSet<Player> _blockedPlayers;

    private void Awake()
    {
        _blockedPlayers = new HashSet<Player>();
    }

    public bool IsBlocked(Player player) =>
        _blockedPlayers.Contains(player);

    public void Block(Player player)
    {
        _blockedPlayers.Add(player);
        StartCoroutine(BlockRoutine(player, duration, color));
    }

    private IEnumerator BlockRoutine(Player player, float duration, Color color)
    {
        yield return StartCoroutine(SetColorRoutine(player, duration, color));

        _blockedPlayers.Remove(player);
    }

    private static IEnumerator SetColorRoutine(Player player, float duration, Color color)
    {
        var playerColor = player.GetComponent<RandomColor>();

        Color32 oldColor = playerColor.color;

        playerColor.color = color;
        yield return new WaitForSeconds(duration);
        playerColor.color = oldColor;
    }
}
