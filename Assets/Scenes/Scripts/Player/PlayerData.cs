using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player", fileName = "PlayerData")]
public sealed class PlayerData : ScriptableObject
{
    [Min(0.1f)]
    [SerializeField] private float _rotationSpeed = 30f;

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

    public float RotationSpeed => _rotationSpeed;
    public float MinMoveDistance => _minMoveDistance;
    public float WalkSpeed => _walkSpeed;
    public float DashDistance => _dashDistance;
    public float DashMaxSpeed => _dashMaxSpeed;
    public AnimationCurve DashSpeed => _dashSpeed;
}
