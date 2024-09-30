using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGen : MonoBehaviour
{
	/*
	public int mapWidth;
	public int mapHeight;
	public float scale;

	public int octaves;
	[Range(0, 1)]
	public float roughness;
	public float refinement;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;

	public void generateMap()
    {
        float[,] noiseMap = Noise.createNoiseMap(mapWidth, mapHeight, scale, seed, octaves, roughness, refinement, offset);

        MapDisplay display = FindAnyObjectByType<MapDisplay>();

        display.drawNoiseMap(noiseMap);

    }

    */


	public enum DrawMode { NoiseMap, ColorMap};
	public DrawMode drawMode;

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;

	public Terrain[] regions;

	public void GenerateMap()
	{
		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

		Color[] colorMap = new Color[mapWidth * mapHeight];

		for(int y=0; y<mapHeight; y++)
        {
			for(int x=0; x<mapWidth; x++)
            {
				float currentHeight = noiseMap[x, y];
				for(int z = 0; z<regions.Length; z++)
                {
					if (currentHeight <= regions[z].height)
                    {
						colorMap[y * mapWidth + x] = regions[z].Color;
						break;
                    }
                }
            }
        }


		MapDisplay display = FindObjectOfType<MapDisplay>();
		if (drawMode == DrawMode.NoiseMap)
		{
			display.drawTexture(TextureGen.TextureFromHeightMap(noiseMap));
		}
		else if(drawMode == DrawMode.ColorMap)
        {
			display.drawTexture(TextureGen.TextureFromColorMap(colorMap,mapWidth,mapHeight));

		}
	}

	void OnValidate()
	{
		if (mapWidth < 1)
		{
			mapWidth = 1;
		}
		if (mapHeight < 1)
		{
			mapHeight = 1;
		}
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}
		if (octaves < 0)
		{
			octaves = 0;
		}
	}

	[System.Serializable]
	public struct Terrain{
			public string Name;
			public float height;
			public Color Color;
		}

		

}
