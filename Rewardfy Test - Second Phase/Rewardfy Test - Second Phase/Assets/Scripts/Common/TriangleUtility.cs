using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class TriangleUtility
{
    // Check if point P(px, py) is inside the triangle ABC
    public static bool IsPointInTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
    {
        // Compute vectors        
        Vector2 v0 = C - A;
        Vector2 v1 = B - A;
        Vector2 v2 = P - A;

        // Compute dot products
        float dot00 = Vector2.Dot(v0, v0);
        float dot01 = Vector2.Dot(v0, v1);
        float dot02 = Vector2.Dot(v0, v2);
        float dot11 = Vector2.Dot(v1, v1);
        float dot12 = Vector2.Dot(v1, v2);

        // Compute barycentric coordinates
        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

        // Check if point is in triangle
        return (u >= 0) && (v >= 0) && (u + v < 1);
    }

    public static Vector3? GetRayIntersectionInTriangle(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 e1 = v1 - v0;
        Vector3 e2 = v2 - v0;
        Vector3 h = Vector3.Cross(ray.direction, e2);
        float a = Vector3.Dot(e1, h);

        if (a > -Mathf.Epsilon && a < Mathf.Epsilon)
            return null; // This ray is parallel to this triangle.

        float f = 1.0f / a;
        Vector3 s = ray.origin - v0;
        float u = f * Vector3.Dot(s, h);

        if (u < 0.0f || u > 1.0f)
            return null;

        Vector3 q = Vector3.Cross(s, e1);
        float v = f * Vector3.Dot(ray.direction, q);

        if (v < 0.0f || u + v > 1.0f)
            return null;

        float t = f * Vector3.Dot(e2, q);

        if (t > Mathf.Epsilon) // Ray intersection
        {
            return ray.origin + ray.direction * t;
        }

        return null; // No hit
    }

    public static bool RayIntersectsTriangle(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2)
    {
        Vector3 e1 = v1 - v0;
        Vector3 e2 = v2 - v0;
        Vector3 h = Vector3.Cross(ray.direction, e2);
        float a = Vector3.Dot(e1, h);
        if (a > -Mathf.Epsilon && a < Mathf.Epsilon)
            return false;

        float f = 1.0f / a;
        Vector3 s = ray.origin - v0;
        float u = f * Vector3.Dot(s, h);
        if (u < 0.0f || u > 1.0f)
            return false;

        Vector3 q = Vector3.Cross(s, e1);
        float v = f * Vector3.Dot(ray.direction, q);
        if (v < 0.0f || u + v > 1.0f)
            return false;

        float t = f * Vector3.Dot(e2, q);
        if (t > Mathf.Epsilon) // ray intersection
        {
            return true;
        }

        return false;
    }

    public static Vector3? RayIntersectsHexagonCell(HexagonCell cell, Ray ray)
    {
        ray.origin -= (Vector3)cell.Position;
        var vertices = cell.Shape.GetShapeVertices();

        // Check for intersection with each triangle of the hexagon
        for (int i = 0; i < 6; i++)
        {
            var _point = GetRayIntersectionInTriangle(ray, vertices[i], vertices[(i + 1) % 6], Vector3.zero);
            if (_point.HasValue)
            {
                return _point.Value;
            }
        }

        return null;
    }
}
