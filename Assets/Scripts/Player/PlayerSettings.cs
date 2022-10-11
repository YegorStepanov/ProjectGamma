using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player", fileName = "Player Settings")]
public sealed class PlayerSettings : ScriptableObject
{
    [Range(0.01f, 30f)]
    [SerializeField] private float _horizontalRotationSpeedRadians = 1f;

    [Range(0.01f, 30f)]
    [SerializeField] private float _verticalRotationSpeedRadians = 5f;

    [Min(0f)]
    [SerializeField] private float _minMoveDistance = 0.01f;

    [Header("Walking")]
    [Range(0.1f, 100f)]
    [SerializeField] private float _walkSpeed = 10f;

    [Header("Dashing")]
    [Min(0.1f)]
    [SerializeField] private float _dashDistance = 5f;
    [Range(0.1f, 100f)]
    [SerializeField] private float _dashMaxSpeed = 30f;
    [SerializeField] private AnimationCurve _dashSpeed;

    public float HorizontalRotationSpeedRadians => _horizontalRotationSpeedRadians;
    public float VerticalRotationSpeedRadians => _verticalRotationSpeedRadians;
    public float MinMoveDistance => _minMoveDistance;
    public float WalkSpeed => _walkSpeed;
    public float DashDistance => _dashDistance;
    public float DashMaxSpeed => _dashMaxSpeed;
    public AnimationCurve DashSpeed => _dashSpeed.NotNull();
}