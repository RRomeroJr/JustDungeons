﻿using System.Collections.Generic;
using UnityEngine;

public sealed class AbilityDeliveryTransformationController
{
    private float _remainingRotationTime;
    private int _rotationIndex;
    public Vector2 _skillShotVector;
    private readonly bool _isRotating;
    private readonly List<RotationElement> _rotationSequence;
    private readonly AbilityDelivery _abilityDelivery;

    private AbilityType Type => _abilityDelivery.type;
    private Transform Transform => _abilityDelivery.transform;
    private Transform Target => _abilityDelivery.Target;
    private Actor Caster => _abilityDelivery.Caster;
    private Vector3 WorldPointTarget => _abilityDelivery.worldPointTarget;
    private float RotationsPerSecond => _rotationSequence[_rotationIndex].RotationsPerSecond;
    private bool IsTracking => _abilityDelivery.trackTarget;
    private bool IsAimingAtCollider => _abilityDelivery.aimAtCollider && Target != null;
    private float Speed => _abilityDelivery.speed;

    public AbilityDeliveryTransformationController(AbilityDelivery abilityDelivery, List<RotationElement> rotationSequence)
    {
        _abilityDelivery = abilityDelivery;
        _rotationSequence = rotationSequence;

        if (rotationSequence != null && rotationSequence.Count > 0)
        {
            _isRotating = true;
            _remainingRotationTime = rotationSequence[0].Duration;
            _rotationIndex = 0;
        }
    }

    public void InitialSpawn()
    {
        // Set the initial spawn of Aoe type to collider center
        if (Type is AbilityType.Aoe && IsAimingAtCollider)
        {
            if (Target.TryGetComponent(out Collider2D targetCollider))
            {
                Transform.position = targetCollider.bounds.center;
            }
        }
        // Set the initial direction of LineAoe type to target
        else if (Type is AbilityType.LineAoe)
        {
            AimAtTarget();
        }
        else if (Type is AbilityType.Skillshot)
        {
            _skillShotVector = WorldPointTarget - Transform.position;
            _skillShotVector.Normalize();
            _skillShotVector = Speed * _skillShotVector;
        }
    }

    public void Move()
    {
        if (Type == AbilityType.Normal)
        {
            Transform.position = Vector2.MoveTowards(Transform.position, Target.position, _abilityDelivery.speed);
        }
        else if (Type == AbilityType.Skillshot)
        {
            Transform.position = (Vector2)Transform.position + _skillShotVector;
        }
        else if (_abilityDelivery.followCaster && Caster != null)
        {
            Transform.position = Caster.transform.position;
        }
        else if (_abilityDelivery.followTarget && Target != null)
        {
            Transform.position = Target.position;
        }
    }

    public void TrackTarget()
    {
        if (!IsTracking)
        {
            return;
        }

        if (Type is AbilityType.LineAoe)
        {
            AimAtTarget();
        }
    }

    public void Rotate()
    {
        if (!_isRotating)
        {
            return;
        }

        UpdateRotationTimeAndIndex();

        if (!Mathf.Approximately(RotationsPerSecond, 0))
        {
            Vector3 rotation = new Vector3(0, 0, RotationsPerSecond * 360) * Time.fixedDeltaTime;
            Transform.Rotate(rotation, Space.World);
        }
    }

    private void UpdateRotationTimeAndIndex()
    {
        // No need to update rotation time or index if there is only one element
        if (_rotationSequence.Count == 1)
        {
            return;
        }

        _remainingRotationTime -= Time.fixedDeltaTime;

        if (_remainingRotationTime <= 0)
        {
            _rotationIndex++;
            if (_rotationIndex >= _rotationSequence.Count)
            {
                _rotationIndex = 0;
            }
            _remainingRotationTime += _rotationSequence[_rotationIndex].Duration;
        }
    }

    private void AimAtTarget()
    {
        Vector3 targetLocation;
        if (Target != null)
        {
            if (IsAimingAtCollider && Target.TryGetComponent(out Collider2D targetCollider))
            {
                targetLocation = targetCollider.bounds.center;
            }
            else
            {
                targetLocation = Target.position;
            }
        }
        else
        {
            targetLocation = WorldPointTarget;
        }

        Transform.right = Vector3.Normalize(targetLocation - Transform.position);
    }
}
