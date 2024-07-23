using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMap : IEnumerable<ICell>
{
    event Action<int, int> OnReset;
    ICell this[int row, int column] { get; }
    ICell CreateCell(Vector2Int index);
    Vector2 Size { get; }
    int Width { get; }
    int Height { get; }
    bool IsInsideMap(int x, int y);
    bool IsObstacle(int x, int y);
    void Reset(int x, int y);
    Vector2 GetPositionByCoordinates(Vector2Int coordenate);
}


public interface ICell
{
    CellStatus Status { get; set; }
    Vector2Int Coordenate { get; }
    Vector2 Position { get; }
    IEnumerable<ICell> GetNeighbors(IMap map);
    ICellShape Shape { get; }
}

public interface IPathDrawer
{
    bool IsAnimating { get; }
    void CleanPath();
    IEnumerator Start(IList<ICell> path, IAppController controller);
}

public interface ICellShape 
{
    Vector3[] GetShapeVertices();
    bool IsPointInside(ICell cell, Vector2 point);
}

public interface IMapInputHandler
{
    ICell GetSelectedCell();
}

public interface IAppController
{
    void AddObstacle(Vector2Int index);
    void RemoveObstacle(Vector2Int index);
    void FreeCell(Vector2Int index);
    void SelectCell(Vector2Int index);
    void SelectPathCell(Vector2Int index);
    void PaintPathfinder(Vector2Int index, CellTemplateType type);
}

public interface IAppInteractionHandler
{
    IEnumerator Behaviour(IAppController controller, IMapInputHandler input);
}

public interface IPathFinder
{
    IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map);
}

public interface IMapDrawer
{
    void StartBehaviour(IMap map);
    IEnumerator ChangeCellColor(Vector2Int coordenate, CellTemplateType template);
}
