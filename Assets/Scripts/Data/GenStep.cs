using Client;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class GenStep 
{
    public static List<GenStep> genStepList = new List<GenStep>();
    public abstract void Generate(Map map);
}


public class GrassGenStep : GenStep 
{
    public override void Generate(Map map)
    {
        foreach (MapLayer layer in map.layers.Values)
        {
            if (layer.ZLevel <= map.zLevels * 0.4)
            {
                if (layer.ZLevel > 0)
                {
                    foreach (TileData tile in layer.tiles.Values)
                    {
                        tile.type = TerrainBase.FindTerrainByID("Dirt");
                    }
                } 
                else 
                {
                    foreach (TileData tile in layer.tiles.Values)
                    {
                        tile.type = TerrainBase.FindTerrainByID("Grass");
                    }
                }
            }
        }
    }
}

public class WaterGenStep : GenStep 
{
    public override void Generate(Map map)
    {
        foreach (MapLayer layer in map.layers.Values)
        {
            if (layer.ZLevel <= map.zLevels * 0.4)
            {
                foreach (TileData tile in layer.tiles.Values)
                {
                    float xCoord = (float)tile.position.x / map.size.x * map.scale + map.offset.x;
                    float yCoord = (float)tile.position.y / map.size.y * map.scale + map.offset.y;
                    float value = Mathf.Clamp(Mathf.PerlinNoise(xCoord * 2, yCoord * 2), 0, 0.999999f);
                    if (value * (layer.ZLevel + 2) * 0.5 < 0.30) tile.type = TerrainBase.FindTerrainByID("Water");
                }
            }
        }
    }
}

public class MagmaGenStep : GenStep
{
    public override void Generate(Map map)
    {
        foreach (MapLayer layer in map.layers.Values)
        {
            if (layer.ZLevel > map.zLevels * 0.70) {
                foreach (TileData tile in layer.tiles.Values)
                {
                    float xCoord = (float)tile.position.x / map.size.x * map.scale + map.offset.x;
                    float yCoord = (float)tile.position.y / map.size.y * map.scale + map.offset.y;
                    float value = Mathf.Clamp(Mathf.PerlinNoise(xCoord * 2, yCoord * 2), 0, 0.999999f);
                    if (value * (layer.ZLevel - map.zLevels - 1) * -1 * 0.25 < 0.30) tile.type = TerrainBase.FindTerrainByID("Magma");
                }
            }
        }
    }
}

public class UnderGroundStep : GenStep
{
    public override void Generate(Map map)
    {
        foreach (MapLayer layer in map.layers.Values)
        {
            foreach (TileData tile in layer.tiles.Values)
            {
                float xCoord = (float)tile.position.x / map.size.x * map.scale + map.offset.x;
                float yCoord = (float)tile.position.y / map.size.y * map.scale + map.offset.y;
                float value = Mathf.Clamp(Mathf.PerlinNoise(xCoord * 3, yCoord * 3), 0, 0.999999f);
                if (value < layer.ZLevel * 0.2) tile.type = TerrainBase.FindTerrainByID("Stone");
            }
        }
    }
}

public class CaveStep : GenStep
{
    public override void Generate(Map map)
    {
    }
}