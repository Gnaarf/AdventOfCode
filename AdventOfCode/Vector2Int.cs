using System;
using System.Numerics;

public struct Vector2Int
{
    public int X;
    public int Y;

    public Vector2Int(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }


    //------------------------------------------//
    //                 Constants                //
    //------------------------------------------//
    /// <summary>(0, 0)</summary>
    public static Vector2Int Zero { get { return new Vector2Int(0, 0); } }
    /// <summary>(1, 1)</summary>
    public static Vector2Int One { get { return new Vector2Int(1, 1); } }
    /// <summary>(0, -1)</summary>
    public static Vector2Int Up { get { return new Vector2Int(0, -1); } }
    /// <summary>(1, 0)</summary>
    public static Vector2Int Right { get { return new Vector2Int(1, 0); } }
    /// <summary>(0, 1)</summary>
    public static Vector2Int Down { get { return new Vector2Int(0, 1); } }
    /// <summary>(-1, 0)</summary>
    public static Vector2Int Left { get { return new Vector2Int(-1, 0); } }

    //------------------------------------------//
    //             Instance Functions           //
    //------------------------------------------//
    public float Length { get { return (float)Math.Sqrt(X * X + Y * Y); } }
    public float LengthSqr { get { return X * X + Y * Y; } }

    public Vector2Int RightRotated { get { return new Vector2Int(Y, -X); } }
    public Vector2Int LeftRotated { get { return new Vector2Int(-Y, X); } }

    //------------------------------------------//
    //           Static Functions               //
    //------------------------------------------//
    /// <summary>linear interpolation by t=[0,1]</summary>
    public static Vector2Int Lerp(Vector2Int from, Vector2Int to, float t)
    {
        return (1 - t) * from + t * to;
    }

    public static Vector2Int Sum(params Vector2Int[] values)
    {
        Vector2Int sum = new Vector2Int(0, 0);
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }
        return sum;
    }

    public static float Distance(Vector2Int v1, Vector2Int v2)
    {
        return (v1 - v2).Length;
    }

    public static float DistanceSqr(Vector2Int v1, Vector2Int v2)
    {
        return (v1 - v2).LengthSqr;
    }

    public static float Dot(Vector2Int v1, Vector2Int v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y;
    }

    //------------------------------------------//
    //           Arithmetic Operators           //
    //------------------------------------------//
    // Addition
    /// <summary>add component-wise</summary>
    public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.X + v2.X, v1.Y + v2.Y);
    }

    // Subtraction
    /// <summary>subtract component-wise</summary>
    public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.X - v2.X, v1.Y - v2.Y);
    }
    /// <summary>negate every component</summary>
    public static Vector2Int operator -(Vector2Int v)
    {
        return new Vector2Int(-v.X, -v.Y);
    }

    // Multiplication
    /// <summary>multiply component-wise</summary>
    public static Vector2Int operator *(Vector2Int v1, Vector2Int v2)
    {
        return new Vector2Int(v1.X * v2.X, v1.Y * v2.Y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2Int operator *(float f, Vector2Int v)
    {
        return new Vector2Int((int)f * v.X, (int)f * v.Y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2Int operator *(Vector2Int v, float f)
    {
        return new Vector2Int((int)f * v.X, (int)f * v.Y);
    }

    // Division
    /// <summary>divide component-wise</summary>
    public static Vector2Int operator /(Vector2Int v1, Vector2Int v2)
    {
        if (v2.X == 0 || v2.Y == 0) { throw new Exception("Devide by Zero"); }
        return new Vector2Int(v1.X / v2.X, v1.Y / v2.Y);
    }
    /// <summary>divide both components by factor</summary>
    public static Vector2Int operator /(Vector2Int v, float f)
    {
        if (f == 0) { throw new Exception("Devide by Zero"); }
        return new Vector2Int(v.X / (int)f, v.Y / (int)f);
    }

    // Equality
    /// <summary>check component-wise</summary>
    public static bool operator ==(Vector2Int v1, Vector2Int v2)
    {
        return (v1.X == v2.X) && (v1.Y == v2.Y);
    }
    /// <summary>check component-wise</summary>
    public static bool operator !=(Vector2Int v1, Vector2Int v2)
    {
        return (v1.X != v2.X) || (v1.Y != v2.Y);
    }

    public override bool Equals(object obj)
    {
        if(obj is Vector2Int)
        {
            return (Vector2Int)obj == this;
        }
        return GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}