using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using TheGameName;

interface IComponent
{
    public Texture2D Texture { get; set; }
    public Vector2 Scale { get; set; }
    public Vector2 Position { get; set; }
    public Rectangle Rectangle { get; set; }
    public int UpdateOrder { get; set; }

    public int DrawOrder { get; set; }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }

    public void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }
}