using UnityEngine;
using UnityEditor;

public class PlayerPlacer : EditorWindow
{
    private float playerHeight = 2f;

    [MenuItem("Tools/Place Player Capsule")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPlacer>("Place Player");
    }

    void OnGUI()
    {
        GUILayout.Label("Player Settings", EditorStyles.boldLabel);
        playerHeight = EditorGUILayout.FloatField("Player Height", playerHeight);

        if (GUILayout.Button("Place Player"))
        {
            PlacePlayer();
        }
    }

    void PlacePlayer()
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.localScale = new Vector3(1, playerHeight / 2f, 1);
        player.transform.position = new Vector3(0, playerHeight / 2f + 0.05f, 0);

        Debug.Log("Player capsule created at origin.");
    }
}
