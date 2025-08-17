using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MazeRow
{
    public List<int> row;
}

[CreateAssetMenu(fileName = "MazeData", menuName = "Scriptable Objects/MazeData")]
public class MazeData : ScriptableObject
{
    public int width;
    public int length;

    public List<MazeRow> maze;

    public float cameraRange;

    public GameObject camera;
    public void SetCamPos()
    {
        if (camera == null)
        {
            Debug.LogError("Camera has not been assigned!");
            return;
        }

        camera.transform.position = new Vector3(width / 2, cameraRange, length / 2);
    }

    public void Resize()
    {
        maze = new List<MazeRow>();
        for (int y = 0; y < length; y++)
        {
            MazeRow newRow = new MazeRow();
            newRow.row = new List<int>();
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == length - 1)
                    newRow.row.Add(1);
                else
                    newRow.row.Add(0);
            }
            maze.Add(newRow);
        }
    }
}
