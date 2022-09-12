using UnityEngine;

public sealed class ScoreManager : MonoBehaviour
{
    public bool IsPlayerWon(Player player) =>
        player.Score > 3;

    public void IncreaseScore(Player player) =>
        player.Score++;
}
