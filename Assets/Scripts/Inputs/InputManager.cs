using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
  private InputActions inputActions;

  public delegate void MoveAction(InputAction.CallbackContext context);
  public static event MoveAction OnMove;

  private void Awake()
  {
    inputActions = new InputActions();
    inputActions.Player.Move.performed += ctx => OnMove(ctx);
  }

  private void OnEnable()
  {
    inputActions.Enable();
  }

  private void OnDisable()
  {
    inputActions.Disable();
  }
}
