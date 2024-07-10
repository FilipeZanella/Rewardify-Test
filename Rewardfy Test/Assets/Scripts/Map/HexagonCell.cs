using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class HexagonCell : Cell
{
    private float size;

    public HexagonCell(IMap map, Vector2Int coordenate, float size) : base (map, coordenate)
    {
        this.size = size;
    }

    public override IEnumerable<ICell> GetNeighbors(IMap map)
    {
        int xx = Coordenate.y % 2 == 1 ? 1 : -1; 
        List<(int, int)> neighborOffsets = new List<(int, int)>
        {
            (1, 0), (-1, 0), (0, 1), (0, -1), (xx, -1), (xx, 1)
        };

        foreach (var (dx, dy) in neighborOffsets)
        {
            Vector2Int coo = new Vector2Int(Coordenate.x + dx, Coordenate.y + dy);

            if (map.IsInsideMap(coo.x, coo.y))
            {
                ICell neighbor = map[coo.x, coo.y];
                if (neighbor.Status != CellStatus.Obstacle)
                {
                    yield return neighbor;
                }
            }
        }
    }

    public override bool IsRayIntersectingCollider(Ray ray)
    {
        Vector3[] vertices = GetHexagonVertices(Position, Vector3.forward, size);

        // Check for intersection with each triangle of the hexagon
        for (int i = 0; i < 6; i++)
        {
            Vector3 v0 = vertices[i];
            Vector3 v1 = vertices[(i + 1) % 6];
            Vector3 v2 = Position;

            if (TriangleUtility.RayIntersectsTriangle(ray, v0, v1, v2))
            {
                return true;
            }
        }

        return false;
    }

    private Vector3[] GetHexagonVertices(Vector3 center, Vector3 normal, float size)
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
}
