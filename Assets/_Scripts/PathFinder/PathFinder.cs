using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.UIElements;

public class PathFinder : MonoBehaviour
{
    #region Singleton
    public static PathFinder Instance;
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

    [SerializeField] private GameObject player;
    [SerializeField] MazeData mazeData;
    [SerializeField] List<Vector3Int> playerToGoals;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
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
            // lấy node có f nhỏ nhất
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

                float tentativeG = current.g + 1; // cost = 1 mỗi bước

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
        return null; // không tìm thấy
    }

    private bool IsInside(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < mazeData.width && pos.z >= 0 && pos.z < mazeData.length;
    }

    private bool IsWall(Vector3Int pos)
    {
        return mazeData.maze[pos.z].row[pos.x] == 1; // chú ý: z = row, x = col
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

    public void HandleObjectHovering(Vector3 goal)
    {
        Vector3Int startPos = Vector3Int.RoundToInt(player.transform.position);
        Vector3Int goalPos = Vector3Int.RoundToInt(goal);

        playerToGoals = FindPath(startPos, goalPos);

        if (playerToGoals != null)
        {
            //Debug.Log("Path length: " + playerToGoals.Count);
            //foreach (var step in playerToGoals)
            //    Debug.Log("Step: " + step);
            StartCoroutine(MoveAlongPath(playerToGoals));
        }
        else
        {
            Debug.Log("Không tìm thấy đường!");
        }
    }

    [SerializeField] private float moveSpeed = 5f;
    private Coroutine moveCoroutine;

    private IEnumerator MoveAlongPath(List<Vector3Int> path)
    {
        foreach (var step in path)
        {
            Vector3 targetPos = new Vector3(step.x, player.transform.position.y, step.z);

            // Di chuyển tới vị trí step
            while (Vector3.Distance(player.transform.position, targetPos) > 0.05f)
            {
                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    targetPos,
                    moveSpeed * Time.deltaTime
                );
                yield return null; // đợi frame sau
            }

            // Snap về đúng vị trí (tránh sai số float)
            player.transform.position = targetPos;
        }

        moveCoroutine = null; // reset khi xong
    }

}
