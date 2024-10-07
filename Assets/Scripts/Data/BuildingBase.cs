using System;
using UnityEngine;

namespace Client
{
    public class BuildingBase
    {
        [NonSerialized] public static BuildingBase[] creatureList;
        public string DisplayName;
        public string IdName;
        public float movementImpact;

        public string TexturePath;
        [NonSerialized] public Sprite texture;
    }
}