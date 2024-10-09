using Client;
using System.Collections.Generic;
namespace Client
{
    public class PathfindingInfo
    {
        public List<MapTile> searched = new List<MapTile>();
        public List<MapTile> toSearch = new List<MapTile>();
        public Dictionary<MapTile, float> gCost = new Dictionary<MapTile, float>();
        public Dictionary<MapTile, float> fCost = new Dictionary<MapTile, float>();
        public Dictionary<MapTile, float> hCost = new Dictionary<MapTile, float>();
        public Dictionary<MapTile, MapTile> connection = new Dictionary<MapTile, MapTile>();
    }
}