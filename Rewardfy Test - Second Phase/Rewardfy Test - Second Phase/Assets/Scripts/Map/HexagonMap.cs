using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexagonMap : Map
{
    public HexagonMap(int width, int height) : base(width, height) { }

    public override Vector2 Size => new Vector2((Width) * 0.86f, (Height) * 0.76f);

    public override ICell CreateCell(Vector2Int index)
    {
        return new HexagonCell(index, GetPositionByCoordinates(index));
    }

    public override Vector2 GetPositionByCoordinates(Vector2Int coordenate)
    {
        return new Vector2(coordenate.x * 0.86f + (coordenate.y % 2 == 1 ? 0.43f : 0), coordenate.y * 0.76f);
    }
}
