using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameName;

public interface IGameEntity: IUpdatable
{
    UpdateOrder UpdateOrder { get; }
    DrawOrder DrawOrder { get; }

    public double Health { get;}

    public double Damage { get; }

    public bool IsAlive { get;}

    public Rectangle Rectangle { get;}

    public EntityType Type { get;}

    public Vector2 Position { get;}

    void Draw(SpriteBatch spriteBatch, GameTime gameTime);

    public void TakeDamage(double damage);

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect);

    public void Restart()
    {
        ;
    }
}
