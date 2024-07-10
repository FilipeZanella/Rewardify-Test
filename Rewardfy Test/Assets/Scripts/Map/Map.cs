using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Map : IMap
{
    public event Action<int, int> OnReset;

    protected ICell[,] map;
    public int Width { get => map.GetLength(0); }
    public int Height { get => map.GetLength(1); }

    public abstract Vector2 Size { get; }

    public ICell this[int x, int y]
    {
        get => map[x, y];
    }

    public Map (int width, int height) 
    {
        map = GetNewMap(width, height);
    } 

    public abstract Vector2 GetPositionByCoordinates(Vector2Int coordenate);

    public abstract ICell[,] GetNewMap(int width, int height);

    public void Reset(int x, int y)
    {
        map = GetNewMap(x, y);
        OnReset?.Invoke(x, y);
    }

    public bool IsInsideMap(int x, int y)
    {
        return x >= 0 && x < Width && y >= 0 && y < Height;
    }

    public bool IsObstacle(int x, int y)
    {
        return IsInsideMap(x, y) && map[x, y].Status == CellStatus.Obstacle;
    }

    public IEnumerator<ICell> GetEnumerator()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                yield return map[x, y];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
