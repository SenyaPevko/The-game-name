using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Threading.Tasks;

namespace TheGameName;

public class Enemy : IGameEntity, ICanAttack
{
    private const int ATTACK_DISTANCE = 200;
    private const int ATTACK_RATE = 1000;
    private const int MAGAZINE_SIZE = 10;

    private float rotation;
    private double attackDelayTimer;
    private double healRate = 2000;
    private double healTimer;
    private EnemyMovementAI pathFinder = new EnemyMovementAI();
    public bool CurrentlyFindingPath { get; private set; }

    public ProgressBar HealthBar { get; private set; }

    public UpdateOrder UpdateOrder { get; private set; } = UpdateOrder.Enemy;
    public DrawOrder DrawOrder { get; private set; } = DrawOrder.Enemy;
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


    public Enemy(Vector2 position, Texture2D texture, Player player, double health)
    {
        Position = position;
        Texture = texture;
        Player = player;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        Health = health;
        CurrentlyFindingPath = false;
    }

    public void Update(GameTime gameTime)
    {
        if (!Move()) Attack(gameTime);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        HealthBar.Update(Health);
        Heal(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.Aqua, rotation,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        HealthBar.Draw(spriteBatch, gameTime);
    }

    public bool Move()
    {
        if (CheckIfCanAttack()) return false;
        if (!CurrentlyFindingPath)
        {
            var findPathTask = new Task(() =>
            {
                CurrentlyFindingPath = true;
                var nextTile = pathFinder.FindPath(TheGameName.TileMap, Position, Player.Position);
                if (nextTile == null) return;
                var directionToNextTile = MathOperations.GetDirectionToPoint(Position.ToPoint(), nextTile.Rectangle.Center);
                var angleToNextTile = MathOperations.GetAngleBetweenPoints(Position.ToPoint(), nextTile.Rectangle.Center);
                directionToNextTile.Normalize();
                var x = Position.X + directionToNextTile.X * Speed;
                var y = Position.Y + directionToNextTile.Y * Speed;
                Position = new Vector2(x, y);
                rotation = angleToNextTile;
                CurrentlyFindingPath = false;
            }
            );
            findPathTask.Start();
        }
        return true;
    }

    public bool Attack(GameTime gameTime)
    {
        if (AttackCounter <= 0) return false;
        attackDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (attackDelayTimer > ATTACK_RATE)
        {
            attackDelayTimer = 0;
            TheGameName.BulletsContoller.AddBulletToFired(this, Player.Position);
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
        if (!IsAlive)
        {
            TheGameName.DropSpawner.Spawn(Position + new Vector2(5,-5), DropType.Health, 1);
            TheGameName.DropSpawner.Spawn(Position, DropType.Energy, 1);
        }
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
       
    }

    public void SetHealthBar(ProgressBar healthBar)
    {
        HealthBar = healthBar;
        Health = healthBar.MaxValue;
    }

    public void Heal(GameTime gameTime)
    {
        healTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (Health < HealthBar.MaxValue / 3 && healRate < healTimer)
        {
            healTimer = gameTime.ElapsedGameTime.Milliseconds;
            Health += HealthBar.MaxValue/4;
        }
    }

    public bool CheckIfCanAttack()
    {
        var directionToPlayer = MathOperations.GetDirectionToPoint(Position.ToPoint(), Player.Position.ToPoint());
        var angleToPlayer = MathOperations.GetAngleBetweenPoints(Position.ToPoint(), Player.Position.ToPoint());
        var distanceToPlayer = directionToPlayer.Length();
        directionToPlayer.Normalize();
        if (distanceToPlayer <= ATTACK_DISTANCE &&
            TheGameName.TileMap.GetTilesOnDirection(Position, Player.Position)
            .Where(tile => tile.Collision == TileCollision.Impassable)
            .Count() == 0)
        {
            rotation = angleToPlayer;
            return true;
        }
        return false;
    }
}