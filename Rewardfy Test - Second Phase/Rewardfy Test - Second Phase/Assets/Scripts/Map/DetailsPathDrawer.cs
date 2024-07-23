using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetailPathDrawer : AStarPathFinder, IPathDrawer
{
    private bool isAnimating;
    public bool IsAnimating => isAnimating;

    private IPathDrawer sequence;
    private IAppController appController;
    private List<Node> affectedNodes = new List<Node>();
    MobileAppVisualDTO dto;

    public DetailPathDrawer(IAppController controller, IPathDrawer sequence) 
    {
        this.appController = controller;
        this.sequence = sequence;
    }

    protected override void StartSearch(ICell cellStart, ICell cellEnd, IMap map) 
    {
        base.StartSearch(cellStart, cellEnd, map);

        dto = new MobileAppVisualDTO(map);
    }

    private void Paint(ICell current) 
    {
        foreach (var cells in openList)
        {
            appController.PaintPathfinder(cells.Cell.Coordenate, CellTemplateType.PathFinder_Included);
            affectedNodes.Add(cells);
        }

        foreach (var node in current.GetNeighbors(map)) 
        {
            appController.PaintPathfinder(node.Coordenate, CellTemplateType.PathFinder_Around);
            affectedNodes.Add(nodeDict[node]);
        }

        foreach (var cells in closedSet) 
        {
            appController.PaintPathfinder(cells.Coordenate, CellTemplateType.PathFinder_Excluded);
            affectedNodes.Add(nodeDict[cells]);
        }

        appController.PaintPathfinder(current.Coordenate, CellTemplateType.PathFinder_Current);
        affectedNodes.Add(nodeDict[current]);
    }

    private void OnGUI() 
    {
        foreach (var node in affectedNodes)
        {
            var pos = dto.GetGUIPosition(node.Cell);
            Rect labelRect = new Rect(pos.x, pos.y, dto.multiplier, dto.multiplier);
            GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
            centeredStyle.alignment = TextAnchor.MiddleCenter;
            string text = "G: " + node.G + "\nH: " + node.H + "\nF: " + node.F;
            centeredStyle.fontSize = CalculateMaxFontSize(text, labelRect, centeredStyle);
            GUI.Label(labelRect, text, centeredStyle);
        }
    }

    int CalculateMaxFontSize(string text, Rect rect, GUIStyle style)
    {
        int minSize = 1;
        int maxSize = 1000; // Define um valor máximo razoável para o tamanho da fonte

        while (minSize <= maxSize)
        {
            int midSize = (minSize + maxSize) / 2;
            style.fontSize = midSize;

            // Calcula o tamanho do conteúdo com o tamanho da fonte atual
            Vector2 contentSize = style.CalcSize(new GUIContent(text));

            if (contentSize.x > rect.width*0.62f || contentSize.y > rect.height*0.62f)
            {
                maxSize = midSize - 1; // Tamanho muito grande
            }
            else
            {
                minSize = midSize + 1; // Tamanho muito pequeno
            }
        }

        return maxSize;
    }

    public IEnumerator Start(IList<ICell> path, IAppController controller)
    {
        sequence?.CleanPath();
        CleanPath();
        StartSearch(path[0], path[path.Count-1], map);
        Node currentNode;

        isAnimating = true;
        GUIController.instance.onGUI += OnGUI;

        while (openList.Any())
        {
            CleanPath();

            if (Iterate(out currentNode))
            {
                if (sequence != null)
                {
                    GUIController.instance.onGUI -= OnGUI;
                    isAnimating = false;
                    yield return sequence.Start(path, controller);
                }

                yield break;
            }
            
            Paint(currentNode.Cell);


            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            yield return null;
        }

        isAnimating = false;
        GUIController.instance.onGUI -= OnGUI;
    }

    public void CleanPath()
    {
        foreach (var cells in affectedNodes)
        {
            appController.FreeCell(cells.Cell.Coordenate);
        }

        affectedNodes.Clear();
    }
}
