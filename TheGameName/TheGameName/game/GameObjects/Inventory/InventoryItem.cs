using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;

namespace TheGameName;

public class InventoryItem : IComponent
{
    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public DrawOrder UpdateOrder => DrawOrder.Inventory;
    public DrawOrder DrawOrder => DrawOrder.Inventory;
    public Texture2D Texture { get; private set; }
    public int Amount {get; private set; }

    public InventoryItem(Texture2D texture, Vector2 position, int amount)
    {
        Amount = amount;
        Texture = texture;
        Position = position;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
        spriteBatch.DrawString(Globals.fontThin, $"{Amount}", Position, Color.White);
    }

    public void Update(GameTime gameTime)
    {
        
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
}