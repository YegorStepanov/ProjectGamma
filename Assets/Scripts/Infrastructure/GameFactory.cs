using InputManagers;
using UnityEngine;

namespace Infrastructure
{
    public sealed class GameFactory : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraController _playerCameraPrefab;
        [SerializeField] private PlayerSettings _playerSettings;

        public void ConstructPlayer(GameObject playerGameObject)
        {
            Player player = playerGameObject.GetComponent<Player>().NotNull();

            CameraController cameraController = CreateCamera(player.CameraFocusPoint);
            var inputManager = new RelativeInputManagerDecorator(new DefaultInputManager(), cameraController.transform);
            player.Construct(_playerSettings, inputManager);
        }

        private CameraController CreateCamera(Transform focusOn)
        {
            CameraController controller = Instantiate(_playerCameraPrefab);
            var inputManager = new DefaultInputManager();
            controller.Construct(_camera, inputManager);
            controller.FocusOn = focusOn;
            return controller;
        }
    }
}