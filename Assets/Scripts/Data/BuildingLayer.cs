using System.Collections.Generic;

namespace Client 
{
    public class BuildingLayer 
    {
        public static List<BuildingLayer> allLayers = new List<BuildingLayer>();
        public string name;
        public string description;
        public int priority;
    }
}