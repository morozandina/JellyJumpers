using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class ClimbUp : MonoBehaviour
    {
        [Header("Climb settings")]
        [SerializeField] private LayerMask hitMask;

        [Header("Offsets")]
        [SerializeField] private Vector3 spineOffset;
        
        [Header("Gizmos")]
        [SerializeField] private float gizmosSphereRadius = 0.1f;

        private Vector3 _wallHitPoint, _wallNormal;
        private float _topEdge;

        private void Update()
        {
            if (Physics.Raycast(transform.position + spineOffset, transform.forward, out var hit, 1, hitMask))
            {     
                _wallHitPoint = hit.point;
                _wallNormal = hit.normal;
                
                var col = hit.collider;
                var bounds = col.bounds;
                _topEdge = bounds.center.y + bounds.extents.y;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_wallHitPoint, gizmosSphereRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere( new Vector3(_wallHitPoint.x, _topEdge, _wallHitPoint.z), gizmosSphereRadius);
        }
    }
}
