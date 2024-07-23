using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRaycaster : IMapInputHandler
{
    private IMap map;
    private Camera camera;

    public MapRaycaster(Camera camera, IMap map)
    {
        this.camera = camera;
        this.map = map;
    }

    public ICell GetSelectedCell()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                var cell = map[x, y];
                var point = TriangleUtility.RayIntersectsHexagonCell(cell as HexagonCell, ray);
                if (point.HasValue && cell.Shape.IsPointInside(cell, point.Value))
                {
                    return map[x,y];
                }
            }
        }

        return null;
    }
}