using UnityEngine;

public sealed class GameFactory : MonoBehaviour
{
    [SerializeField] private CameraController _playerCameraPrefab;

    public CameraController CreateCamera(Transform focusOn)
    {
        CameraController controller = Instantiate(_playerCameraPrefab);
        controller.Construct(Camera.main, new InputManager());
        controller.FocusOn = focusOn;
        return controller;
    }
}