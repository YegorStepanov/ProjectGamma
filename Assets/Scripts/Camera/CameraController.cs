using InputManagers;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public sealed class CameraController : MonoBehaviour, ICameraController
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxDistance = 5;
    [SerializeField] private float _rotationSpeed = 90f;
    [Range(-89.9f, 89.9f)]
    [SerializeField] private float _minVerticalAngle = -45f;
    [Range(-89.9f, 89.9f)]
    [SerializeField] private float _maxVerticalAngle = 45f;

    private IInputManager _inputManager;
    private Vector2 _rotation = new Vector2(45f, 0f);

    public Transform FocusOn { get; set; }

    public void Construct(IInputManager inputManager)
    {
        _inputManager = inputManager;
    }

    private void Awake()
    {
        transform.localRotation = Quaternion.Euler(_rotation);
    }

    private void LateUpdate()
    {
        if (FocusOn == null) return;

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

        Vector3 point = FocusOn.position - lookDirection;
        float distance = GetCollisionDistance(point - lookDirection, lookDirection, lookRotation);

        Vector3 lookPosition = point - lookDirection * distance;

        transform.SetPositionAndRotation(lookPosition, lookRotation);
    }

    private float GetCollisionDistance(Vector3 point, Vector3 direction, Quaternion rotation)
    {
        Vector3 nearPlaneHalfSize = GetNearClipPlaneHalfSize();

        float distance = _maxDistance - _camera.nearClipPlane;
        if (Physics.BoxCast(point, nearPlaneHalfSize, -direction, out RaycastHit hit, rotation, distance))
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