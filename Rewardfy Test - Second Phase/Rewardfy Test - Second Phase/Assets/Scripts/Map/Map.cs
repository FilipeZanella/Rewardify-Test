using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Map : IMap
{
    public event Action<int, int> OnReset;

    protected ICell[,] map;
    
    public abstract Vector2 Size { get; }
    public int Width { get => map.GetLength(0); }
    public int Height { get => map.GetLength(1); }
    public ICell this[int x, int y]
    {
        get => map[x, y];
    }

    public Map(int width, int height)
    {
        GetNewMap(width, height);
    }

    public abstract ICell CreateCell(Vector2Int index);
    public abstract Vector2 GetPositionByCoordinates(Vector2Int coordenate);

    public void GetNewMap(int width, int height)
    {
        map = new HexagonCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = CreateCell(new Vector2Int(x, y));
            }
        }
    }

    public void Reset(int x, int y)
    {
        GetNewMap(x, y);
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
