using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    #region Singleton
    public static MapGenerator Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetUp();
    }
    #endregion

    [SerializeField] MazeData mazeData;
    [SerializeField] Block[,] map;
    [SerializeField] List<GameObject> obstaclesPrefabs;
    [SerializeField] private new GameObject camera;
    void SetUp()
    {
        // Gán Main Camera vào MazeData khi bắt đầu game
        mazeData.camera = camera; //Camera.main.gameObject;

        // Gọi hàm đặt vị trí camera
        mazeData.SetCamPos();

        // MAZE (1 = wall, 0 = lane)
        List<MazeRow> maze = mazeData.maze;

        map = new Block[mazeData.width, mazeData.length];

        for (int z = 0; z < mazeData.length; z++)
        {
            for (int x = 0; x < mazeData.width; x++)
            {
                Vector3 pos = new Vector3(x, 1, z);

                int cellValue = maze[z].row[x];

                GameObject go;

                switch (cellValue)
                {
                    case 0:
                        // Instantiate empty block prefab
                        go = Instantiate(obstaclesPrefabs[0], pos, Quaternion.identity, this.transform);
                        map[x, z] = go.GetComponent<EmptyBlock>();
                        break;

                    case 1:
                        // Instantiate wall prefab
                        go = Instantiate(obstaclesPrefabs[1], pos, Quaternion.identity, this.transform);
                        map[x, z] = go.GetComponent<Wall>();
                        break;
                    case 2:
                        go = Instantiate(obstaclesPrefabs[0], pos, Quaternion.identity, this.transform);
                        map[x, z] = go.GetComponent<EmptyBlock>();
                        GameObject enemy = Instantiate(obstaclesPrefabs[2], pos, Quaternion.EulerRotation(90, -90f, 0), this.transform);
                        break;
                    default:
                        // Instantiate empty block prefab
                        go = Instantiate(obstaclesPrefabs[0], pos, Quaternion.identity, this.transform);
                        map[x, z] = go.GetComponent<EmptyBlock>();
                        break;
                }
            }
        }
    }
    void Start()
    {

    }
}
