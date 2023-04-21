using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class Bullet : IComponent
{
    public double damage = 5d;
    public float speed = 6f;
    public float rotation;
    public Vector2 direction;
    public Texture2D Texture { get; set; }
    public Vector2 Scale { get; set; }
    public Vector2 Position { get; set; }
    public Rectangle Rectangle { get; set; }
    public int UpdateOrder { get; set; } = 0;
    public int DrawOrder { get; set; } = 0;

    private readonly RenderStateController renderStateController = new RenderStateController();

    public Bullet(Texture2D Texture, Player player)
    {
        this.Texture = Texture;
        Position = player.Position;
        direction = -player.Position + Globals.cursor.Position;
        rotation = MathOperations.GetAngleBetweenPoints(player.Position.ToPoint(), Globals.cursor.Position.ToPoint());
    }

    public void Update(GameTime gameTime)
    {
        direction.Normalize();
        var x = Position.X + direction.X * speed;
        var y = Position.Y + direction.Y * speed;
        Position = new Vector2(x, y);
        Rectangle = new Rectangle(Position.ToPoint(), new Point(Texture.Width, Texture.Height));
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        //renderStateController.Draw(spriteBatch, Position, SpriteEffects.None);
        spriteBatch.Draw(Texture, Position, null, Color.Aqua, rotation,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
    }
}