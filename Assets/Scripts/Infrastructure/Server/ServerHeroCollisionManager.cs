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
            // var moveVector = player.Velocity; // hitPlayer.GetComponent<CharacterController>().velocity;
            // Debug.Log($"moveVector = {moveVector}");
            // Hit?.Invoke(this, hitPlayer, hit.moveDirection * hit.moveLength);

            // Vector3 moveVector;

            // Debug.Log($"start HandlePlayersHit {player.Data.Name} {collidedPlayer.Data.Name} {isServer} {isClient}");

            Debug.Log($"before state={player.StateMachine.State} {player.Data.Name}");

            if (player.StateMachine.State != PlayerState.Dash)
                return;

            Debug.Log($"State {player.StateMachine.State} {player.name}");

            //remove another check temporary
            if (collidedPlayer.StateMachine.State != PlayerState.Dash || IsPlayerContributeMore(player, collidedPlayer))
            {
                Debug.Log($"if passed {player.name}");
                HandlePlayersHit(player, collidedPlayer);
            }
        }

        private static bool IsPlayerContributeMore(Player player, Player collidedPlayer)
        {
            Vector3 collisionDirection = Vector3.Normalize(collidedPlayer.Position - player.Position);
            Vector3 playerPower = Vector3.Project(player.Forward, collisionDirection);
            //we should invert direction
            Vector3 collidedPower = Vector3.Project(collidedPlayer.Forward, -collisionDirection);

            // bool beyondThePlane = Vector3.Dot(collisionDirection, collidedPower) < 0;
            //if (beyondThePlane)
            //{
            //    Debug.Log("Beyound the plane");
            //    return true;
            //}
            //else
            if (playerPower.sqrMagnitude < collidedPower.sqrMagnitude)
            {
                Debug.Log($"{player.name} magnitude is less");
                return true;
            }

            return false;
            // Debug.Log($"HandlePlayersHit {isServer} {isClient}");
        }

        // [Command]
        //[Server]
        private void HandlePlayersHit(Player winner, Player loser)
        {
            if (_serverBlockingManager.TryBlock(loser))
            {
                Debug.Log($"tryblock {winner.name}");
                _serverScoreManager.IncreaseScore(winner);

                if (_serverScoreManager.IsPlayerWon(winner))
                {
                    Debug.Log($"playerwon {winner.name}");
                    _serverGameOverManager.EndGame(winner);
                }            }
        }
    }
}