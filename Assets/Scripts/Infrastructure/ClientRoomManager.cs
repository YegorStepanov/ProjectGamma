using Infrastructure.Server;
using InputManagers;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Infrastructure
{
    public sealed class ClientRoomManager : NetworkBehaviour
    {
        [SerializeField] public GameFactory _gameFactory;
        [SerializeField] private ServerHeroCollisionManager _serverHeroCollisionManager;

        [TargetRpc]
        public void TargetConstructPlayer([UsedImplicitly] NetworkConnection conn, GameObject gamePlayer, PlayerSettings settings)
        {
            IPlayer player = gamePlayer.GetComponent<IPlayer>().NotNull();

            CameraController cameraController = _gameFactory.CreateCamera(player.CameraFocusPoint);
            var inputManager = new TransformRelativeInputManager(cameraController.transform);
            player.Construct(settings, inputManager);

            if (isServer) //todo:
                player.Hit += _serverHeroCollisionManager.HandleColliderHit;

            //todo: the state should be synced
            if (isLocalPlayer)
                player.StateMachine.SetState(PlayerState.Walk);
        }
    }
}