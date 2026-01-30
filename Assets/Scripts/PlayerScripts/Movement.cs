using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    Rigidbody2D _rb2d;
    Vector2 _moveInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb2d.linearVelocity = _moveInput * _moveSpeed;
    }

    private void OnMove(InputValue input)
    {
        _moveInput = input.Get<Vector2>();
    }
}
