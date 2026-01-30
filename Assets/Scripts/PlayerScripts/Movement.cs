using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;

    [SerializeField] private AStarPathfinding aStarPathfinding;
    Rigidbody2D _rb2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();

        InputManager.OnMove += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        Vector2 inputVector = input;
        // Get the grid position from mouse position

        Vector2Int start = Vector2Int.FloorToInt(transform.position);
        Vector2Int goal = start + Vector2Int.FloorToInt(inputVector);

        var path = aStarPathfinding.FindPath(new Vector3Int(start.x, start.y, 0), new Vector3Int(goal.x, goal.y, 0));
        if (path != null && path.Count > 1)
        {
            StartCoroutine(MoveCoroutine(path.ToArray()));
        }
    }

    private IEnumerator MoveCoroutine(Vector3Int[] path)
    {
        for (int i = 1; i < path.Length; i++)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(path[i].x + 0.5f, path[i].y + 0.5f, 0); // Center of the tile
            float journeyLength = Vector3.Distance(startPos, endPos);
            float startTime = Time.time;

            while (Vector3.Distance(transform.position, endPos) > 0.01f)
            {
                float distCovered = (Time.time - startTime) * _moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
                yield return null;
            }

            transform.position = endPos; // Ensure we end exactly at the target position
        }
    }
}
