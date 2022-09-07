using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour
{
    [SerializeField] private InputManager _inputManager;

    [SerializeField] private Transform _target;
    [SerializeField] private float _maxDistance = 5;
    [SerializeField] private float _rotationSpeed = 90f;
    [Range(-89.9f, 89.9f)]
    [SerializeField] private float _minVerticalAngle = -45f;
    [Range(-89.9f, 89.9f)]
    [SerializeField] private float _maxVerticalAngle = 45f;

    private Camera _camera;
    private Vector2 _rotation = new Vector2(45f, 0f);

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        transform.localRotation = Quaternion.Euler(_rotation);
    }

    private void LateUpdate()
    {
        Vector2 rotation = _rotation;

        rotation += GetInputRotation();
        rotation = ConstrainRotation(rotation);
        LookToTarget(rotation);

        _rotation = rotation;
    }

    private Vector2 GetInputRotation()
    {
        Vector2 rotation = _inputManager.ReadRotateAction();
        return rotation * _rotationSpeed * Time.unscaledDeltaTime;
    }

    private Vector2 ConstrainRotation(Vector2 rotation)
    {
        rotation.x = Mathf.Clamp(rotation.x, _minVerticalAngle, _maxVerticalAngle);

        if (rotation.y < 0f)
            rotation.y += 360f;
        else if (rotation.y >= 360f)
            rotation.y -= 360f;

        return rotation;
    }

    private void LookToTarget(Vector2 rotation)
    {
        Quaternion lookRotation = Quaternion.Euler(rotation);
        Vector3 lookDirection = lookRotation * Vector3.forward;

        Vector3 point = _target.position;
        float distance;// = GetCollisionDistance(point, lookDirection, lookRotation);

        if (Physics.Raycast(point, -lookDirection, out RaycastHit hit, _maxDistance))
        {
            // lookPosition = point - lookDirection * hit.distance;
            distance = hit.distance;
        }
        else
        {
            distance = _maxDistance;
        }

        // //workaround for a bug: on dashing, the camera ... 
        // if (collisionDistance < _distance)
        //     collisionDistance = GetCollisionDistance(point - lookDirection, lookDirection, lookRotation);

        Vector3 lookPosition = point - lookDirection * distance;

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private float GetCollisionDistance(Vector3 point, Vector3 direction, Quaternion rotation)
    {
        Vector3 nearPlaneHalfSize = GetNearClipPlaneHalfSize();

        float castDistance = _maxDistance - _camera.nearClipPlane;
        if (Physics.BoxCast(point, nearPlaneHalfSize, -direction, out RaycastHit hit, rotation, castDistance))
        {
            return hit.distance + _camera.nearClipPlane;
        }

        return _maxDistance;
    }

    private Vector3 GetNearClipPlaneHalfSize()
    {
        float angle = _camera.fieldOfView * 0.5f;
        angle *= Mathf.Deg2Rad;

        float height = Mathf.Tan(angle);
        float width = height * _camera.aspect;

        var size = new Vector3(width, height);
        return size;
    }
}
