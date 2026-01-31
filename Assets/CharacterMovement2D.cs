using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement2D : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float stopDistance = 0.05f;
    [SerializeField] private InputActionReference moveActionRef;

    [Header("Room Bounds")]
    [SerializeField] private SpriteRenderer roomAreaSprite; // Assign the room area sprite in inspector

    Rigidbody2D rb;
    Vector2 targetPosition;
    bool hasWaypoint = false;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnEnable()
    {
        if (moveActionRef?.action == null) { enabled = false; return; }
        moveActionRef.action.Enable();
        moveActionRef.action.performed += OnSetWaypoint;
    }

    void OnDisable()
    {
        if (moveActionRef?.action == null) return;
        moveActionRef.action.performed -= OnSetWaypoint;
        moveActionRef.action.Disable();
    }

    void OnSetWaypoint(InputAction.CallbackContext ctx)
    {
        if (Mouse.current == null) return;
        var screenPos = Mouse.current.position.ReadValue();
        var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -Camera.main.transform.position.z));
        if (roomAreaSprite != null)
        {
            var bounds = roomAreaSprite.bounds;
            worldPos.x = Mathf.Clamp(worldPos.x, bounds.min.x, bounds.max.x);
            worldPos.y = Mathf.Clamp(worldPos.y, bounds.min.y, bounds.max.y);
        }
        targetPosition = new Vector2(worldPos.x, worldPos.y);
        hasWaypoint = true;
    }

    void FixedUpdate()
    {
        if (!hasWaypoint) return;
        var pos = rb.position;
        var dir = targetPosition - pos;
        var dist = dir.magnitude;
        if (dist > stopDistance)
        {
            var move = dir.normalized * moveSpeed * Time.fixedDeltaTime;
            if (move.magnitude > dist) move = dir;
            rb.MovePosition(pos + move);
        }
        else
        {
            rb.MovePosition(targetPosition);
            hasWaypoint = false;
        }
    }
}
