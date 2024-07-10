using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using System;

public class AStarPathFinder : IPathFinder
{
    public IList<ICell> FindPathOnMap(ICell cellStart, ICell cellEnd, IMap map)
    {
        var openList = new List<Node> { new Node(cellStart, null, 0, GetHeuristic(cellStart, cellEnd)) };
        var closedSet = new HashSet<ICell>();
        var nodeDict = new Dictionary<ICell, Node>
        {
            { cellStart, openList.First() }
        };

        while (openList.Any())
        {
            openList.Sort((a, b) => a.F.CompareTo(b.F));
            var currentNode = openList.First();
            if (currentNode.Cell == cellEnd)
            {
                return ReconstructPath(currentNode);
            }

            openList.Remove(currentNode);
            closedSet.Add(currentNode.Cell);

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

        return new List<ICell>(); // No path found
    }

    private int GetHeuristic(ICell cellStart, ICell cellEnd)
    {
        return Mathf.Abs(cellStart.Coordenate.x - cellEnd.Coordenate.x) + Mathf.Abs(cellStart.Coordenate.y - cellEnd.Coordenate.y);
    }

    private IList<ICell> ReconstructPath(Node node)
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

    private class Node
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

    private class NodeComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            int compare = x.F.CompareTo(y.F);
            return compare == 0 ? x.H.CompareTo(y.H) : compare;
        }
    }
}
