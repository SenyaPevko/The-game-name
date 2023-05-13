using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;
public class Camera: IGameEntity
{
    public Matrix Transform;
    public float Rotation { get; set; }
    public Vector2 Position { get; set; }
    public Player Player { get; set; }
    public float Width { get; private set; }
    public float Height { get; private set; }

    public Order UpdateOrder => throw new NotImplementedException();

    public Order DrawOrder => throw new NotImplementedException();

    public double Health => throw new NotImplementedException();

    public double Damage => throw new NotImplementedException();

    public bool IsAlive => throw new NotImplementedException();

    public Rectangle Rectangle => throw new NotImplementedException();

    public EntityType Type => throw new NotImplementedException();

    public Camera(Player player)
    {
        Player = player;
        Position = player.Position; 
    }

    public void Move(Vector2 change)
    {
        Position += change;
    }

    public Matrix GetTransforamtion(GraphicsDevice graphicsDevice)
    {
        Width = graphicsDevice.Viewport.Width * 0.5f;
        Height = graphicsDevice.Viewport.Height * 0.5f;
        Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0))*
            Matrix.CreateRotationZ(Rotation)
            * Matrix.CreateTranslation(
                new Vector3(Width, Height, 0));
        return Transform;
    }

    public void Update(GameTime gameTime)
    {
        var mapWidth = Globals.tileMap.TileWidth * Globals.tileMap.MapWidth;
        var mapHeight = Globals.tileMap.TileHeight * Globals.tileMap.MapHeight;
        var canMoveHorizontaly = !((Width + Player.Position.X) >= mapWidth || (Player.Position.X - Width) <= 0);
        var canMoveVertically = !((Height + Player.Position.Y) >= mapHeight || (Player.Position.Y - Height) <= 0);
        if (canMoveHorizontaly && canMoveVertically)
            Position = Player.Position;
        else if (!canMoveHorizontaly || !canMoveVertically)
        {
            var cameraX = Player.Position.X;
            var cameraY = Position.Y;
            if (!canMoveHorizontaly)
            {
                cameraX = Position.X;
                Position = new Vector2(cameraX, Player.Position.Y);
            }
            if (!canMoveVertically)
            {
                Position = new Vector2(cameraX, cameraY);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(double damage)
    {
        throw new NotImplementedException();
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        throw new NotImplementedException();
    }
}

