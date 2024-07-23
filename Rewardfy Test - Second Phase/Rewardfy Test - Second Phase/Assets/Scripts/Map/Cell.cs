using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cell : ICell
{
    public CellStatus Status { get; set; }
    public Vector2Int Coordenate { get; private set; }
    public Vector2 Position { get; private set; }

    public abstract ICellShape Shape { get; }

    public Cell(Vector2Int coordenate, Vector2 position)
    {
        Coordenate = coordenate;
        Position = position;
    }

    public abstract IEnumerable<ICell> GetNeighbors(IMap map);
}
