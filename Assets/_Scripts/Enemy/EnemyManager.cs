using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Overlays;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Singleton
    public static EnemyManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    #endregion

    [SerializeField] MazeData mazeData;
    List<EnemyController> enemies = new List<EnemyController>();

    void Start()
    {
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).ToList();
    }

    public void TriggerAllEnemies(Vector3 triggerPos)
    {
        if (enemies == null) Debug.Log("end Game");
        Vector3Int playerPos = Vector3Int.RoundToInt(triggerPos);
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;
            Vector3Int ePos = Vector3Int.RoundToInt(enemy.transform.position);
            enemy.OnAlert(FindPath(ePos, playerPos));
        }
    }


    private Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),   // right
        new Vector3Int(-1, 0, 0),  // left
        new Vector3Int(0,0, 1),   // foward
        new Vector3Int(0,0, -1)   // back
    };

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal)
    {

        var open = new List<Node>();
        var closed = new HashSet<Vector3Int>();

        Node startNode = new Node(start, null, 0, Heuristic(start, goal));
        open.Add(startNode);

        while (open.Count > 0)
        {
            // get node f min
            open.Sort((a, b) => a.f.CompareTo(b.f));
            Node current = open[0];
            open.RemoveAt(0);

            if (current.position == goal)
                return ReconstructPath(current);

            closed.Add(current.position);

            foreach (var dir in directions)
            {
                Vector3Int neighborPos = current.position + dir;

                if (!IsInside(neighborPos) || IsWall(neighborPos) || closed.Contains(neighborPos))
                    continue;

                float tentativeG = current.g + 1; // cost = 1 per step

                Node neighbor = open.Find(n => n.position == neighborPos);
                if (neighbor == null)
                {
                    neighbor = new Node(neighborPos, current, tentativeG, Heuristic(neighborPos, goal));
                    open.Add(neighbor);
                }
                else if (tentativeG < neighbor.g)
                {
                    neighbor.g = tentativeG;
                    neighbor.f = neighbor.g + neighbor.h;
                    neighbor.parent = current;
                }
            }
        }
        return null;
    }

    private bool IsInside(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < mazeData.width && pos.z >= 0 && pos.z < mazeData.length;
    }

    private bool IsWall(Vector3Int pos)
    {
        return mazeData.maze[pos.z].row[pos.x] == 1; // z = row, x = col
    }

    private float Heuristic(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.z - b.z); // Manhattan
    }

    private List<Vector3Int> ReconstructPath(Node node)
    {
        var path = new List<Vector3Int>();
        while (node != null)
        {
            path.Add(node.position);
            node = node.parent;
        }
        path.Reverse();
        return path;
    }

    private class Node
    {
        public Vector3Int position;
        public Node parent;
        public float g, h, f;

        public Node(Vector3Int pos, Node parent, float g, float h)
        {
            this.position = pos;
            this.parent = parent;
            this.g = g;
            this.h = h;
            this.f = g + h;
        }
    }
}