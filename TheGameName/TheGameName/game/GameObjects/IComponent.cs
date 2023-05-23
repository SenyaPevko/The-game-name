using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using TheGameName;

interface IComponent: IUpdatable
{
    public Vector2 Position { get; }
    public Rectangle Rectangle { get; }
    public DrawOrder UpdateOrder { get; }
    public DrawOrder DrawOrder { get; }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime);
}