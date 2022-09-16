using Mirror;
using UnityEngine;

public sealed class GameFactory : NetworkBehaviour
{
    [SerializeField] private CameraController _playerCameraPrefab;
    [SerializeField] private HeroCollisionManager _heroCollisionManager;
    [SerializeField] private PlayerSettings _playerSettings;
    
    [Server]
    public Player CreatePlayer(Player playerPrefab)
    {
        Player player = Instantiate(playerPrefab);
        player.Construct(_playerSettings, EmptyInputManager.Instance);

        player.Hit += OnPlayerOnHit;
        player.LocalPlayerStarted += RpcOnLocalPlayerStarted;
        return player;
    }

    [Server]
    private void OnPlayerOnHit(Player player, ControllerColliderHit hit)
    {
        _heroCollisionManager.HandleColliderHit(player, hit);
    }

    private void RpcOnLocalPlayerStarted(Player player)
    {
        CameraController cam = CreateCamera(player.CameraFocusPoint);

        var inputManager = new PointRelativeInputManager(cam.transform);
        player.Construct(player.Settings, inputManager);
        player.StateMachine.SetState(PlayerState.Walk);
    }

    [Client]
    private CameraController CreateCamera(Transform focusOn)
    {
        CameraController controller = Instantiate(_playerCameraPrefab);
        controller.Construct(Camera.main, new InputManager());
        controller.FocusOn = focusOn;
        return controller;
    }
}
