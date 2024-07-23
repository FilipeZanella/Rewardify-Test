using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LinePathDrawer : IPathDrawer
{
    private LineRenderer line;
    private IList<ICell> path;
    private IAppController appController;
    private Func<ICell, Vector2> positionHandler;

    private bool isAnimating;
    public bool IsAnimating => isAnimating;

    public LinePathDrawer(LineRenderer line, Func<ICell, Vector2> positionHandler = null)
    {
        this.line = line;
        this.positionHandler = positionHandler;

        UIManager.instance.GetPanel<HUD>().OnResetButtonClicked += () => Reset();
        UIManager.instance.GetPanel<HUD>().OnSelectMapSize += (x, y) => Reset();
    }

    private void Reset()
    {
        line.positionCount = 0;
        path = null;
    }

    public IEnumerator Start(IList<ICell> path, IAppController controller)
    {
        CleanPath();

        isAnimating = true;

        this.appController = controller;
        this.path = path;
        var _path = new List<ICell>(path);
        Vector3 oldPosition;
        Vector3 newPosition;

        List<Vector3> positions = new List<Vector3>() { positionHandler?.Invoke(_path[0]) ?? path[0].Position };
        line.positionCount = positions.Count;
        line.SetPositions(positions.ToArray());

        controller.SelectPathCell(_path[0].Coordenate);

        for (int i = 1; i < _path.Count; i++)
        {
            positions.Add(positionHandler?.Invoke(_path[i]) ?? path[i].Position);
            line.positionCount = positions.Count;
            line.SetPositions(positions.ToArray());

            oldPosition = positions[i - 1];
            newPosition = positions[i];

            yield return LoopUtility.Tween((t) => line.SetPosition(i, Vector2.Lerp(oldPosition, newPosition, t)), 0.1f);

            controller.SelectPathCell(_path[i].Coordenate);
        }

        isAnimating = false;
    }

    public void CleanPath()
    {
        if (path != null && path.Count != 0)
        {
            foreach (var cell in path)
            {
                if (cell.Status != CellStatus.Obstacle)
                {
                    appController.FreeCell(cell.Coordenate);
                }
            }
        }

        Reset();
    }
}

