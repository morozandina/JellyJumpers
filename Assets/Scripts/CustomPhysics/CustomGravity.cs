using UnityEngine;

namespace CustomPhysics
{
    [RequireComponent(typeof(Rigidbody))]
    public class CustomGravity : MonoBehaviour
    {
        public float gravityScale = 1.0f;
 
        private const float GlobalGravity = -9.81f;

        private Rigidbody _rb;

        private void OnEnable ()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
        }

        private void FixedUpdate ()
        {
            var gravity = GlobalGravity * gravityScale * Vector3.up;
            _rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }
}
