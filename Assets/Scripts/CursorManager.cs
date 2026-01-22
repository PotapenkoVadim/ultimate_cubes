using UnityEngine;

public class CursorManager: MonoBehaviour
{
  public static CursorManager Instance;

  private void Awake()
  {
    if (Instance == null) Instance = this;
    else Destroy(gameObject);
  }

  private void Start() => SetCursorState(true);

  private void SetCursorState(bool isLocked)
  {
    Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
    Cursor.visible = !isLocked;
  }
}