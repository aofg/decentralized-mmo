using UnityEngine;

namespace Uniful
{
    public struct RectInt
    {
        public int x { get; set; }

        public int y { get; set; }

        public Vector2Int position
        {
            get { return new Vector2Int(x, y); }
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        public Vector2Int center
        {
            get { return new Vector2Int(x + width/2, y + height/2); }
            set
            {
                x = value.x - width/2;
                y = value.y - height/2;
            }
        }

        public Vector2Int min
        {
            get { return new Vector2Int(xMin, yMin); }
            set
            {
                xMin = value.x;
                yMin = value.y;
            }
        }

        public Vector2Int max
        {
            get { return new Vector2Int(xMax, yMax); }
            set
            {
                xMax = value.x;
                yMax = value.y;
            }
        }

        public int width { get; set; }

        public int height { get; set; }

        public Vector2Int size
        {
            get { return new Vector2Int(width, height); }
            set
            {
                width = value.x;
                height = value.y;
            }
        }

        public int xMin
        {
            get { return x; }
            set
            {
                var xMax = this.xMax;
                x = value;
                width = xMax - x;
            }
        }

        public int yMin
        {
            get { return y; }
            set
            {
                var yMax = this.yMax;
                y = value;
                height = yMax - y;
            }
        }

        public int xMax
        {
            get { return width + x; }
            set { width = value - x; }
        }

        public int yMax
        {
            get { return height + y; }
            set { height = value - y; }
        }

        public RectInt(int left, int top, int width, int height)
        {
            x = left;
            y = top;
            this.width = width;
            this.height = height;
        }

        public RectInt(RectInt source)
        {
            x = source.x;
            y = source.y;
            width = source.width;
            height = source.height;
        }

        public static RectInt MinMaxRect(int left, int top, int right, int bottom)
        {
            return new RectInt(left, top, right - left, bottom - top);
        }

        public void Set(int left, int top, int width, int height)
        {
            x = left;
            y = top;
            this.width = width;
            this.height = height;
        }

        public override string ToString()
        {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", x, y, width, height);
        }

        public string ToString(string format)
        {
            return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})",
                x.ToString(format),
                y.ToString(format),
                width.ToString(format),
                height.ToString(format));
        }

        public bool Contains(Vector2 point)
        {
            return point.x >= xMin && point.x < xMax && point.y >= yMin && point.y < yMax;
        }

        public bool Contains(Vector3 point)
        {
            return point.x >= xMin && point.x < xMax && point.y >= yMin && point.y < yMax;
        }

        public bool Contains(Vector3 point, bool allowInverse)
        {
            if (!allowInverse)
            {
                return Contains(point);
            }
            var flag = (width < 0 && point.x <= xMin && point.x > xMax) ||
                       (width >= 0 && point.x >= xMin && point.x < xMax);
            return flag &&
                   ((height < 0f && point.y <= yMin && point.y > yMax) ||
                    (height >= 0 && point.y >= yMin && point.y < yMax));
        }

        private static RectInt OrderMinMax(RectInt rect)
        {
            if (rect.xMin > rect.xMax)
            {
                var xMin = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = xMin;
            }
            if (rect.yMin > rect.yMax)
            {
                var yMin = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = yMin;
            }
            return rect;
        }

        public bool Overlaps(RectInt other)
        {
            return other.xMax > xMin && other.xMin < xMax && other.yMax > yMin && other.yMin < yMax;
        }

        public bool Overlaps(RectInt other, bool allowInverse)
        {
            var rect = this;
            if (allowInverse)
            {
                rect = OrderMinMax(rect);
                other = OrderMinMax(other);
            }
            return rect.Overlaps(other);
        }

        public static Vector2 NormalizedToPoint(RectInt rectangle, Vector2 normalizedRectCoordinates)
        {
            return new Vector2(Mathf.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x),
                Mathf.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
        }

        public static Vector2 PointToNormalized(RectInt rectangle, Vector2 point)
        {
            return new Vector2(Mathf.InverseLerp(rectangle.x, rectangle.xMax, point.x),
                Mathf.InverseLerp(rectangle.y, rectangle.yMax, point.y));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ width.GetHashCode() << 2 ^ y.GetHashCode() >> 2 ^ height.GetHashCode() >> 1;
        }

        public override bool Equals(object other)
        {
            if (!(other is RectInt))
            {
                return false;
            }
            var rect = (RectInt) other;
            return x.Equals(rect.x) && y.Equals(rect.y) && width.Equals(rect.width) && height.Equals(rect.height);
        }

        public static bool operator !=(RectInt lhs, RectInt rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y || lhs.width != rhs.width || lhs.height != rhs.height;
        }

        public static bool operator ==(RectInt lhs, RectInt rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }
    }
}