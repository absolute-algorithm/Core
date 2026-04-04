using System.Numerics;

namespace AbsoluteAlgorithm.Core.Numerics;

/// <summary>
/// A high-performance, scale-based conversion engine for any physical or digital dimension.
/// </summary>
public static class Geometry
{
    #region Linear Interpolation & Basic Math

    /// <summary>
    /// Linearly interpolates between two values based on a weight.
    /// </summary>
    /// <param name="a">The start value.</param>
    /// <param name="b">The end value.</param>
    /// <param name="t">The interpolation weight (usually between 0 and 1).</param>
    public static T Lerp<T>(T a, T b, T t) where T : IFloatingPoint<T>
    {
        return a + (b - a) * t;
    }

    /// <summary>
    /// Linearly interpolates between two Vector3 positions.
    /// </summary>
    public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
    {
        return Vector3.Lerp(a, b, t);
    }

    /// <summary>
    /// Returns the aspect ratio for a given width and height.
    /// </summary>
    public static T AspectRatio<T>(T width, T height) where T : IFloatingPoint<T>
    {
        return width / height;
    }

    #endregion

    #region Area & Volume

    /// <summary>
    /// Calculates the area of a square. Works with any number type (int, float, etc.).
    /// </summary>
    public static T AreaOfSquare<T>(T side) where T : INumber<T>
    {
        return side * side;
    }

    /// <summary>
    /// Calculates the area of a rectangle.
    /// </summary>
    public static T AreaOfRectangle<T>(T length, T width) where T : INumber<T>
    {
        return length * width;
    }

    /// <summary>
    /// Calculates the area of a circle.
    /// </summary>
    public static T AreaOfCircle<T>(T radius) where T : IFloatingPoint<T>
    {
        return T.Pi * radius * radius;
    }

    /// <summary>
    /// Calculates the volume of a sphere. V = 4/3 * π * r³
    /// </summary>
    public static T VolumeOfSphere<T>(T radius) where T : IFloatingPoint<T>
    {
        T fourThirds = T.CreateChecked(4.0 / 3.0);
        return fourThirds * T.Pi * radius * radius * radius;
    }

    /// <summary>
    /// Calculates the volume of a cylinder. V = π * r² * h
    /// </summary>
    public static T VolumeOfCylinder<T>(T radius, T height) where T : IFloatingPoint<T>
    {
        return T.Pi * radius * radius * height;
    }

    #endregion

    #region Distance & Range

    /// <summary>
    /// Calculates the Euclidean distance between two 2D points generically.
    /// </summary>
    public static T Distance<T>(T x1, T y1, T x2, T y2) where T : IFloatingPoint<T>, IRootFunctions<T>
    {
        T dx = x2 - x1;
        T dy = y2 - y1;
        return T.Sqrt((dx * dx) + (dy * dy));
    }

    /// <summary>
    /// Calculates the Euclidean distance between two points in 3D space generically.
    /// </summary>
    public static T Distance3D<T>(T x1, T y1, T z1, T x2, T y2, T z2) where T : IFloatingPoint<T>, IRootFunctions<T>
    {
        T dx = x2 - x1;
        T dy = y2 - y1;
        T dz = z2 - z1;
        return T.Sqrt((dx * dx) + (dy * dy) + (dz * dz));
    }

    /// <summary>
    /// Optimized range check. Avoids expensive Square Root calculations.
    /// Use this for performance-critical proximity checks.
    /// </summary>
    public static T DistanceSquared<T>(T x1, T y1, T x2, T y2) where T : INumber<T>
    {
        T dx = x2 - x1;
        T dy = y2 - y1;
        return (dx * dx) + (dy * dy);
    }

    /// <summary>
    /// Calculates the distance between two Vector2 points using SIMD optimization.
    /// </summary>
    public static float Distance(Vector2 start, Vector2 end) => Vector2.Distance(start, end);

    /// <summary>
    /// Calculates the distance between two Vector3 points using SIMD optimization.
    /// </summary>
    public static float Distance(Vector3 start, Vector3 end) => Vector3.Distance(start, end);

    /// <summary>
    /// Finds the midpoint between two coordinates.
    /// </summary>
    public static (T x, T y) Midpoint<T>(T x1, T y1, T x2, T y2) where T : INumber<T>
    {
        T two = T.CreateChecked(2);
        return ((x1 + x2) / two, (y1 + y2) / two);
    }

    #endregion

    #region Intersection & Collision

    /// <summary>
    /// Checks if a point is inside a 2D Axis-Aligned Bounding Box (AABB).
    /// </summary>
    public static bool IsPointInRect<T>(T px, T py, T rx, T ry, T rw, T rh) where T : INumber<T>
    {
        return px >= rx && px <= rx + rw &&
               py >= ry && py <= ry + rh;
    }

    /// <summary>
    /// Checks if a Vector2 point is within a rectangular bound.
    /// </summary>
    public static bool IsPointInRect(Vector2 point, Vector2 rectPos, Vector2 rectSize)
    {
        return point.X >= rectPos.X && point.X <= rectPos.X + rectSize.X &&
               point.Y >= rectPos.Y && point.Y <= rectPos.Y + rectSize.Y;
    }

    /// <summary>
    /// Checks if two circles are overlapping.
    /// </summary>
    public static bool AreCirclesOverlapping<T>(T x1, T y1, T r1, T x2, T y2, T r2)
        where T : IFloatingPoint<T>, IRootFunctions<T>
    {
        T dx = x2 - x1;
        T dy = y2 - y1;
        T distanceSquared = (dx * dx) + (dy * dy);
        T radiusSum = r1 + r2;
        return distanceSquared <= (radiusSum * radiusSum);
    }

    #endregion

    #region Direction & Rotation

    /// <summary>
    /// Calculates the normalized 2D direction vector from a source to a target.
    /// </summary>
    public static Vector2 Direction(Vector2 from, Vector2 to) => Vector2.Normalize(to - from);

    /// <summary>
    /// Calculates the normalized 3D direction vector from a source to a target.
    /// </summary>
    public static Vector3 Direction(Vector3 from, Vector3 to) => Vector3.Normalize(to - from);

    /// <summary>
    /// Calculates the angle in radians between two vectors.
    /// </summary>
    public static float AngleBetween(Vector2 from, Vector2 to)
    {
        float dot = Vector2.Dot(Vector2.Normalize(from), Vector2.Normalize(to));
        return MathF.Acos(Math.Clamp(dot, -1.0f, 1.0f));
    }

    /// <summary>
    /// Converts a direction vector into an angle in radians (Clockwise from Up).
    /// </summary>
    public static float VectorToAngle(Vector2 direction) => MathF.Atan2(direction.X, -direction.Y);

    /// <summary>
    /// Converts an angle in radians into a 2D unit direction vector.
    /// </summary>
    public static Vector2 AngleToVector(float radians) => new Vector2(MathF.Sin(radians), -MathF.Cos(radians));

    /// <summary>
    /// Calculates the rotation (Quaternion) required for a source to look at a target.
    /// </summary>
    public static Quaternion LookAtRotation(Vector3 source, Vector3 target, Vector3 up)
    {
        Vector3 forward = Vector3.Normalize(target - source);
        if (MathF.Abs(Vector3.Dot(forward, up)) > 0.99f) return Quaternion.Identity;
        return Quaternion.CreateFromRotationMatrix(Matrix4x4.CreateLookAt(source, target, up));
    }

    #endregion
}