using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace TheGameName;

public class InventoryItem: IGameEntity
{
    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public UpdateOrder UpdateOrder => UpdateOrder.Inventory;
    public DrawOrder DrawOrder => DrawOrder.Inventory;
    public Texture2D Texture { get; private set; }
    public int Amount {get; private set; }
    public DropType DropType { get; private set; }
    public Color Color { get; private set; }
    public double Health => 0;
    public double Damage => 0;
    public bool IsAlive => true;
    public EntityType Type => EntityType.Inventory;

    public InventoryItem(Texture2D texture, Vector2 position, DropType dropType, int amount)
    {
        Amount = amount;
        Texture = texture;
        Position = position;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        DropType = dropType;
        Color = Color.White;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, Color);
        spriteBatch.DrawString(TheGameName.FontThin, $"{Amount}", Position, Color.White);
    }

    public void Update(GameTime gameTime)
    {
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void IncreaseAmount(int amount)
    {
        Amount += amount;
    }

    public bool DecreaseAmount(int amount)
    {
        if (Amount <= 0) return false;
        Amount -= amount;
        return true;
    }

    public void Move(Vector2 position)
    {
        Position += position;
    }

    public void SetToHovered()
    {
        Color = Color.White;
    }

    public void SetToStatic()
    {
        Color = Color.Gray;
    }

    public void TakeDamage(double damage)
    {
       
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        
    }
}