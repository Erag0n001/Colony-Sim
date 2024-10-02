using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Client
{
    public class CreatureBase 
    {
        [NonSerialized] public static CreatureBase[] creatureList;
        public string DisplayName;
        public string IdName;
        public string Type;
        public float BaseSpeed;

        public string TexturePath;
        [NonSerialized] public Sprite texture;
    }
}
