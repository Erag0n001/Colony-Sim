using System;

namespace Client
{
    public class Biome
    {
        [NonSerialized] public static BiomeBase[] biomeList;
        public string DisplayName;
        public string IdName;
        public GenStep[] gensteps;
    }
}