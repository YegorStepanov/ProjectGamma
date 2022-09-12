using Mirror;
using UnityEngine;

public sealed class GameFactory : NetworkBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CameraController _playerCameraPrefab;
    [SerializeField] private HeroCollisionManager _heroCollisionManager;
    [SerializeField] private PlayerData _playerData;

    private FakeInputManager _fakeInputManager;

    public static GameFactory Instance;
    
    private void Awake()
    {
        _fakeInputManager = new FakeInputManager();
        Instance = this;
    }

    public Player CreateTestPlayer(Player playerPrefab, Vector3 position, Quaternion rotation)
    {
        Player player = Instantiate(playerPrefab, position, rotation);
        // player.Construct(_playerData, _fakeInputManager, CreateCamera, _heroCollisionManager.HandleColliderHit);
        return player;
    }

    public Player CreatePlayer(Player playerPrefab, Vector3 position, Quaternion rotation)
    {
        Player player = Instantiate(playerPrefab, position, rotation);
        // player.Construct(_playerData, _inputManager, CreateCamera, _heroCollisionManager.HandleColliderHit);
        return player;
    }

    public void Construct(Player player)
    {
        player.Construct(_playerData, _inputManager, CreateCamera, _heroCollisionManager.HandleColliderHit);
    }
    

    public CameraController CreateCamera(Transform focusOn)
    {
        CameraController controller = Instantiate(_playerCameraPrefab);
        controller.Construct(Camera.main, _inputManager);
        controller.FocusOn = focusOn;
        return controller;
    }
}
