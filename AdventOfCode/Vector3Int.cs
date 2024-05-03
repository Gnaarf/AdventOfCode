using System;
using System.Numerics;

public struct Vector3Int
{
    public int X;
    public int Y;
    public int Z;

    public Vector3Int(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return "(" + X + ", " + Y + ", " + Z + ")";
    }


    //------------------------------------------//
    //                 Constants                //
    //------------------------------------------//
    /// <summary>(0, 0, 0)</summary>
    public static Vector3Int Zero { get { return new Vector3Int(0, 0, 0); } }
    /// <summary>(1, 1, 1)</summary>
    public static Vector3Int One { get { return new Vector3Int(1, 1, 1); } }
    /// <summary>(0, 0, 1)</summary>
    public static Vector3Int Up { get { return new Vector3Int(0, 0, 1); } }
    /// <summary>(0, 0, -1)</summary>
    public static Vector3Int Down { get { return new Vector3Int(0, 0, -1); } }
    /// <summary>(1, 0, 0)</summary>
    public static Vector3Int Right { get { return new Vector3Int(1, 0, 0); } }
    /// <summary>(-1, 0, 0)</summary>
    public static Vector3Int Left { get { return new Vector3Int(-1, 0, 0); } }
    /// <summary>(0, 1, 0)</summary>
    public static Vector3Int Forward { get { return new Vector3Int(0, 1, 0); } }
    /// <summary>(0, -1, 0)</summary>
    public static Vector3Int Backward { get { return new Vector3Int(0, -1, 0); } }

    //------------------------------------------//
    //             Instance Functions           //
    //------------------------------------------//
    public float Length { get { return (float)Math.Sqrt(X * X + Y * Y + Z + Z); } }
    public float LengthSqr { get { return X * X + Y * Y + Z * Z; } }

    //------------------------------------------//
    //           Static Functions               //
    //------------------------------------------//
    /// <summary>linear interpolation by t=[0,1]</summary>
    public static Vector3Int Lerp(Vector3Int from, Vector3Int to, float t)
    {
        return (1 - t) * from + t * to;
    }

    public static Vector3Int Sum(params Vector3Int[] values)
    {
        Vector3Int sum = new Vector3Int(0, 0, 0);
        for (int i = 0; i < values.Length; i++)
        {
            sum += values[i];
        }
        return sum;
    }

    public static float Distance(Vector3Int v1, Vector3Int v2)
    {
        return (v1 - v2).Length;
    }

    public static float DistanceSqr(Vector3Int v1, Vector3Int v2)
    {
        return (v1 - v2).LengthSqr;
    }

    public static float Dot(Vector3Int v1, Vector3Int v2)
    {
        return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
    }

    //------------------------------------------//
    //           Arithmetic Operators           //
    //------------------------------------------//
    // Addition
    /// <summary>add component-wise</summary>
    public static Vector3Int operator +(Vector3Int v1, Vector3Int v2)
    {
        return new Vector3Int(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }

    // Subtraction
    /// <summary>subtract component-wise</summary>
    public static Vector3Int operator -(Vector3Int v1, Vector3Int v2)
    {
        return new Vector3Int(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }
    /// <summary>negate every component</summary>
    public static Vector3Int operator -(Vector3Int v)
    {
        return new Vector3Int(-v.X, -v.Y, -v.Z);
    }

    // Multiplication
    /// <summary>multiply component-wise</summary>
    public static Vector3Int operator *(Vector3Int v1, Vector3Int v2)
    {
        return new Vector3Int(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
    }
    /// <summary>multiply components with factor</summary>
    public static Vector3Int operator *(float f, Vector3Int v)
    {
        return new Vector3Int((int)f * v.X, (int)f * v.Y, (int)f * v.Z);
    }
    /// <summary>multiply components with factor</summary>
    public static Vector3Int operator *(Vector3Int v, float f)
    {
        return new Vector3Int((int)f * v.X, (int)f * v.Y, (int)f * v.Z);
    }

    // Division
    /// <summary>divide component-wise</summary>
    public static Vector3Int operator /(Vector3Int v1, Vector3Int v2)
    {
        if (v2.X == 0 || v2.Y == 0) { throw new Exception("Devide by Zero"); }
        return new Vector3Int(v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
    }
    /// <summary>divide components by factor</summary>
    public static Vector3Int operator /(Vector3Int v, float f)
    {
        if (f == 0) { throw new Exception("Devide by Zero"); }
        return new Vector3Int(v.X / (int)f, v.Y / (int)f, v.Z / (int)f);
    }

    // Equality
    /// <summary>check component-wise</summary>
    public static bool operator ==(Vector3Int v1, Vector3Int v2)
    {
        return (v1.X == v2.X) && (v1.Y == v2.Y) && (v1.Z == v2.Z);
    }
    /// <summary>check component-wise</summary>
    public static bool operator !=(Vector3Int v1, Vector3Int v2)
    {
        return (v1.X != v2.X) || (v1.Y != v2.Y) || (v1.Z != v2.Z);
    }

    public override bool Equals(object obj)
    {
        if(obj is Vector3Int)
        {
            return (Vector3Int)obj == this;
        }
        return GetHashCode() == obj.GetHashCode();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}