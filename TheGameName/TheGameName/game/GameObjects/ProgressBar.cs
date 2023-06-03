using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameName;

public class ProgressBar
{
    public Texture2D Background { get; private set; }
    public Texture2D Foreground { get; private set; }
    public Vector2 Position { get; private set; }
    public double MaxValue { get; private set; }
    public double CurrentValue { get; private set; }
    public Rectangle Part { get; private set; }
    public Vector2 TargetPreviousPosition { get; private set; }
    public IGameEntity Target;

    public ProgressBar(Texture2D bg, Texture2D fg, double max, Vector2 pos, IGameEntity target)
    {
        Background = bg;
        Foreground = fg;
        MaxValue = max;
        CurrentValue = max;
        Position = pos;
        Part = new(0, 0, Foreground.Width, Foreground.Height);
        Target = target;
        if(Target != null)
            TargetPreviousPosition = target.Position;
    }

    public void Update(double value)
    {
        CurrentValue = value;
        var width = (int)(CurrentValue / MaxValue * Foreground.Width);
        Part = new Rectangle(Part.X, Part.Y, width, Part.Height);
        Move();
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Background, Position, Color.White);
        spriteBatch.Draw(Foreground, Position, Part, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 1f);
    }

    public void Move()
    {
        Position += Target.Position - TargetPreviousPosition;
        TargetPreviousPosition = Target.Position;
    }
}