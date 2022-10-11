using UnityEngine;

public interface ICameraController
{
    public Transform FocusOn { get; set; }
    Vector3 TransformDirection(Vector3 direction);
}