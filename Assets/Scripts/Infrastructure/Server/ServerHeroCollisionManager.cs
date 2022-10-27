using System;
using Mirror;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerHeroCollisionManager : NetworkBehaviour
    {
        [SerializeField] private ServerGameOverManager _serverGameOverManager;
        [SerializeField] private ServerScoreManager _serverScoreManager;
        [SerializeField] private ServerBlockingManager _serverBlockingManager;

        public void HandleColliderHit(IPlayer player, ControllerColliderHit hit)
        {
            GameObject go = hit.gameObject;
            if (!go.CompareTag(player.Data.Tag))
                return;

            Debug.Log($"start HandlePlayersHit {isServer} {isClient}");

            if (go.TryGetComponent(out IPlayer anotherPlayer))
            {
                if (player.StateMachine.State == PlayerState.Dash)
                {
                    HandlePlayersHit(player, anotherPlayer);
                }
            }
        }

        // [Command]
        //[Server]
        private void HandlePlayersHit(IPlayer winner, IPlayer loser)
        {
            Debug.Log($"HandlePlayersHit {isServer} {isClient}");
            if (_serverBlockingManager.TryBlock(loser))
            {
                _serverScoreManager.IncreaseScore(winner);

                if (_serverScoreManager.IsPlayerWon(winner))
                    _serverGameOverManager.EndGame(winner);
            }
        }
    }
}