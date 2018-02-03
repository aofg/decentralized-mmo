using System;
using UnityEngine;

namespace Uniful
{
    public struct Vector3Int
    {
        public int x;
        public int y;
        public int z;

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3Int index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3Int index!");
                }
            }
        }

        public Vector3Int normalized
        {
            get { return Normalize(this); }
        }

        public int magnitude
        {
            get { return Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z); }
        }

        public static Vector3Int zero
        {
            get { return new Vector3Int(0, 0, 0); }
        }

        public static Vector3Int one
        {
            get { return new Vector3Int(1, 1, 1); }
        }

        public static Vector3Int forward
        {
            get { return new Vector3Int(0, 0, 1); }
        }

        public static Vector3Int back
        {
            get { return new Vector3Int(0, 0, -1); }
        }

        public static Vector3Int up
        {
            get { return new Vector3Int(0, 1, 0); }
        }

        public static Vector3Int down
        {
            get { return new Vector3Int(0, -1, 0); }
        }

        public static Vector3Int left
        {
            get { return new Vector3Int(-1, 0, 0); }
        }

        public static Vector3Int right
        {
            get { return new Vector3Int(1, 0, 0); }
        }

        public Vector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3Int(int x, int y)
        {
            this.x = x;
            this.y = y;
            z = 0;
        }

        public void Set(int new_x, int new_y, int new_z)
        {
            x = new_x;
            y = new_y;
            z = new_z;
        }

        public static Vector3Int Scale(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x*b.x, a.y*b.y, a.z*b.z);
        }

        public void Scale(Vector3Int scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        public static Vector3Int Cross(Vector3Int lhs, Vector3Int rhs)
        {
            return new Vector3Int(lhs.y*rhs.z - lhs.z*rhs.y, lhs.z*rhs.x - lhs.x*rhs.z, lhs.x*rhs.y - lhs.y*rhs.x);
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() << 2 ^ z.GetHashCode() >> 2;
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3Int))
            {
                return false;
            }
            var vector = (Vector3Int) other;
            return x.Equals(vector.x) && y.Equals(vector.y) && z.Equals(vector.z);
        }

        public static Vector3Int Reflect(Vector3Int inDirection, Vector3Int inNormal)
        {
            return -2*Dot(inNormal, inDirection)*inNormal + inDirection;
        }

        public static Vector3Int Normalize(Vector3Int value)
        {
            value.x = Mathf.Max(1, Mathf.Abs(value.x));
            value.y = Mathf.Max(1, Mathf.Abs(value.y));
            value.z = Mathf.Max(1, Mathf.Abs(value.z));

            return value;
        }

        public void Normalize()
        {
            x = Mathf.Max(1, Mathf.Abs(x));
            y = Mathf.Max(1, Mathf.Abs(y));
            z = Mathf.Max(1, Mathf.Abs(z));
        }

        public override string ToString()
        {
            return string.Format("({0:F1}, {1:F1}, {2:F1})", new object[]
            {
                x,
                y,
                z
            });
        }

        public string ToString(string format)
        {
            return string.Format("({0}, {1}, {2})", new object[]
            {
                x.ToString(format),
                y.ToString(format),
                z.ToString(format)
            });
        }

        public static int Dot(Vector3Int lhs, Vector3Int rhs)
        {
            return lhs.x*rhs.x + lhs.y*rhs.y + lhs.z*rhs.z;
        }

        public static int Distance(Vector3Int a, Vector3Int b)
        {
            return (b - a).magnitude;
        }

        public static int Magnitude(Vector3Int a)
        {
            return Mathf.Abs(a.x) + Mathf.Abs(a.y) + Mathf.Abs(a.z);
        }

        public static Vector3Int Min(Vector3Int lhs, Vector3Int rhs)
        {
            return new Vector3Int(Mathf.Min(lhs.x, rhs.x), Mathf.Min(lhs.y, rhs.y), Mathf.Min(lhs.z, rhs.z));
        }

        public static Vector3Int Max(Vector3Int lhs, Vector3Int rhs)
        {
            return new Vector3Int(Mathf.Max(lhs.x, rhs.x), Mathf.Max(lhs.y, rhs.y), Mathf.Max(lhs.z, rhs.z));
        }

        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3Int operator -(Vector3Int a)
        {
            return new Vector3Int(-a.x, -a.y, -a.z);
        }

        public static Vector3Int operator *(Vector3Int a, int d)
        {
            return new Vector3Int(a.x*d, a.y*d, a.z*d);
        }

        public static Vector3Int operator *(int d, Vector3Int a)
        {
            return new Vector3Int(a.x*d, a.y*d, a.z*d);
        }

        public static Vector3Int operator /(Vector3Int a, int d)
        {
            return new Vector3Int(a.x/d, a.y/d, a.z/d);
        }

        public static bool operator ==(Vector3Int lhs, Vector3Int rhs)
        {
            return Magnitude(lhs - rhs) == 0;
        }

        public static bool operator !=(Vector3Int lhs, Vector3Int rhs)
        {
            return Magnitude(lhs - rhs) != 0;
        }

        public static implicit operator Vector3(Vector3Int vec3)
        {
            return new Vector3(vec3.x, vec3.y, vec3.z);
        }

        public static implicit operator Vector3Int(Vector3 vec3)
        {
            return new Vector3Int(Mathf.FloorToInt(vec3.x), Mathf.FloorToInt(vec3.y), Mathf.FloorToInt(vec3.z));
        }
    }
}