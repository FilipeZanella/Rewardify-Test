using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap : IEnumerable<ICell>
{
    event Action<int, int> OnReset;
    ICell this[int row, int column] { get; }
    int Width {  get; }
    int Height { get; }
    Vector2 Size {  get; }
    bool IsInsideMap(int x, int y);
    bool IsObstacle(int x, int y);
    void Reset(int x, int y);
    Vector2 GetPositionByCoordinates(Vector2Int coordenate);
}

public interface ICell 
{
    event Action<CellStatus> OnChangeStatus;
    CellStatus Status { get; set; }
    Vector2Int Coordenate { get; }
    Vector2 Position { get; }
    bool IsRayIntersectingCollider(Ray ray);
    IEnumerable<ICell> GetNeighbors(IMap map);
}

public interface IPathFinder 
{
    IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map);
}