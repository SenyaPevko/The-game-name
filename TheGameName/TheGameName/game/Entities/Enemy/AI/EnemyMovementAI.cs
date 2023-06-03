using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TheGameName;

public class EnemyMovementAI
{
    public class Node: IComparable<Node>
    {
        public bool Walkable { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int GCost { get; private set; } // стоимость от начала к этой ноде
        public int HCost { get; private set; } // стоимость от этой ноды к концу, без учета непроходимости тайлов
        public int FCost { get { return GCost + HCost; } }
        public Node Parent { get; private set; }
        public Tile Tile { get; private set; }

        public Node(Tile tile, Node parent)
        {
            Tile = tile;
            Walkable = Tile.Collision == TileCollision.Walkable;
            X = TheGameName.TileMap.GetTileXIndex(Tile);
            Y = TheGameName.TileMap.GetTileYIndex(Tile);
            Parent = parent;
        }

        public void SetDistance(Node targetNode)
        {
            HCost = MathOperations.GetHeuristicDistance(targetNode, this);
            if (Parent != null)
                GCost = Parent.GCost + MathOperations.GetHeuristicDistance(Parent, this);
        }

        public void ChangeParent(Node parent)
        {
            Parent = parent;
        }

        public static bool operator ==(Node node1, Node node2)
        {
            if (node1 is null && node2 is null) return true;
            if (node1 is null || node2 is null) return false;
            return node1.X == node2.X && node1.Y == node2.Y;
        }
        public static bool operator !=(Node node1, Node node2)
        {
            if (node1 is null && node2 is null) return false;
            if (node1 is null || node2 is null) return true;
            return node1.X != node2.Y || node1.Y != node2.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Node) return false;
            var node = (Node)obj;
            return X == node.X && Y == node.Y;
        }

        public override int GetHashCode()
        {
            return (X * 269 + Y) * 191;
        }

        public int CompareTo(Node other)
        {
            return FCost - other.FCost;
        }
    }
    public Tile FindPath(Tile start, Tile target)
    {
        var startNode = new Node(start, null);
        var targetNode = new Node(target, null);
        startNode.SetDistance(targetNode);
        var openedNodes = new PriorityQueue<Node>();
        var closedNodes = new HashSet<Node>();
        openedNodes.Enqueue(startNode);

        while (openedNodes.Any())
        {
            // берем ноду с меньшим fcost
            var currentNode = openedNodes.Dequeue();
            closedNodes.Add(currentNode);

            if (currentNode == targetNode)
            {
                targetNode.ChangeParent(currentNode);
                var resultPath = ParseNodesIntoPath(startNode, targetNode);
                return resultPath.Any() ? resultPath.Last().Tile : null;
            }

            // обход соседей
            foreach (var tileNeighbor in TheGameName.TileMap.GetTileNeighbours(currentNode.Tile))
            {
                if (tileNeighbor == null) continue;
                var neighbor = new Node(tileNeighbor, currentNode);

                if (!neighbor.Walkable || closedNodes.Contains(neighbor))
                    continue;

                if (!openedNodes.Contains(neighbor))
                {
                    neighbor.SetDistance(targetNode);
                    openedNodes.Enqueue(neighbor);
                }
            }
        }
        // нет пути
        return null;
    }


    //путь от конца в начало
    public IEnumerable<Node> ParseNodesIntoPath(Node startNode, Node targetNode)
    {
        var currentNode = targetNode;
        while (currentNode != startNode)
        {
            if (currentNode.Parent == null)
                break;
            yield return currentNode;
            currentNode = currentNode.Parent;
        }
    }
}
