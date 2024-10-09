using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Client
{
    public class Pathfinding : MonoBehaviour
    {
        public void PlayButtonPressed()
        {
            MapTile start = MainManager.currentMap.GetTileFromVector(new Position(0,0));
            MapTile end = MainManager.currentMap.GetTileFromVector(new Position(25, 25));
            List<MapTile> tile = MainManager.currentMap.layers[0].creatures.First().Value.FindPathToDestination(start,end);
            Debug.Log(tile);
        }
    }
}