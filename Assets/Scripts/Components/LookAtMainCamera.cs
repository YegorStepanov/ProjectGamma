using UnityEngine;

public sealed class LookAtMainCamera : MonoBehaviour
{
    private CameraController _cameraController;

    private void Update()
    {
        if (_cameraController == null)
        {
            var cam = Camera.main;
            if (cam != null && cam.TryGetComponent(out CameraController cameraController))
            {
                _cameraController = cameraController;
                _cameraController.AfterCameraLateUpdate += OnAfterCameraLateUpdate;
            }
        }
    }

    private void OnDestroy()
    {
        // _cameraController can be destroyed first (it will remove all its subscribers by itself)
        if(_cameraController != null)
            _cameraController.AfterCameraLateUpdate -= OnAfterCameraLateUpdate;
    }

    private void OnAfterCameraLateUpdate()
    {
        transform.forward = _cameraController.transform.forward;
    }
}