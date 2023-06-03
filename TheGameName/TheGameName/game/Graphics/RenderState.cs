using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameName;


public class RenderState : IUpdatable
{

    public string Name { get; }
    public IAnimation Animation { get; }

    public RenderState(string name, IAnimation animation)
    {
        Name = name;
        Animation = animation;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float opacity, Color color)
    {
        Animation?.Draw(spriteBatch, position, spriteEffects, opacity, color);
    }

    public void Update(GameTime gameTime)
    {
        Animation?.Update(gameTime);
    }

    public void Play()
    {
        Animation?.Play();
    }

    public void Pause()
    {
        Animation.Pause();
    }
}