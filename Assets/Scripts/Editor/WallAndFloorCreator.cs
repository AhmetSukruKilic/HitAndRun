using UnityEngine;
using UnityEditor;

public class WallAndFloorCreator : EditorWindow
{
    private float floorWidth = 10f;
    private float floorDepth = 5f;
    private float wallHeight = 2f;
    private float wallThickness = 0.5f;

    [MenuItem("Tools/Generate Floor and Walls")]
    public static void ShowWindow()
    {
        GetWindow<WallAndFloorCreator>("Wall and Floor Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Floor Settings", EditorStyles.boldLabel);
        floorWidth = EditorGUILayout.FloatField("Width", floorWidth);
        floorDepth = EditorGUILayout.FloatField("Depth", floorDepth);

        GUILayout.Label("Wall Settings", EditorStyles.boldLabel);
        wallHeight = EditorGUILayout.FloatField("Height", wallHeight);
        wallThickness = EditorGUILayout.FloatField("Thickness", wallThickness);

        if (GUILayout.Button("Generate"))
        {
            GenerateFloorAndWalls();
        }
    }

    void GenerateFloorAndWalls()
    {
        GameObject parent = new GameObject("GeneratedArea");

        // Floor
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.localScale = new Vector3(floorWidth, 0.1f, floorDepth);
        floor.transform.position = new Vector3(0, 0, 0);
        floor.name = "Floor";
        floor.transform.parent = parent.transform;

        // Left Wall
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftWall.transform.localScale = new Vector3(wallThickness, wallHeight, floorDepth);
        leftWall.transform.position = new Vector3(-floorWidth / 2f + wallThickness / 2f, wallHeight / 2f, 0);
        leftWall.name = "Left Wall";
        leftWall.transform.parent = parent.transform;

        // Right Wall
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightWall.transform.localScale = new Vector3(wallThickness, wallHeight, floorDepth);
        rightWall.transform.position = new Vector3(floorWidth / 2f - wallThickness / 2f, wallHeight / 2f, 0);
        rightWall.name = "Right Wall";
        rightWall.transform.parent = parent.transform;

        Debug.Log("Generated floor and walls successfully!");
    }
}
