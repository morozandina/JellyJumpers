using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class ClimbUp : MonoBehaviour
    {
        private readonly int _climbId = Animator.StringToHash("Climb");
        
        [Header("Climb settings")]
        [SerializeField] private LayerMask hitMask;
        [SerializeField] public CapsuleCollider capsule;

        [Header("Offsets")]
        [SerializeField] private float upDistance;

        [Header("Gizmos")]
        [SerializeField] private Vector3 rayCastOffset = Vector3.one;

        private Vector3[] _wayPoints = new Vector3[]{};
        private Movement _movement;
        private bool _isClimbing = false;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
        }

        private void FixedUpdate()
        {
            if (_movement.IsGrounded)
                return;
            
            if (Physics.Raycast(capsule.bounds.center + (transform.forward / 1.5f) + rayCastOffset, -transform.up, out var hit, upDistance, hitMask))
            {
                var position = transform.position;
                _wayPoints = new []
                {
                    position,
                    new(position.x, hit.point.y, position.z),
                    hit.point
                };
                
                Climb();
            }
        }

        private void Climb()
        {
            _isClimbing = true;
            _movement.animator.SetTrigger(_climbId);
            transform.DOPath(_wayPoints, .8f, PathType.Linear, PathMode.Ignore).OnComplete(() =>
            {
                _isClimbing = false;
                _movement.animator.ResetTrigger(_climbId);
            });
        }
    }
}
