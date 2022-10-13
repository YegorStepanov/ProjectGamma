using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public sealed class ClientRoomManager : NetworkBehaviour
{
    [SerializeField] public GameFactory _gameFactory;
    [SerializeField] private HeroCollisionManager _heroCollisionManager;

    [TargetRpc]
    // [Client] ???
    public void TargetConstructPlayer([UsedImplicitly] NetworkConnection conn, GameObject gamePlayer, PlayerSettings settings)
    {
        IPlayer player = gamePlayer.GetComponent<IPlayer>().NotNull();

        CameraController cameraController = _gameFactory.CreateCamera(player.CameraFocusPoint);
        var inputManager = new TransformRelativeInputManager(cameraController.transform);
        player.Construct(settings, inputManager);

        player.Hit += _heroCollisionManager.HandleColliderHit;
        //todo: the state should be synced

        if (isLocalPlayer)
            player.StateMachine.SetState(PlayerState.Walk);
    }
}