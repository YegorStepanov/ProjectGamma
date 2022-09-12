using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player", fileName = "PlayerData")]
public sealed class PlayerData : ScriptableObject
{
    [SerializeField, Min(0.1f)]
    private float _rotationSpeed = 30f;

    [Header("Walking")]
    [SerializeField, Range(0.1f, 100f)]
    private float _walkSpeed = 10f;

    [Header("Dashing")]
    [SerializeField, Min(0.1f)]
    private float _dashDistance = 5f;

    [SerializeField, Range(0.1f, 100f)]
    private float _dashMaxSpeed = 30f;

    [SerializeField]
    private AnimationCurve _dashSpeed;

    public float RotationSpeed => _rotationSpeed;

    public float WalkSpeed => _walkSpeed;

    public float DashDistance => _dashDistance;
    public float DashMaxSpeed => _dashMaxSpeed;
    public AnimationCurve DashSpeed => _dashSpeed;
}
