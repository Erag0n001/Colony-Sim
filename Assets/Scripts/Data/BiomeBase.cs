using System;
using UnityEngine;
using System.Collections.Generic;
namespace Client 
{
    public class BiomeBase 
    {
        [NonSerialized] public static BiomeBase[] biomeList;
        public string DisplayName;
        public string IdName;
        public string[] GenSteps = new string[] { "GrassGenStep", "WaterGenStep" };
        [NonSerialized] public List<GenStep> gensteps = new List<GenStep>();
    }
}