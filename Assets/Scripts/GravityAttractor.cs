using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
  [Header("Settings")]
  [SerializeField] private float _gravityConstant = 9.8f;

  public void Attract(Rigidbody gravityBody)
  {
    Vector3 gravityUp = (gravityBody.position - transform.position).normalized;
    gravityBody.AddForce(-gravityUp * _gravityConstant, ForceMode.Acceleration);

    Quaternion targetRotation = Quaternion.FromToRotation(gravityBody.transform.up, gravityUp) * gravityBody.rotation;
    gravityBody.rotation = Quaternion.Slerp(gravityBody.rotation, targetRotation, 5f * Time.deltaTime);
  }
}
