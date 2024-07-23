using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class HexagonCellShape : ICellShape
{
    private Vector3[] storedVertices;

    public HexagonCellShape(float size)
    {
        storedVertices = GetHexagonVertices(Vector3.zero, Vector3.forward, size);
    }

    public Vector3[] GetShapeVertices()
    {
        return storedVertices;
    }

    private static Vector3[] GetHexagonVertices(Vector3 center, Vector3 normal, float size)
    {
        Vector3[] vertices = new Vector3[6];
        float angle = 0;

        for (int i = 0; i < 6; i++)
        {
            vertices[i] = center + Quaternion.AngleAxis(angle, normal) * (Vector3.right * size);
            angle += 60f;  // 60 degrees for each vertex of the hexagon
        }

        return vertices;
    }

    public bool IsPointInside(ICell cell, Vector2 point)
    {
        point -= cell.Position;

        for (int i = 0; i < 6; i++)
        {
            if (TriangleUtility.IsPointInTriangle(storedVertices[i], storedVertices[(i + 1) % 6], Vector3.zero, point))
            {
                return true;
            }
        }

        return false;
    }
}
