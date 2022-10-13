using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player", fileName = "Player Settings")]
public sealed class PlayerSettings : ScriptableObject
{
    [Range(0.01f, 30f)]
    public float HorizontalRotationSpeedRadians = 1f;
    [Range(0.01f, 30f)]
    public float VerticalRotationSpeedRadians = 5f;
    [Min(0f)]
    public float MinMoveDistance = 0.01f;

    [Header("Walking")]
    [Range(0.1f, 100f)]
    public float WalkSpeed = 10f;

    [Header("Dashing")]
    [Min(0.1f)]
    public float DashDistance = 5f;
    [Range(0.1f, 100f)]
    public float DashMaxSpeed = 30f;
    public AnimationCurve DashSpeed;
}