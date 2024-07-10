using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TriangleUtility 
{
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
}
