using UnityEngine;

public sealed class GameFactory : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private CameraController _cameraController;

    private FakeInputManager _fakeInputManager;

    private void Awake()
    {
        _fakeInputManager = new FakeInputManager();
    }

    public GameObject CreateEnemyPlayer(GameObject playerPrefab, Vector3 position) => 
        CreatePlayer(playerPrefab, position, _inputManager, null);

    public GameObject CreateControlledPlayer(GameObject playerPrefab, Vector3 position) => 
        CreatePlayer(playerPrefab, position, _fakeInputManager, _cameraController.transform);

    private GameObject CreatePlayer(GameObject playerPrefab, Vector3 position, IInputManager inputManager, Transform relativeTo)
    {
        GameObject player = Instantiate(playerPrefab, position, default);

        var dashState = player.GetComponent<DashState>();
        var walkState = player.GetComponent<WalkState>();
        var heroMovement = player.GetComponent<HeroMovement>();

        if (relativeTo == null)
            relativeTo = heroMovement.transform;
        
        dashState.Construct(relativeTo, heroMovement);
        walkState.Construct(inputManager, relativeTo);

        heroMovement.Construct(new IState[] { dashState, walkState });
        return player;
    }
}
