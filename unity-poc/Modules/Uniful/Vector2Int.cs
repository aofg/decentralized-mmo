using System;
using UnityEngine;

namespace Uniful
{
    public struct Vector2Int
    {
        public int x;
        public int y;

        public int this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return x;
                }
                if (index != 1)
                {
                    throw new IndexOutOfRangeException("Invalid Vector2Int index!");
                }
                return y;
            }
            set
            {
                if (index != 0)
                {
                    if (index != 1)
                    {
                        throw new IndexOutOfRangeException("Invalid Vector2Int index!");
                    }
                    y = value;
                }
                else
                {
                    x = value;
                }
            }
        }

        public Vector2Int normalized
        {
            get
            {
                var result = new Vector2Int(x, y);
                result.Normalize();
                return result;
            }
        }

        public int magnitude
        {
            get { return Mathf.Abs(x) + Mathf.Abs(y); }
        }

        public static Vector2Int zero
        {
            get { return new Vector2Int(0, 0); }
        }

        public static Vector2Int one
        {
            get { return new Vector2Int(1, 1); }
        }

        public static Vector2Int up
        {
            get { return new Vector2Int(0, 1); }
        }

        public static Vector2Int right
        {
            get { return new Vector2Int(1, 0); }
        }

        public Vector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Set(int new_x, int new_y)
        {
            x = new_x;
            y = new_y;
        }

        public static Vector2Int Scale(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x*b.x, a.y*b.y);
        }

        public void Scale(Vector2Int scale)
        {
            x *= scale.x;
            y *= scale.y;
        }

        public void Normalize()
        {
            x = Mathf.Max(1, Mathf.Abs(x));
            y = Mathf.Max(1, Mathf.Abs(y));
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1})", x, y);
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1})", x.ToString(format), y.ToString(format));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2;
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector2Int))
            {
                return false;
            }
            var vector = (Vector2Int) other;
            return x.Equals(vector.x) && y.Equals(vector.y);
        }

        public static int Dot(Vector2Int lhs, Vector2Int rhs)
        {
            return lhs.x*rhs.x + lhs.y*rhs.y;
        }

        public static int Distance(Vector2Int a, Vector2Int b)
        {
            return (a - b).magnitude;
        }

        public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs)
        {
            return new Vector2Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y));
        }

        public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs)
        {
            return new Vector2Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y));
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        public static Vector2Int operator -(Vector2Int a)
        {
            return new Vector2Int(-a.x, -a.y);
        }

        public static Vector2Int operator *(Vector2Int a, int d)
        {
            return new Vector2Int(a.x*d, a.y*d);
        }

        public static Vector2Int operator *(int d, Vector2Int a)
        {
            return new Vector2Int(a.x*d, a.y*d);
        }

        public static Vector2Int operator /(Vector2Int a, int d)
        {
            return new Vector2Int(a.x/d, a.y/d);
        }

        public static Vector2Int operator %(Vector2Int a, int d)
        {
            return new Vector2Int(a.x % d, a.y % d);
        }

        public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
        {
            return (lhs - rhs).magnitude == 0;
        }

        public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
        {
            return (lhs - rhs).magnitude != 0;
        }

        public static implicit operator Vector2Int(Vector3Int v)
        {
            return new Vector2Int(v.x, v.y);
        }

        public static implicit operator Vector3(Vector2Int v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }
    }
}