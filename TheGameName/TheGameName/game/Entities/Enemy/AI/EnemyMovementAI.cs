using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class EnemyMovementAI
{
    public EnemyMovementAI()
    {
        
    }

    public Tile FindPath(TileMap map, Vector2 startPosition, Vector2 targetPosition)
    {
        var notVisited = map.ToList().ToHashSet();
        var track = new Dictionary<Tile, DijkstraData>();
        var startTile = map.GetTileByVectorPosition(startPosition);
        var endTile = map.GetTileByVectorPosition(targetPosition);
        track[startTile] = new DijkstraData { Price = 0, Previous = null };
        while (true)
        {
            Tile toOpen = null;
            var bestPrice = double.PositiveInfinity;
            foreach (var tile in notVisited)
            {
                if (track.ContainsKey(tile) && track[tile].Price < bestPrice && tile.Collision == TileCollision.Walkable)
                {
                    bestPrice = track[tile].Price;
                    toOpen = tile;
                }
            }
            if (toOpen == null) return null;
            if (toOpen == endTile) break;
            foreach (var nextNode in map.GetTileNeighbours(toOpen))
            {
                var currentPrice = track[toOpen].Price + toOpen.Damage;
                if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
                {
                    track[nextNode] = new DijkstraData { Previous = toOpen, Price = currentPrice };
                }
            }
            notVisited.Remove(toOpen);
        }

        var result = new List<Tile>();
        while (endTile != null)
        {
            result.Add(endTile);
            endTile = track[endTile].Previous;
        }
        result.Reverse();
        if (result.Count == 1) return result[0];
        return result[1];
    }
}

class DijkstraData
{
    public Tile Previous { get; set; }
    public double Price { get; set; }
}