﻿using UnityEngine;

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
    [SerializeField] private float _jumpHeight = 1.2f;
    [SerializeField] private float _gravity = -15f;
    [Min(0.01f)]
    [SerializeField] private float _groundProbingDistance = 0.5f;

    [Header("Dashing")]
    [Min(0.1f)]
    [SerializeField] private float _dashDistance = 5f;
    [Range(0.1f, 100f)]
    [SerializeField] private float _dashMaxSpeed = 14f;
    [SerializeField] private AnimationCurve _dashSpeedPercentage;

    public float RotationSpeed => _rotationSpeed;
    public float MoveThreshold => _moveThreshold;
    public float WalkSpeed => _walkSpeed;
    public float DashDistance => _dashDistance;
    public float DashMaxSpeed => _dashMaxSpeed;
    public AnimationCurve DashSpeedPercentage => _dashSpeedPercentage;
    public float JumpHeight => _jumpHeight;
    public float Gravity => _gravity;
    public float GroundProbingDistance => _groundProbingDistance;
}