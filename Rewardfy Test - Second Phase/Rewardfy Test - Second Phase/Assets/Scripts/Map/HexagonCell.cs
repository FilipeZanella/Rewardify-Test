using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class HexagonCell : Cell
{
    private static HexagonCellShape shape = new HexagonCellShape(0.5f);

    public HexagonCell(Vector2Int coordenate, Vector2 position) : base(coordenate, position) { }

    public override ICellShape Shape => shape;

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
}
