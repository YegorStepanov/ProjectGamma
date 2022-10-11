using System;
using Mirror;
using UnityEngine;

public sealed class GameFactory : NetworkBehaviour
{
    [SerializeField] private CameraController _playerCameraPrefab;
    [SerializeField] private HeroCollisionManager _heroCollisionManager;
    [SerializeField] private PlayerSettings _playerSettings;

    public Player CreatePlayer(Player playerPrefab) //?
    {
        Player player = Instantiate(playerPrefab);
        player.Construct(_playerSettings, EmptyInputManager.Instance);

        player.Hit += OnPlayerOnHit;
        player.LocalPlayerStarted += OnLocalPlayerStarted;

        return player;
    }

    [ClientRpc]
    public void RpcInitializePlayer(Player player)
    {
        Debug.Log("INIT RPC PLAYER");
        InitializePlayer(player);
    }

    public void InitializePlayer(Player player)
    {
        Debug.Log("INIT PLAYER");
        PlayerSettings playerSettings = player.Settings;
        player.Construct(playerSettings, EmptyInputManager.Instance);

        player.Hit += OnPlayerOnHit;
        player.LocalPlayerStarted += OnLocalPlayerStarted;
    }

    public void OnPlayerOnHit(Player player, ControllerColliderHit hit)
    {
        _heroCollisionManager.HandleColliderHit(player, hit);
    }

    public void OnLocalPlayerStarted(Player player)
    {
        Console.WriteLine("+OnLocalPlayerStarted");

        CameraController cam = CreateCamera(player.CameraFocusPoint);

        var inputManager = new PointRelativeInputManager(cam.transform);
        player.Construct(player.Settings, inputManager);
        player.StateMachine.SetState(PlayerState.Walk);
    }

    private CameraController CreateCamera(Transform focusOn)
    {
        CameraController controller = Instantiate(_playerCameraPrefab);
        controller.Construct(Camera.main, new InputManager());
        controller.FocusOn = focusOn;
        return controller;
    }
}