using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MazeGenerator))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeGenerator mazeGenerator = (MazeGenerator)target;

        if (GUILayout.Button("Spawn Grid"))
        {
            mazeGenerator.SpawnGrid();
        }
    }
}
