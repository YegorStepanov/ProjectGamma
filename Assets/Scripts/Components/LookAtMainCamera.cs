using UnityEngine;

public sealed class LookAtMainCamera : MonoBehaviour
{
    private Camera _camera;

    private void LateUpdate()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            return;
        }

        if (_camera != null)
            transform.forward = _camera.transform.forward;
    }
}