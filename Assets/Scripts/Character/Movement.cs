using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    [RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
    public class Movement : MonoBehaviour
    {
         private float _playerHeight = 2f;

         [SerializeField] private Transform orientation;
         [SerializeField] private Transform cam;

         [Header("Movement")]
         [SerializeField] private float moveSpeed = 6f;
         [SerializeField] private float airMultiplier = 0.4f;
         [SerializeField] private float movementMultiplier = 10f;
         [SerializeField] private float rotationSpeed;

         [Header("Sprinting")]
         [SerializeField] private float walkSpeed = 4f;
         [SerializeField] private float sprintSpeed = 6f;
         [SerializeField] private float acceleration = 10f;

         [Header("Jumping")]
         public float jumpForce = 5f;

         [Header("Keybindings")]
         [SerializeField] private KeyCode jumpKey = KeyCode.Space;
         [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

         [Header("Drag")]
         [SerializeField] private float groundDrag = 6f;
         [SerializeField] private float airDrag = 2f;

         private float _horizontalMovement;
         private float _verticalMovement;

         [Header("Ground Detection")]
         [SerializeField] private Transform groundCheck;
         [SerializeField] private LayerMask groundMask;
         [SerializeField] private float groundDistance = 0.2f;

         private bool _isGrounded = true;
         public bool IsGrounded
         {
             get => _isGrounded;
             private set
             {
                 if (_isGrounded == value) return;
                 _isGrounded = value;
                 animator.SetTrigger(_inAirId);
             }
         }

         [Header("Animation")]
         public Animator animator;
         private readonly int _speedId = Animator.StringToHash("Speed");
         private readonly int _sprintId = Animator.StringToHash("Sprint");
         private readonly int _jumpId = Animator.StringToHash("JumpTrigger");
         private readonly int _groundId = Animator.StringToHash("Ground");
         private readonly int _inAirId = Animator.StringToHash("InAir");

         private Vector3 _moveDirection;
         private Vector3 _slopeMoveDirection;
         
         [NonSerialized] private Rigidbody _rb;

         private RaycastHit _slopeHit;
         
         // Hot Fix animation

         private bool OnSlope()
         {
             if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight / 2 + 0.5f))
             {
                 return _slopeHit.normal != Vector3.up;
             }
             return false;
         }

         private void Start()
         {
             _rb = GetComponent<Rigidbody>();
             _rb.freezeRotation = true;

             Cursor.visible = false;
             Cursor.lockState = CursorLockMode.Locked;
         }

         private void Update()
         {
             IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

             MyInput();
             RotatePlayer();
             ControlDrag();
             ControlSpeed();

             if (Input.GetKeyDown(jumpKey) && IsGrounded)
             {
                 Jump();
             }

             _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal);
         }

         private void MyInput()
         {
             _horizontalMovement = Input.GetAxisRaw("Horizontal");
             _verticalMovement = Input.GetAxisRaw("Vertical");

             _moveDirection = orientation.forward * _verticalMovement + orientation.right * _horizontalMovement;
         }

         private void Jump()
         {
             if (!IsGrounded) return;
             animator.SetTrigger(_jumpId);
             _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
             _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
         }

         private void ControlSpeed()
         {
             if (Input.GetKey(sprintKey) && IsGrounded)
             {
                 animator.SetBool(_sprintId, true);
                 moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
             }
             else
             {
                 animator.SetBool(_sprintId, false);
                 moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
             }
         }

         private void ControlDrag()
         {
             _rb.drag = IsGrounded ? groundDrag : airDrag;
         }

         private void FixedUpdate()
         {
             MovePlayer();
             ControlJump();
         }

         private void MovePlayer()
         {
             switch (IsGrounded)
             {
                 case true when !OnSlope():
                     _rb.AddForce(_moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
                     break;
                 case true when OnSlope():
                     _rb.AddForce(_slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
                     break;
                 case false:
                     _rb.AddForce(_moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
                     break;
             }
             
             animator.SetFloat(_speedId, _rb.velocity.magnitude);
         }

         private void ControlJump()
         { 
             animator.SetBool(_groundId, IsGrounded);
         }

         private void RotatePlayer()
         {
             var viewDir = transform.position - new Vector3(cam.position.x, transform.position.y, cam.position.z);
             orientation.forward = viewDir.normalized;
             
             if (_moveDirection != Vector3.zero)
                 transform.forward = Vector3.Slerp(transform.forward, _moveDirection.normalized, Time.deltaTime * rotationSpeed);
         }
    }
}
