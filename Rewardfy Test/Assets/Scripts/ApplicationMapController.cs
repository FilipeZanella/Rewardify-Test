using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplicationMapController : MonoBehaviour
{
    private IMap map;
    private bool isInputWorking = false;
    private Vector2Int? initialValue;

    private MapRaycaster raycaster;
    private MapDrawer mapDrawer;

    private Coroutine drawerCoroutine;
    private Coroutine raycasterCoroutine;
    private Coroutine lineEffectCorroutine;

    IPathFinder pathFinder;
    IList<ICell> path;
    [SerializeField] private LineController line;

    private void Start()
    {
        UIManager.instance.GetPanel<HUD>().OnResetButtonClicked += () =>
        {
            initialValue = null;
            path = null;
        };
        UIManager.instance.GetPanel<HUD>().OnSelectMapSize += (x, y) =>
        {
            path = null;
            initialValue = null;
            isInputWorking = true;
        };
    }

    public void MyStart(IMap map, MapDrawer drawer, MapRaycaster raycaster)
    {
        this.map = map;
        this.raycaster = raycaster;
        this.mapDrawer = drawer;

        if (drawerCoroutine != null)
        {
            StopCoroutine(drawerCoroutine);
            StopCoroutine(raycasterCoroutine);
        }

        pathFinder = new AStarPathFinder();
        drawerCoroutine = StartCoroutine(LoopUtility.Loop(mapDrawer.DrawMap));
        raycasterCoroutine = StartCoroutine(raycaster.CheckForClick(OnTileClicked));
    }

    private void OnTileClicked(Vector2Int tile, int mouseButtonIndex)
    {
        if (!isInputWorking)
            return;

        //right mouse button
        if (mouseButtonIndex == 1)
        {
            map[tile.x, tile.y].Status = map[tile.x, tile.y].Status == CellStatus.Empty ? CellStatus.Obstacle : CellStatus.Empty;
        }
        //left mouse button
        else if (mouseButtonIndex == 0 && !map.IsObstacle(tile.x, tile.y))
        {
            if (initialValue != null)
            {
                //whether the target cell is not the initial one
                if (initialValue.Value.x != tile.x || initialValue.Value.y != tile.y)
                {
                    if (!line.IsAnimating)
                    {
                        //paint path back to default
                        if (path != null && path.Count != 0)
                        {
                            foreach (var cell in path)
                            {
                                if (cell.Status != CellStatus.Obstacle)
                                {
                                    mapDrawer.ChangeTileTemplate(cell.Coordenate, VisualTemplateType.Default);
                                }
                            }
                        }

                        path = pathFinder.FindPathOnMap(map[initialValue.Value.x, initialValue.Value.y], map[tile.x, tile.y], map);
                        
                        var ini = map[initialValue.Value.x, initialValue.Value.y].Coordenate;
                        var fin = map[tile.x, tile.y].Coordenate;
                        UIManager.instance.GetPanel<HUD>().Print("Initial Cell: " + ini.x + ", " + ini.y + "    Target: " + fin.x + ", " + fin.y + "    Path Distance: " + path.Count);
                        if (path.Count != 0)
                        {
                            //animate line toward path
                            lineEffectCorroutine = StartCoroutine(line.Animate(path, (cell) => StartCoroutine(mapDrawer.ChangeColorSmoothly(cell.Coordenate, VisualTemplateType.Path_Reached))));
                        }

                        initialValue = tile;
                    }
                }
            }
            else
            {
                initialValue = tile;
                UIManager.instance.GetPanel<HUD>().Print("Initial Cell: " + initialValue.Value.x + ", " + initialValue.Value.y);
                mapDrawer.ChangeTileTemplate(tile, VisualTemplateType.Initial);
            }
        }
    }
}