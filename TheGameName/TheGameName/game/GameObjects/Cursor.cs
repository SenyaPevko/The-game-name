using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using TheGameName;

public class Cursor : IComponent
{
    public Texture2D Texture { get; private set; }
    public Vector2 Scale { get; private set; }
    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public Order UpdateOrder { get; private set; } = Order.Cursor;
    public Order DrawOrder { get; private set; } = Order.Cursor;

    public Cursor(Texture2D texture)
    {
        this.Texture = texture;
        Scale = new Vector2(0, 0);
    }
    public void Update(GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        var x = mouse.X - 650;
        var y = mouse.Y - 350;
        Position = new Vector2(x, y);
    }

    public void Draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        spritebatch.Draw(Texture, Position, Color.AliceBlue);
    }
}