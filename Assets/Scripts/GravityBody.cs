using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody: MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private GravityAttractor _planet;
  [SerializeField] private float _force = 5f;

  private Rigidbody _rb;

  private void Start()
  {
    _rb = GetComponent<Rigidbody>();
    _rb.useGravity = false;

    Vector3 directionToPlanet = (_planet.transform.position - transform.position).normalized;
    Vector3 orbitDirection = Vector3.Cross(directionToPlanet, Vector3.up).normalized;
    _rb.linearVelocity = orbitDirection * _force;
  }

  private void FixedUpdate()
  {
    if (_planet != null)
      _planet.Attract(_rb);
  }
}