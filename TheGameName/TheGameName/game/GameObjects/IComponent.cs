using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using TheGameName;

interface IComponent
{
    public Texture2D Texture { get; }
    public Vector2 Scale { get; }
    public Vector2 Position { get; }
    public Rectangle Rectangle { get; }
    public Order UpdateOrder { get; }
    public Order DrawOrder { get; }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }

    public void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }
}