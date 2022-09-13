using Mirror;
using UnityEngine;

public sealed class GameFactory : NetworkBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CameraController _playerCameraPrefab;
    [SerializeField] private HeroCollisionManager _heroCollisionManager;
    [SerializeField] private PlayerData _playerData;

    public Player CreatePlayer(Player playerPrefab, Vector3 position)
    {
        Vector3 lookDirection = LookToSceneCenter(position);
        return Instantiate(playerPrefab, position, Quaternion.LookRotation(lookDirection));
    }

    public void ConstructPlayer(Player player, string playerName)
    {
        player.Construct(_playerData, () => _inputManager, CreateCamera, _heroCollisionManager.HandleColliderHit);

        player.gameObject.name = playerName;
        player.Name = playerName;
    }

    public static Vector3 LookToSceneCenter(Vector3 position)
    {
        Vector3 lookDirection = -position;
        lookDirection.y = 0;
        return lookDirection;
    }

    private CameraController CreateCamera(Transform focusOn)
    {
        CameraController controller = Instantiate(_playerCameraPrefab);
        controller.Construct(Camera.main, _inputManager);
        controller.FocusOn = focusOn;
        return controller;
    }
}
