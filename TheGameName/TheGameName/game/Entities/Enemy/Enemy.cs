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

    public int UpdateOrder => 2;
    public int DrawOrder => 2;
    public Vector2 Position { get; set; }
    public Texture2D Texture { get; set; }
    public float Speed { get; set; } = 1.5f;
    public double Health { get; set; } = 4;
    public bool IsAlive { get; set; } = true;
    public double Damage { get; set; }
    public Rectangle Rectangle { get; set; }
    public string Type { get; set; } = "Enemy";
    public Player player { get; set; }
    public int AttackCounter { get; set; } = MAGAZINE_SIZE;
    public double AttackDelayTimer { get; set; }


    public Enemy(Vector2 position, Texture2D texture, Player player)
    {
        Position = position;
        Texture = texture;
        this.player = player;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        if (!Move(player.Position))
            Attack(gameTime);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.Aqua, rotation,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
    }

    public bool Move(Vector2 targetPosition)
    {
        Vector2 direction = MathOperations.GetDirectionToPoint(Position.ToPoint(), targetPosition.ToPoint());
        float angle = MathOperations.GetAngleBetweenPoints(Position.ToPoint(), targetPosition.ToPoint());
        float distance = direction.Length();
        direction.Normalize();
        if (distance <= ATTACK_DISTANCE) return false;
        var x = Position.X + direction.X * Speed;
        var y = Position.Y + direction.Y * Speed;
        Position = new Vector2(x, y);
        rotation = angle;
        return true;
    }

    public bool Attack(GameTime gameTime)
    {
        if (AttackCounter <= 0) return false;
        attackDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (attackDelayTimer > ATTACK_RATE)
        {
            attackDelayTimer = 0;
            Globals.bulletsContoller.AddBulletToFired(this, player.Position);
            //AttackCounter--;
            return true;
        }
        return false;
    }

    public void Reload(GameTime gameTime)
    {
        AttackCounter = MAGAZINE_SIZE;
    }

    public virtual void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) IsAlive = false;
    }

}