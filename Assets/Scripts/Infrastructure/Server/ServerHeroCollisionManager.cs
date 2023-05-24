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

        public void HandleColliderHit(Player player, Player collidedPlayer)
        {
            if (player.StateMachine.State != PlayerState.Dash)
                return;

            if (collidedPlayer.StateMachine.State != PlayerState.Dash || IsPlayerContributeMore(player, collidedPlayer))
            {
                HandlePlayersHit(player, collidedPlayer);
            }
        }

        private static bool IsPlayerContributeMore(Player player, Player collidedPlayer)
        {
            Vector3 collisionDirection = Vector3.Normalize(collidedPlayer.Position - player.Position);
            Vector3 playerPower = Vector3.Project(player.Forward, collisionDirection);
            Vector3 collidedPower = Vector3.Project(collidedPlayer.Forward, -collisionDirection);

            return playerPower.sqrMagnitude < collidedPower.sqrMagnitude;
        }

        private void HandlePlayersHit(Player winner, Player loser)
        {
            if (_serverBlockingManager.TryBlock(loser))
            {
                _serverScoreManager.IncreaseScore(winner);

                if (_serverScoreManager.IsPlayerWon(winner))
                {
                    _serverGameOverManager.EndGame(winner);
                }
            }
        }
    }
}