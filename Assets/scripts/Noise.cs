using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    /*
    public static float[,] createNoiseMap(int noiseWidth, int noiseHeight, float scale, int seed, int octave, float roughness, float refinement, Vector2 offset)
    {
        float[,] noiseMap = new float[noiseWidth, noiseHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octave];
        for (int i = 0; i < octave; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.00001f;
        }

        float halfWidth = noiseWidth / 2f;
        float halfHeight = noiseHeight / 2f;

        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        for (int y = 0; y < noiseHeight; y++)
        {
            for (int x = 0; x < noiseWidth; x++)
            {
                float amplitude = 1;
                float frequncy = 1;
                float mapHeight = 0;

                for (int i = 0; i < octave; i++)
                {
                    float samplex = (x - halfWidth) / scale * frequncy + octaveOffsets[i].x;
                    float sampley = (y - halfHeight) / scale * frequncy + octaveOffsets[i].y;


                    float perloinValue = Mathf.PerlinNoise(samplex, sampley) * 2 - 1;
                    mapHeight += perloinValue * amplitude;

                    amplitude = roughness * amplitude;
                    frequncy = refinement * frequncy;
                }

                if (noiseHeight > maxHeight)
                {
                    maxHeight = noiseHeight;
                }
                else if (noiseHeight < minHeight)
                {
                    minHeight = noiseHeight;
                }

                noiseMap[x, y] = mapHeight;
            }
        }

        for (int y = 0; y < noiseHeight; y++)
        {
            for (int x = 0; x < noiseWidth; x++)
            {
                noiseMap[x, y] = Mathf.Lerp(minHeight, maxHeight, noiseMap[x, y]);
            }
        }


        return noiseMap;
    }
}

    */
    



    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
    