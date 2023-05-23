using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class Bullet : IGameEntity
{
    public double damage = 5d;
    public float speed = 6f;
    public float rotation;
    public Vector2 direction;
    public Texture2D Texture { get; private set; }
    public Vector2 Scale { get; private set; }
    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public UpdateOrder UpdateOrder { get; private set; } = UpdateOrder.Bullet;
    public DrawOrder DrawOrder { get; private set; } = DrawOrder.Bullet;
    public double Health { get; private set; } = 1;
    public double Damage { get; private set; } = 2;
    public bool IsAlive { get; private set; } = true;
    public EntityType Type { get; private set; } = EntityType.Bullet;
    public IGameEntity Sender { get; private set; }

    public Bullet(Texture2D Texture, IGameEntity sender, Vector2 target)
    {
        this.Texture = Texture;
        Position = sender.Position;
        direction = -sender.Position + target;
        rotation = MathOperations.GetAngleBetweenPoints(sender.Position.ToPoint(), target.ToPoint());
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        Sender = sender;
    }

    public void Update(GameTime gameTime)
    {
        Move();
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.Aqua, rotation,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        if (entityToIntersect.Type == EntityType.Drop || 
            entityToIntersect.Type == EntityType.Portal) return;
        if (entity.Type == EntityType.Bullet && entityToIntersect.Type == EntityType.Tile)
        {
            var tile = (Tile)entityToIntersect;
            if (tile.Collision == TileCollision.Impassable)
                entity.TakeDamage(entity.Damage);
        }
        else if (entity.Type == EntityType.Bullet && ((Bullet)entity).Sender.Type != entityToIntersect.Type)
        {
            if (entityToIntersect.Type == EntityType.Player && ((Player)entityToIntersect).IsDodging) 
                return;
            entityToIntersect.TakeDamage(entity.Damage);
            IsAlive = false;
        }
    }

    public void Move()
    {
        direction.Normalize();
        var x = Position.X + direction.X * speed;
        var y = Position.Y + direction.Y * speed;
        Position = new Vector2(x, y);
    }

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) IsAlive = false;
    }
}