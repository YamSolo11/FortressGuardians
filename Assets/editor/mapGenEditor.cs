using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



[CustomEditor(typeof(mapGen))]
public class mapGenEditor : Editor
{

	public override void OnInspectorGUI()
	{
		mapGen mapGen = (mapGen)target;

		if (DrawDefaultInspector())
		{
			if (mapGen.autoUpdate)
			{
				mapGen.GenerateMap();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			mapGen.GenerateMap();
		}
	}
}


/*
[CustomEditor(typeof(mapGen))]
public class MapGeneratorEditor : Editor
{

	public override void OnInspectorGUI()
	{
		mapGen mapGen = (mapGen)target;

		if (DrawDefaultInspector())
		{
			if (mapGen.autoUpdate)
			{
				mapGen.GenerateMap();
			}
		}

		if (GUILayout.Button("Generate"))
		{
			mapGen.GenerateMap();
		}
	}
}
*/