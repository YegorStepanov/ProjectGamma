using UnityEngine;

public sealed class LookAtMainCamera : Mirror.NetworkBehaviour
{
    private Camera _camera;

    private void LateUpdate()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            return;
        }

        transform.forward = _camera.transform.forward;
    }
}
