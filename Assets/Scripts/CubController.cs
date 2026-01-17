using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CubController : MonoBehaviour
{
  private InputSystem_Actions _inputSystem;
  private Rigidbody _rb;
  private float _currentPower = 0f;
  private bool _isCharging = false;

  [Header("Settings")]
  [SerializeField] private float _changeSpeed = 10f;
  [SerializeField] private float _maxPower = 50f;
  [SerializeField] private float _minPower = 2f;

  private void Awake()
  {
    _inputSystem = new();
    _rb = GetComponent<Rigidbody>();
    _rb.interpolation = RigidbodyInterpolation.Interpolate;
  }

  private void OnEnable()
  {
    _inputSystem.Player.Enable();
    _inputSystem.Player.Move.started += OnMovePerformed;
    _inputSystem.Player.Move.performed += OnMovePerformed;
    _inputSystem.Player.Move.canceled += OnMoveCanceled;
  }

  private void OnDisable()
  {
    _inputSystem.Player.Move.started -= OnMovePerformed;
    _inputSystem.Player.Move.performed -= OnMovePerformed;
    _inputSystem.Player.Move.canceled -= OnMoveCanceled;
    _inputSystem.Player.Disable();
  }

  private void OnMovePerformed(InputAction.CallbackContext ctx)
  {
    _isCharging = true;
  }

  private void OnMoveCanceled(InputAction.CallbackContext ctx)
  {
    _isCharging = false;

    if (_currentPower >= _minPower)
    {
      _rb.AddForce(transform.forward * _currentPower, ForceMode.Impulse);
    }

    _currentPower = 0f;
  }

  private void Update()
  {
    ChargePower();
  }

  private void OnCollisionEnter(Collision collision)
  {
    Vector3 currentVelocity = _rb.linearVelocity;
    currentVelocity.y = 0;

    if (currentVelocity.sqrMagnitude > 0.1f)
    {
      transform.rotation = Quaternion.LookRotation(currentVelocity.normalized);
      _rb.angularVelocity = Vector3.zero;
    }
  }

  private void ChargePower()
  {
    if (_isCharging)
    {
      _currentPower += _changeSpeed * Time.deltaTime;
      _currentPower = Mathf.Clamp(_currentPower, 0, _maxPower);
      Debug.Log($"Накопление силы: {_currentPower}");
    }
  }
}
