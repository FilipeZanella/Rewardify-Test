using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineController : MonoBehaviour
{
    LineRenderer lineRenderer;

    public bool IsAnimating { get; private set; } = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        UIManager.instance.GetPanel<HUD>().OnResetButtonClicked += () => lineRenderer.positionCount = 0;
        UIManager.instance.GetPanel<HUD>().OnSelectMapSize += (x, y) => lineRenderer.positionCount = 0;
    }

    public IEnumerator Animate(IList<ICell> path, Action<ICell> onGetToCell)
    {
        IsAnimating = true;

        var _path = new List<ICell>(path);
        Vector3 oldPosition;
        Vector3 newPosition;

        List<Vector3> positions = new List<Vector3>() { _path[0].Position };
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        onGetToCell?.Invoke(_path[0]);

        for (int i = 1; i < _path.Count; i++)
        {
            positions.Add(_path[i].Position);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());

            oldPosition = _path[i - 1].Position;
            newPosition = _path[i].Position;

            yield return LoopUtility.Tween((t) => lineRenderer.SetPosition(i, Vector2.Lerp(oldPosition, newPosition, t)), 0.09f);

            onGetToCell?.Invoke(_path[i]);
        }

        IsAnimating = false;
    }
}
