using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Block
{
    [SerializeField] long scoreValue = 1000;

    [Header("State")]
    [SerializeField] EnemyState state = EnemyState.Idle;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float rotateSpeed = 90f;

    [Header("Chase")]
    [SerializeField] private float chaseTimeout = 2.5f;

    private List<Vector3> path;
    private int currentStep;
    private bool isMoving;

    private Transform enemy;
    private Animator animator;

    private float timer;

    private Quaternion targetRotation;
    private Vector3 rotationOffsetEuler = new Vector3(90, -90f, 0);

    // ===================== UNITY =====================

    private void Awake()
    {
        enemy = transform;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Debug.Log(state);
        switch (state)
        {
            case EnemyState.Idle:
                UpdateIdle();
                break;

            case EnemyState.Patrol:
                UpdateMove();
                break;

            case EnemyState.Chase:
                UpdateMove();
                UpdateChaseTimer();
                break;
        }
    }

    // ===================== STATE =====================

    void UpdateIdle()
    {
        isMoving = false;

        Vector3 snapPos = new Vector3(
        Mathf.Round(transform.position.x),
        transform.position.y,
        Mathf.Round(transform.position.z)
    );

        transform.position = Vector3.Lerp(
            transform.position,
            snapPos,
            Time.deltaTime * 8f
        );
    }

    // ===================== MOVE =====================

    void UpdateMove()
    {
        if (!isMoving || path == null || currentStep >= path.Count)
            return;

        if (state == EnemyState.Chase)
            timer += Time.deltaTime;

        Vector3 targetPos = path[currentStep];

        enemy.position = Vector3.MoveTowards(
            enemy.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(enemy.position, targetPos) <= 0.05f)
        {
            enemy.position = targetPos;
            currentStep++;

            if (currentStep < path.Count)
            {
                Vector3 nextDir = (path[currentStep] - enemy.position).normalized;
                UpdateRotation(nextDir);
            }
            else
            {
                isMoving = false;
                state = EnemyState.Idle;
            }
        }

        enemy.rotation = Quaternion.Slerp(
            enemy.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    // ===================== CHASE =====================

    void UpdateChaseTimer()
    {
        if (timer >= chaseTimeout)
        {
            currentStep = 0;
            path.Clear();
            timer = 0;
            state = EnemyState.Idle;
        }
    }

    public void OnAlert(List<Vector3Int> newPath)
    {
        if (newPath == null || newPath.Count == 0)
            return;

        timer = 0;

        path = new List<Vector3>();
        path.Add(enemy.position);

        foreach (var step in newPath)
            path.Add(new Vector3(step.x, enemy.position.y, step.z));

        currentStep = 0;
        isMoving = path.Count > 1;

        state = EnemyState.Chase;
        // animator.SetBool("isMoving", true);
    }

    // ===================== ROTATION =====================

    void UpdateRotation(Vector3 direction)
    {
        if (direction == Vector3.zero) return;

        Quaternion lookRot = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion offset = Quaternion.Euler(rotationOffsetEuler);
        targetRotation = lookRot * offset;
    }

    // ===================== TRIGGER =====================

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyManager.Instance.TriggerAllEnemies(other.transform.position);
            GameManager.Instance.AddScore(scoreValue);
            Destroy(this.gameObject);
        }
    }
}

// ===================== ENUM =====================

public enum EnemyState
{
    Idle,
    Patrol,
    Chase
}

