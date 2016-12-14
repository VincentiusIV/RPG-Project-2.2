using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGeneratorEditor : Editor {

	public override void OnInspectorGUI()
    {
        WorldGenerator gen = (WorldGenerator)target;

        DrawDefaultInspector();

        if(GUILayout.Button("Generate"))
        {
            gen.GenerateWorld();
        }
    }
}
