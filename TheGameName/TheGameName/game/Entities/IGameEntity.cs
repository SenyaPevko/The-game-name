using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public interface IGameEntity: IUpdatable
{
    int UpdateOrder { get; }
    int DrawOrder { get; }

    public double Health { get; set; }

    public double Damage { get; set; }

    public bool IsAlive { get; set; }

    public Rectangle Rectangle { get; set; }

    public string Type { get; set; }

    public Vector2 Position { get; set; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch, GameTime gameTime);

    public virtual void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            IsAlive = false;
        }
    }
}
