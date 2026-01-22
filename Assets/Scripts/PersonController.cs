using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PersonController: MonoBehaviour
{
  private InputSystem_Actions _inputSystem;
  private Rigidbody _rb;
  private Vector2 _moveInputValue;
  private bool _isGrounded;

  [Header("Movement Settings")]
  [SerializeField] private float _moveSpeed = 5f;
  [SerializeField] private Transform _cameraTransform;

  [Header("Jumping Settings")]
  [SerializeField] private float _jumpForce = 7f;
  [SerializeField] private LayerMask _groundLayer;
  [SerializeField] private float _groudCheckDistance = 0.3f;
  

  private void Awake() {
    _inputSystem = new();
    _rb = GetComponent<Rigidbody>();
  }

  private void OnEnable() {
    _inputSystem.Player.Enable();
    _inputSystem.Player.Jump.performed += HandleJump;
  }

  private void OnDisable() {
    _inputSystem.Player.Jump.performed -= HandleJump;
    _inputSystem.Player.Disable();
  }

  private void Update() => _moveInputValue = _inputSystem.Player.Move.ReadValue<Vector2>();
  private void FixedUpdate() => MovePlayer();

  private void MovePlayer()
  {
    if (!_cameraTransform) return;
    
    Vector3 forward = _cameraTransform.forward;
    Vector3 right = _cameraTransform.right;

    forward.y = 0;
    right.y = 0;
    forward.Normalize();
    right.Normalize();

    Vector3 desiredMoveDirection = forward * _moveInputValue.y + right * _moveInputValue.x;
    _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * desiredMoveDirection);

    if (desiredMoveDirection != Vector3.zero)
    {
      Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
      _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, 10f * Time.fixedDeltaTime));
    } 
  }

  private void HandleJump(InputAction.CallbackContext ctx)
  {
    Vector3 rayStart = transform.position + Vector3.up * 0.1f;
    float rayDistance = 0.5f;

    _isGrounded = Physics.Raycast(rayStart, Vector3.down, rayDistance, _groundLayer, QueryTriggerInteraction.Ignore);

    Debug.DrawRay(rayStart, Vector3.down * rayDistance, _isGrounded ? Color.green : Color.red, 1f);
    Debug.Log($"Grounded: {_isGrounded}");

    if (_isGrounded)
    {
      _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);
      _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groudCheckDistance);
  }
}