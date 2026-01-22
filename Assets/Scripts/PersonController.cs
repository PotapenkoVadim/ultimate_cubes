using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PersonController: MonoBehaviour
{
  private InputSystem_Actions _inputSystem;
  private Rigidbody _rb;
  private Vector2 _moveInputValue;

  [Header("Movement Settings")]
  [SerializeField] private float _moveSpeed = 5f;
  [SerializeField] private Transform _cameraTransform;

  private void Awake() {
    _inputSystem = new();
    _rb = GetComponent<Rigidbody>();
  }

  private void OnEnable() => _inputSystem.Player.Enable();
  private void OnDisable() => _inputSystem.Player.Disable();
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
}