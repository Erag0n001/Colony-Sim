using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public class TerrainBase
    {
        [NonSerialized] public static TerrainBase[] terrainList;
        public string IdName;
        public string Name;

        public float WalkModifier = 1;

        public string TexturePath;
        [NonSerialized] public Sprite texture;

        public static TerrainBase FindTerrainByID(string name)
        {
            return terrainList.Where(t => t.Name == name).FirstOrDefault();
        }
    }
}
