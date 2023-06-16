using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameName;

public class ProgressBar: IGameEntity
{
    public Texture2D Background { get; private set; }
    public Texture2D Foreground { get; private set; }
    public Vector2 Position { get; private set; }
    public double MaxValue { get; private set; }
    public double CurrentValue { get; private set; }
    public Rectangle Part { get; private set; }
    public Vector2 TargetPreviousPosition { get; private set; }
    public UpdateOrder UpdateOrder => UpdateOrder.ProgressBar;
    public DrawOrder DrawOrder => DrawOrder.ProgressBar;
    public double Health => 0;
    public double Damage => 0;
    public bool IsAlive { get; private set; } = true;
    public Rectangle Rectangle {get; private set;}
    public EntityType Type => EntityType.ProgressBar;
    public IGameEntity Target { get; private set; }

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

    public void TakeDamage(double damage)
    {
        throw new System.NotImplementedException();
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        throw new System.NotImplementedException();
    }

    public void Update(GameTime gameTime)
    {
        throw new System.NotImplementedException();
    }
}