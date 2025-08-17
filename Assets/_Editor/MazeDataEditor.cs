using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeData))]
public class MazeDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MazeData mazeData = (MazeData)target;

        // Width & Length
        mazeData.width = EditorGUILayout.IntField("Width", mazeData.width);
        mazeData.length = EditorGUILayout.IntField("Length", mazeData.length);

        // Auto resize nếu cần
        if (mazeData.maze == null ||
            mazeData.maze.Count != mazeData.length ||
            (mazeData.maze.Count > 0 && mazeData.maze[0].row.Count != mazeData.width))
        {
            mazeData.Resize();
        }

        // 🎯 Camera
        mazeData.camera = (GameObject)EditorGUILayout.ObjectField("Camera", mazeData.camera, typeof(GameObject), true);
        mazeData.cameraRange = EditorGUILayout.FloatField("Camera Range", mazeData.cameraRange);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Maze Layout", EditorStyles.boldLabel);

        // Hiển thị dạng bảng
        for (int y = 0; y < mazeData.length; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < mazeData.width; x++)
            {
                mazeData.maze[y].row[x] = EditorGUILayout.IntField(
                    mazeData.maze[y].row[x],
                    GUILayout.Width(30)
                );
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(mazeData);
        }
    }
}
