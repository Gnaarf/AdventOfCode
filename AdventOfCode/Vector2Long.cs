using System;
using System.Numerics;

public struct Vector2Long
{
    public long X;
    public long Y;

    public Vector2Long(long x, long y)
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
    public static Vector2Long Zero { get { return new Vector2Long(0, 0); } }
    /// <summary>(1, 1)</summary>
    public static Vector2Long One { get { return new Vector2Long(1, 1); } }
    /// <summary>(0, -1)</summary>
    public static Vector2Long Up { get { return new Vector2Long(0, -1); } }
    /// <summary>(1, 0)</summary>
    public static Vector2Long Right { get { return new Vector2Long(1, 0); } }
    /// <summary>(0, 1)</summary>
    public static Vector2Long Down { get { return new Vector2Long(0, 1); } }
    /// <summary>(-1, 0)</summary>
    public static Vector2Long Left { get { return new Vector2Long(-1, 0); } }

    //------------------------------------------//
    //             Instance Functions           //
    //------------------------------------------//
    public float Length { get { return (float)Math.Sqrt(X * X + Y * Y); } }
    public float LengthSqr { get { return X * X + Y * Y; } }

    public Vector2Long RightRotated { get { return new Vector2Long(Y, -X); } }
    public Vector2Long LeftRotated { get { return new Vector2Long(-Y, X); } }

    //------------------------------------------//
    //           Static Functions               //
    //------------------------------------------//
    /// <summary>linear interpolation by t=[0,1]</summary>
    public static Vector2Long Lerp(Vector2Long from, Vector2Long to, float t)
    {
        return (1 - t) * from + t * to;
    }

    public static Vector2Long Sum(params Vector2Long[] values)
    {
        Vector2Long sum = new Vector2Long(0, 0);
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }
        return sum;
    }

    public static float Distance(Vector2Long v1, Vector2Long v2)
    {
        return (v1 - v2).Length;
    }

    public static float DistanceSqr(Vector2Long v1, Vector2Long v2)
    {
        return (v1 - v2).LengthSqr;
    }

    public static float Dot(Vector2Long v1, Vector2Long v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y;
    }

    //------------------------------------------//
    //           Arithmetic Operators           //
    //------------------------------------------//
    // Addition
    /// <summary>add component-wise</summary>
    public static Vector2Long operator +(Vector2Long v1, Vector2Long v2)
    {
        return new Vector2Long(v1.X + v2.X, v1.Y + v2.Y);
    }

    // Subtraction
    /// <summary>subtract component-wise</summary>
    public static Vector2Long operator -(Vector2Long v1, Vector2Long v2)
    {
        return new Vector2Long(v1.X - v2.X, v1.Y - v2.Y);
    }
    /// <summary>negate every component</summary>
    public static Vector2Long operator -(Vector2Long v)
    {
        return new Vector2Long(-v.X, -v.Y);
    }

    // Multiplication
    /// <summary>multiply component-wise</summary>
    public static Vector2Long operator *(Vector2Long v1, Vector2Long v2)
    {
        return new Vector2Long(v1.X * v2.X, v1.Y * v2.Y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2Long operator *(float f, Vector2Long v)
    {
        return new Vector2Long((long)f * v.X, (long)f * v.Y);
    }
    /// <summary>multiply both components with factor</summary>
    public static Vector2Long operator *(Vector2Long v, float f)
    {
        return new Vector2Long((long)f * v.X, (long)f * v.Y);
    }

    // Division
    /// <summary>divide component-wise</summary>
    public static Vector2Long operator /(Vector2Long v1, Vector2Long v2)
    {
        if (v2.X == 0 || v2.Y == 0) { throw new Exception("Devide by Zero"); }
        return new Vector2Long(v1.X / v2.X, v1.Y / v2.Y);
    }
    /// <summary>divide both components by factor</summary>
    public static Vector2Long operator /(Vector2Long v, float f)
    {
        if (f == 0) { throw new Exception("Devide by Zero"); }
        return new Vector2Long(v.X / (long)f, v.Y / (long)f);
    }

    // Equality
    /// <summary>check component-wise</summary>
    public static bool operator ==(Vector2Long v1, Vector2Long v2)
    {
        return (v1.X == v2.X) && (v1.Y == v2.Y);
    }
    /// <summary>check component-wise</summary>
    public static bool operator !=(Vector2Long v1, Vector2Long v2)
    {
        return (v1.X != v2.X) || (v1.Y != v2.Y);
    }

    public override bool Equals(object obj)
    {
        if(obj is Vector2Long)
        {
            return (Vector2Long)obj == this;
        }
        return GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}