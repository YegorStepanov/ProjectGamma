using UnityEngine;

public sealed class MockCameraController : ICameraController
{
    private readonly Transform _t;
    public Transform FocusOn { get; set; }

    public MockCameraController(Transform t)
    {
        _t = t;
        FocusOn = t;
    }

    public Vector3 TransformDirection(Vector3 direction) =>
        _t.TransformDirection(direction);
}
