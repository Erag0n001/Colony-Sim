using System;
using UnityEngine;

namespace Client 
{
    public class Position 
    {
        public int x;
        public int y;
        public int z;


        public Vector2 gameObjectPosition;
        public Vector3Int ToVector3Int() => new Vector3Int(x,y,0);
        public Vector3 ToVector3() => new Vector3(x, y, 0);
        public void SyncGameObjectToGrid() 
        {
            gameObjectPosition = new Vector2(x,y);
        }

        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.gameObjectPosition = new Vector2(x, y);
        }   
        public Position(int x, int y) 
        {
            this.x = x;
            this.y = y;
            this.gameObjectPosition = new Vector2(x, y);
        }
        public Position(Vector3Int v)
        {
            this.x = v.x;
            this.y = v.y;
            this.gameObjectPosition = new Vector2(x, y);
        }
        public Position() { }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static bool operator == (Position a, Position b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Position a, Position b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return false;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return true;
            return a.x != b.x && a.y != b.y && a.z != b.z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position other)
            {
                return x == other.x && y == other.y && z == other.z;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public override string ToString()
        {
            return $"Position({x}, {y}, {z})";
        }
    }
}