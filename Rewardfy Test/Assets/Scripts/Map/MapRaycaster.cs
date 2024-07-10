using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRaycaster
{
    public event Action OnClick;

    private IMap map;
    private Camera camera;

    public MapRaycaster(Camera camera, IMap map)
    {
        this.camera = camera;
        this.map = map;
    }

    public IEnumerator CheckForClick(Action<Vector2Int, int> clickEvent) 
    {
        yield return LoopUtility.Loop(() => 
        {
            for (int i = 0; i < 2; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    var tile = NodeClicked(ray);

                    if (tile != null)
                    {
                        clickEvent(tile.Value, i);
                    }
                }
            }
        });
    }

    private Vector2Int? NodeClicked(Ray ray) 
    {
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (map[x,y].IsRayIntersectingCollider(ray)) 
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
}