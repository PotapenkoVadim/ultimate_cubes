using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WaterBuoyancy : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float _waterLevel = 5f;
  [SerializeField] private float _floatingPower = 15f;
  [SerializeField] private float _waterDrag = 3f;
  [SerializeField] private float _waterAngularDrag = 1f;

  private Rigidbody _rb;
  private float _defaultDrag;
  private float _defaultAngularDrag;

  private void Start()
  {
    _rb = GetComponent<Rigidbody>();
    _defaultDrag = _rb.linearDamping;
    _defaultAngularDrag = _rb.angularDamping;
  }

  private void FixedUpdate()
  {
    float difference = transform.position.y - _waterLevel;

    if (difference < 0)
    {
      _rb.AddForceAtPosition(
        Vector3.up * _floatingPower * Mathf.Abs(difference),
        transform.position,
        ForceMode.Acceleration // ps: игнорируем массу объекта
      );
      
      _rb.linearDamping = _waterDrag;
      _rb.angularDamping = _waterAngularDrag;
    } else
    {
      _rb.linearDamping = _defaultDrag;
      _rb.angularDamping = _defaultAngularDrag;
    }
  }
}
