﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System.IO;
using TheGameName;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;


public class TileMap
{
    private Rectangle[] tilesBoundsInTileset;
    private int mapWidth;
    private int mapHeight;
    private Texture2D tilesetTexture;
    public Tile[,] Map { get; private set; }
    public int TileWidth { get; private set; }
    public int TileHeight { get; private set; }
    private string[] data;
    private Dictionary<int, TileCollision> TileCollisionByIndex = new Dictionary<int, TileCollision>() 
    {
        {1, TileCollision.Walkable },
        {2, TileCollision.Walkable },
        {3, TileCollision.Impassable },
        {4, TileCollision.Passable }
    };

    public TileMap(string data, ContentManager contentManager)
    {
        this.data = data.Trim().Split("\n");
        tilesetTexture = contentManager.Load<Texture2D>($"Tiles/{this.data[0].Trim()}");
    }

    public void BuildMap()
    {
        //tiles size
        //вообще размеры нужно доставать из файла, но у меня выдает ошибку парсинга, потом нужно исправить
        TileWidth = 64;
        TileHeight = 64;
        ParseTileset();

        mapWidth = int.Parse(data[2].Split(", ")[0]);
        mapHeight = int.Parse(data[2].Split(", ")[1]);
        FillMap();
    }

    private void ParseTileset()
    {
        var tilesetColumns = tilesetTexture.Width / TileWidth;
        var tilesetRows = tilesetTexture.Height / TileHeight;
        tilesBoundsInTileset = new Rectangle[tilesetColumns * tilesetRows];
        for (int column = 0; column < tilesetColumns; column++)
        {
            for (int row = 0; row < tilesetRows; row++)
            {
                var index = column * tilesetColumns + row;
                var tileRectangle = new Rectangle(row * TileWidth, column * TileHeight, TileWidth, TileHeight);
                tilesBoundsInTileset[index] = tileRectangle;
            }
        }
    }

    public void FillMap()
    {
        Map = new Tile[mapWidth, mapHeight];
        for (int column = 0; column < mapWidth; column++)
        {
            var tileIndexes = data[3 + column].Trim().Split(", ");
            for (int row = 0; row < mapHeight; row++)
            {
                var tileIndex = int.Parse(tileIndexes[row]);
                var tilePosition = new Vector2(row * TileWidth, column * TileHeight);
                var tileRectangleInTileset = tilesBoundsInTileset[tileIndex - 1];
                var tileRectangeOnMap = new Rectangle((int)tilePosition.X, (int)tilePosition.Y, TileWidth, TileHeight);
                var tile = new Tile(tilesetTexture, TileCollisionByIndex[tileIndex],
                    tileRectangeOnMap, tilePosition, tileRectangleInTileset);
                Map[column, row] = tile;
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        for (int column = 0; column < mapWidth; column++)
        {
            for (int row = 0; row < mapHeight; row++)
            {
                Map[column, row].Draw(spriteBatch, gameTime);
            }
        }
    }

    public IEnumerator GetEnumerator()
    {
        foreach(var tile in Map) yield return tile;
    }

    public Tile this[int row, int column] => Map[row, column];

    public Tile GetTileByVectorPosition(Vector2 position)
    {
        return Map[(int)(position.Y / TileHeight),
           (int)(position.X / TileWidth)];
    }

    public IEnumerable<Tile> GetTileNeighbours(Tile tile)
    {
        var tileRow = tile.Rectangle.Y / TileHeight;
        var tileColumn = tile.Rectangle.X / TileWidth;
        for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
                if (dx != 0 && dy != 0) continue;
                else
                {
                    var neighborRow = tileRow + dx;
                    var neighborColumn = tileColumn + dy;
                    if(neighborRow>-1 && neighborRow<mapWidth && neighborColumn > -1 && neighborColumn < mapHeight)
                        yield return Map[tileRow + dx, tileColumn + dy];
                }
    }

    public IEnumerable<Tile> GetTilesOnDirection(Vector2 start, Vector2 end)
    {
        var direction = MathOperations.GetDirectionToPoint(start.ToPoint(), end.ToPoint());
        direction.Normalize();
        var startTile = GetTileByVectorPosition(start);
        var endTile = GetTileByVectorPosition(end);
        while (startTile != endTile)
        {
            var x = start.X + direction.X;
            var y = start.Y + direction.Y;
            start = new Vector2(x, y);
            startTile = GetTileByVectorPosition(start);
            yield return startTile;
        }
    }

    public List<Tile> ToList()
    {
        var tiles = new List<Tile>(mapWidth * mapHeight);
        foreach (var tile in Map) tiles.Add(tile);
        return tiles;
    }

    public bool InBounds(Rectangle tile)
        => tile is { X: >= 0, Y: >= 0 }
           && Map.GetLength(0)*TileWidth > tile.X + TileWidth
           && Map.GetLength(1)*TileHeight > tile.Y+TileHeight;
}