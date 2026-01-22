using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
  [Header("Main Settings")]
  [SerializeField] private Transform _target;
  [SerializeField] private float _distance = 5.0f;
  [SerializeField] private float _sensitivity = 3.0f;

  private float _rotationX = 0;
  private float _rotationY = 0;
  private int _scrollFrameLock = 0;

  private InputSystem_Actions _inputSystem;

  private void Awake() =>_inputSystem = new();
  private void OnEnable() => _inputSystem.Player.Enable();
  private void OnDisable() => _inputSystem.Player.Disable();

  private void LateUpdate()
  {
    if (!_target) return;

    HandleScroll();
    RotateCamera();
    DecreaseScrollFrames();
  }

  private void RotateCamera()
  {
    if (_scrollFrameLock > 0) return;

    Vector2 lookInput = _inputSystem.Player.Look.ReadValue<Vector2>();

    _rotationY += lookInput.x * _sensitivity;
    _rotationX -= lookInput.y * _sensitivity;

    _rotationX = Mathf.Clamp(_rotationX, -30, 60);

    Quaternion rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
    Vector3 position = _target.position - (rotation * Vector3.forward * _distance);

    transform.SetPositionAndRotation(position, rotation);
  }

  private void HandleScroll()
  {
    Vector2 scrollValue = _inputSystem.Player.ScrollMouse.ReadValue<Vector2>();

    if (scrollValue.y != 0)
    {
      _scrollFrameLock = 2;

      float normalizeScrollValue = scrollValue.y * 100f;
      _distance -= normalizeScrollValue;
      _distance = Mathf.Clamp(_distance, 5, 25);
    }
  }

  private void DecreaseScrollFrames() {
    if (_scrollFrameLock > 0)
      _scrollFrameLock -= 1;
  }
}
