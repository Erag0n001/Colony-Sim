using System;
using UnityEngine;

namespace Client 
{
    public class Position 
    {
        public int x;
        public int y;
        public int z;
        public Vector3Int ToVector3Int() => new Vector3Int(x,y,0);
        public Vector3 ToVector3() => new Vector3(x, y, 0);
        public Position(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }   
        public Position(int x, int y) 
        {
            this.x = x;
            this.y = y;
        }
        public Position(Vector3Int v)
        {
            this.x = v.x;
            this.y = v.y;
        }
        public Position() { }

        public static Position operator +(Position a, Position b)
        {
            return new Position(a.x + b.x, a.y + b.y);
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