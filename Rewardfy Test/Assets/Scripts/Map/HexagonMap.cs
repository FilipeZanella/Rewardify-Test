using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonMap : Map
{
    public HexagonMap(int width, int height) : base(width, height) { }

    public override Vector2 Size => new Vector2((Width - 1) * 0.86f, (Height - 1) * 0.76f);

    public override ICell[,] GetNewMap(int width, int height)
    {
        var map = new HexagonCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = new HexagonCell(this, new Vector2Int(x, y), 0.5f);
            }
        }

        return map;
    }

    public override Vector2 GetPositionByCoordinates(Vector2Int coordenate)
    {
        return new Vector2(coordenate.x * 0.86f + (coordenate.y % 2 == 1 ? 0.43f : 0), coordenate.y * 0.76f);
    }
}
