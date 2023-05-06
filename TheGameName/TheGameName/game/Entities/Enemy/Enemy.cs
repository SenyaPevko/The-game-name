using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class Enemy : IGameEntity, ICanAttack
{
    private const int ATTACK_DISTANCE = 200;
    private const int ATTACK_RATE = 1000;
    private const int MAGAZINE_SIZE = 10;

    private float rotation;
    private double attackDelayTimer;

    public Order UpdateOrder { get; private set; } = Order.Enemy;
    public Order DrawOrder { get; private set; } = Order.Enemy;
    public Vector2 Position { get; private set; }
    public Texture2D Texture { get; private set; }
    public float Speed { get; private set; } = 1.5f;
    public double Health { get; private set; } = 4;
    public bool IsAlive { get; private set; } = true;
    public double Damage { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public EntityType Type { get; private set; } = EntityType.Enemy;
    public Player Player { get; private set; }
    public int AttackCounter { get; private set; } = MAGAZINE_SIZE;
    public double AttackDelayTimer { get; private set; }


    public Enemy(Vector2 position, Texture2D texture, Player player)
    {
        Position = position;
        Texture = texture;
        this.Player = player;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        if (!Move()) Attack(gameTime);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.Aqua, rotation,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
    }

    public bool Move()
    {
        var pathFinder = new EnemyMovementAI();
        var nextTile = pathFinder.FindPath(Globals.tileMap, Position, Player.Position);
        
        var directionToNextTile = MathOperations.GetDirectionToPoint(Position.ToPoint(), nextTile.Rectangle.Center);
        var angleToNextTile = MathOperations.GetAngleBetweenPoints(Position.ToPoint(), nextTile.Rectangle.Center);
        directionToNextTile.Normalize();

        var directionToPlayer = MathOperations.GetDirectionToPoint(Position.ToPoint(), Player.Position.ToPoint());
        var angleToPlayer = MathOperations.GetAngleBetweenPoints(Position.ToPoint(), Player.Position.ToPoint());
        var distanceToPlayer = directionToPlayer.Length();
        directionToPlayer.Normalize();
        if (distanceToPlayer <= ATTACK_DISTANCE && 
            Globals.tileMap.GetTilesOnDirection(Position, Player.Position)
            .Where(tile=>tile.Collision != TileCollision.Walkable)
            .Count() == 0)
        {
            rotation = angleToPlayer;
            return false;
        }
        var x = Position.X + directionToNextTile.X * Speed;
        var y = Position.Y + directionToNextTile.Y * Speed;
        Position = new Vector2(x, y);
        rotation = angleToNextTile;
        return true;
    }

    public bool Attack(GameTime gameTime)
    {
        if (AttackCounter <= 0) return false;
        attackDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (attackDelayTimer > ATTACK_RATE)
        {
            attackDelayTimer = 0;
            Globals.bulletsContoller.AddBulletToFired(this, Player.Position);
            return true;
        }
        return false;
    }

    public void Reload(GameTime gameTime)
    {
        AttackCounter = MAGAZINE_SIZE;
    }

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) IsAlive = false;
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
       
    }
}