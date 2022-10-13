using Mirror;
using UnityEngine;

public sealed class GameFactory : NetworkBehaviour
{
    [SerializeField] private CameraController _playerCameraPrefab;
    [SerializeField] private HeroCollisionManager _heroCollisionManager;
    [SerializeField] private PlayerSettings _playerSettings;

    public PlayerSettings Settings
    {
        get => _playerSettings;
        set => _playerSettings = value;
    }

    public Player CreatePlayer(Player playerPrefab) //?
    {
        Player player = Instantiate(playerPrefab);
        //it doesn't required?
        // player.SetSettings(_playerSettings);

        // player.Construct(_playerSettings, EmptyInputManager.Instance);

        //todo
        //player.Hit += OnHit;
        //player.LocalPlayerStarted += OnLocalPlayerStarted;

        return player;
    }

    public void OnHit(Player player, ControllerColliderHit hit)
    {
        _heroCollisionManager.HandleColliderHit(player, hit);
    }

    public void OnLocalPlayerStarted(Player player)
    {
        // Debug.Log($"+OnLocalPlayerStarted {isLocalPlayer} {isClient} {isServer} Settings={player.Settings != null}");
        // CameraController cam = CreateCamera(player.CameraFocusPoint);

        // var inputManager = new PointRelativeInputManager(cam.transform);
        // player.Construct(inputManager);
        player.StateMachine.SetState(PlayerState.Walk);
    }

    public CameraController CreateCamera(Transform focusOn)
    {
        CameraController controller = Instantiate(_playerCameraPrefab);
        controller.Construct(Camera.main, new InputManager());
        controller.FocusOn = focusOn;
        return controller;
    }
}