using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleEnemyAI : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase }
    public State currentState = State.Idle;

    public float idleTime = 2f;
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;

    private float stateTimer = 0f;
    private Vector3 patrolTarget;
    private Transform player;

    [HideInInspector] public EnemySpawner spawner;

    private void OnDestroy()
    {
        if (spawner != null)
            spawner.OnEnemyDestroyed();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SetIdle();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0f)
                    SetPatrol();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
        }
    }

    void SetIdle()
    {
        currentState = State.Idle;
        stateTimer = idleTime;
    }

    void SetPatrol()
    {
        currentState = State.Patrol;
        patrolTarget = transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
    }

    void Patrol()
    {
        if (player && Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = State.Chase;
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, patrolTarget, patrolSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, patrolTarget) < 0.1f)
            SetIdle();
    }

    void Chase()
    {
        if (!player)
        {
            SetIdle();
            return;
        }
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist > detectionRange)
        {
            SetIdle();
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FightManagerSingleton.Instance.StartRandomFight();
            Time.timeScale = 0f; // Pause the game during the fight 

            FightManagerSingleton.OnFightEnded += ResumeGame;
        }
    }

    private void ResumeGame()
    {
        Destroy(gameObject); // Remove enemy after fight
        Time.timeScale = 1f; // Resume the game
        FightManagerSingleton.OnFightEnded -= ResumeGame;
    }
}
