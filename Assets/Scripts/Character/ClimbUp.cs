using System;
using UnityEngine;

namespace Character
{
    public class ClimbUp : MonoBehaviour
    {
        private Movement _movement;
        private readonly int _climbId = Animator.StringToHash("Climb");

        private void Awake()
        {
            _movement = transform.parent.GetComponent<Movement>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground") && !_movement.IsGrounded)
            {
                _movement.animator.SetTrigger(_climbId);
                Debug.Log("YEP");
            }
        }
    }
}
