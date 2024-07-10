using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cell : ICell
{
    public event Action<CellStatus> OnChangeStatus;

    private Vector2Int coordenate;
    private Vector3 position;
    private CellStatus status;

    public CellStatus Status
    {
        get => status;
        set
        {
            if (value != status) 
            {
                status = value;
                OnChangeStatus?.Invoke(status);
            }
        }
    }
    public Vector2Int Coordenate => coordenate;
    public Vector2 Position => position;

    public Cell(IMap map, Vector2Int coordenate)
    {
        this.coordenate = coordenate;
        position = map.GetPositionByCoordinates(coordenate);
    }

    public abstract bool IsRayIntersectingCollider(Ray ray);

    public abstract IEnumerable<ICell> GetNeighbors(IMap map);
}
