using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameName;

public class EnergyDrop : IDropable
{
    public UpdateOrder UpdateOrder => UpdateOrder.Drop;
    public DrawOrder DrawOrder => DrawOrder.Drop;
    public double Damage => 0;
    public bool IsAlive { get; private set; } = true;
    public Rectangle Rectangle { get; private set; }
    public EntityType Type => EntityType.Drop;
    public Vector2 Position { get; private set; }
    public double Health { get; private set; } = 1;
    public Texture2D Texture { get; private set; }
    public Drop Drop { get; private set; }

    public EnergyDrop(Drop drop, Vector2 position)
    {
        Drop = drop;
        Texture = Drop.Texture;
        Position = position;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        if (entityToIntersect.Type == EntityType.Portal)
        {
            ((Portal)entityToIntersect).TakeEnergy(1);
            TakeDamage(Health);
        }
        else if (entityToIntersect.Type == EntityType.Player)
        {
            TheGameName.Inventory.AddAmountToItem(Drop.Type, 1);
            TakeDamage(Health);
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) IsAlive = false;
    }

    public void Update(GameTime gameTime)
    {
        
    }
}