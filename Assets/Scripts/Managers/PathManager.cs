using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class PathManager
    {
        public static readonly string json = "Json";
        public static readonly string jsonCreatures = Path.Combine(json, "Creature");
        public static readonly string jsonTerrain = Path.Combine(json, "Terrain");
        public static readonly string jsonBuilding = Path.Combine(json, "Building");
        public static readonly string jsonBiome = Path.Combine(json, "Biome");

        public static readonly string textures = "Textures";
        public static readonly string texturesCreatures = Path.Combine(textures, "Creature");
        public static readonly string texturesTerrain = Path.Combine(textures, "Terrain");
        public static readonly string texturesBuilding = Path.Combine(textures, "Building");
        public static readonly string texturesBiome = Path.Combine(textures, "Biome");
    }
}
