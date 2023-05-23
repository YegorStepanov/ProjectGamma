using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player", fileName = "Player Settings")]
public sealed class PlayerSettings : ScriptableObject
{
    [Range(1f, 360 * 5f)]
    [SerializeField] private float _rotationSpeed = 1080f;
    [Min(0f)]
    [SerializeField] private float _moveThreshold = 0.01f;

    [Header("Walking")]
    [Range(0.1f, 100f)]
    [SerializeField] private float _walkSpeed = 4f;
    [Min(0.1f)]
    [SerializeField] private float _jumpHeight = 1.2f;
    [SerializeField] private float _gravity = -15f;
    [Min(0.1f)]
    [SerializeField] private float _terminalGravitySpeed = 50f;
    [Min(0f)]
    [SerializeField] private float _jumpTimeout = 0.5f;

    [Header("Dashing")]
    [Min(0.1f)]
    [SerializeField] private float _dashDistance = 5f;
    [Range(0.1f, 100f)]
    [SerializeField] private float _dashMaxSpeed = 14f;
    [SerializeField] private AnimationCurve _dashSpeedPercentage;

    [Header("Ground Probing")]
    [SerializeField] private float _groundProbingOffset = -0.14f;
    [SerializeField] private float _groundProbingRadius = 0.28f;
    [SerializeField] private LayerMask _groundProbingLayers;

    public float RotationSpeed => _rotationSpeed;
    public float MoveThreshold => _moveThreshold;

    public float WalkSpeed => _walkSpeed;
    public float JumpHeight => _jumpHeight;
    public float Gravity => _gravity;
    public float TerminalGravitySpeed => _terminalGravitySpeed;
    public float JumpTimeout => _jumpTimeout;

    public float DashDistance => _dashDistance;
    public float DashMaxSpeed => _dashMaxSpeed;
    public AnimationCurve DashSpeedPercentage => _dashSpeedPercentage;

    public float GroundProbingOffset => _groundProbingOffset;
    public float GroundProbingRadius => _groundProbingRadius;
    public LayerMask GroundProbingLayers => _groundProbingLayers;
}