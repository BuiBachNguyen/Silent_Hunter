using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50.0f;

    private List<Vector3> path;
    private int currentStep = 0;
    private bool isMoving = false;
    private Transform player;

    private void Awake()
    {
        player = transform; // nếu script gắn trên Player
    }

    public void SetPath(List<Vector3Int> newPath)
    {
        path = new List<Vector3>();
        path.Add(player.position); // thêm vị trí hiện tại làm gốc
        foreach (var step in newPath)
        {
            path.Add(new Vector3(step.x, player.position.y, step.z));
        }
        currentStep = 0;
        isMoving = path.Count > 0;
    }

    private void Update()
    {
        if (!isMoving || path == null || currentStep >= path.Count)
            return;

        Vector3 targetPos = path[currentStep];
        player.position = Vector3.MoveTowards(
            player.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(player.position, targetPos) <= 0.05f)
        {
            // Snap để tránh sai số float
            player.position = targetPos;
            currentStep++;

            if (currentStep >= path.Count)
            {
                isMoving = false; // đã đi hết path
            }
        }
    }
}

