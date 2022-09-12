﻿using Mirror;
using UnityEngine;

public sealed class HeroCollisionManager : NetworkBehaviour
{
    [SerializeField] private GameOverManager _gameOverManager;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private PlayerBlockingManager _playerBlockingManager;

    public void HandleColliderHit(Player player, ControllerColliderHit hit)
    {
        GameObject go = hit.gameObject;
        if (!go.CompareTag(Player.Tag))
            return;

        if (go.TryGetComponent(out Player anotherPlayer))
        {
            if (player.State == PlayerState.Dash)
            {
                HandlePlayersHit(player, anotherPlayer);
            }
        }
    }

    private void HandlePlayersHit(Player winner, Player loser)
    {
        if (_playerBlockingManager.IsBlocked(loser))
            return;

        _playerBlockingManager.Block(loser);
        _scoreManager.IncreaseScore(winner);

        if (_scoreManager.IsPlayerWon(winner))
            _gameOverManager.EndGame(winner);
    }
}
