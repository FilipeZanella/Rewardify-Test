using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;

public class AStarPathFinder : IPathFinder
{
    protected IMap map;

    protected List<Node> openList;
    protected HashSet<ICell> closedSet;
    protected Dictionary<ICell, Node> nodeDict;

    protected ICell cellEnd;
    protected ICell cellStart;

    protected virtual void StartSearch(ICell cellStart, ICell cellEnd, IMap map)
    {
        this.map = map;
        this.cellStart = cellStart;
        this.cellEnd = cellEnd;

        openList = new List<Node> { new Node(cellStart, null, 0, GetHeuristic(cellStart, cellEnd)) };
        closedSet = new HashSet<ICell>();
        nodeDict = new Dictionary<ICell, Node>
        {
            { cellStart, openList.First() }
        };
    }

    protected bool Iterate(out Node currentNode)
    {
        openList.Sort((a, b) => a.F.CompareTo(b.F));
        currentNode = openList.First();
        if (currentNode.Cell == cellEnd)
        {
            return true;
        }

        openList.Remove(currentNode);
        closedSet.Add(currentNode.Cell);
        CheckNeighbors(currentNode);

        return false;
    }

    protected void CheckNeighbors(Node currentNode) 
    {
        foreach (var neighbor in currentNode.Cell.GetNeighbors(map))
        {
            if (closedSet.Contains(neighbor))
            {
                continue;
            }

            var tentativeGScore = currentNode.G + 1;

            if (!nodeDict.TryGetValue(neighbor, out var neighborNode))
            {
                neighborNode = new Node(neighbor, currentNode, tentativeGScore, GetHeuristic(neighbor, cellEnd));
                nodeDict[neighbor] = neighborNode;
                openList.Add(neighborNode);
            }
            else if (tentativeGScore < neighborNode.G)
            {
                openList.Remove(neighborNode);
                neighborNode.Parent = currentNode;
                neighborNode.G = tentativeGScore;
                neighborNode.F = tentativeGScore + neighborNode.H;
                //openList.Add(neighborNode);
            }
        }
    }

    public IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
    {
        StartSearch(cellStart, cellEnd, map);
        Node currentNode;

        while (openList.Any())
        {
            if (Iterate(out currentNode)) 
            {
                return ReconstructPath(currentNode);
            }
        }

        return new List<ICell>(); // No path found
    }

    private int GetHeuristic(ICell cellStart, ICell cellEnd)
    {
        return Mathf.Abs(cellStart.Coordenate.x - cellEnd.Coordenate.x) + Mathf.Abs(cellStart.Coordenate.y - cellEnd.Coordenate.y);
    }

    protected IList<ICell> ReconstructPath(Node node)
    {
        var path = new List<ICell>();
        while (node != null)
        {
            path.Add(node.Cell);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }
}

public class Node
{
    public ICell Cell { get; }
    public Node Parent { get; set; }
    public int G { get; set; } // Cost from start to this node
    public int H { get; } // Heuristic cost from this node to end
    public int F { get; set; } // G + H

    public Node(ICell cell, Node parent, int g, int h)
    {
        Cell = cell;
        Parent = parent;
        G = g;
        H = h;
        F = g + h;
    }
}
