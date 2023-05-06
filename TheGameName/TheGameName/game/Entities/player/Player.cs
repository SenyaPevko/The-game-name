using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class Player : IGameEntity, ICanAttack
{
    private const int WALK_SPRITE_COUNT = 3;
    private const int ATTACK_SPRITE_COUNT = 9;
    private const int DAMAGED_SPRITE_COUNT = 2;
    private const int SPRITE_HEIGHT = 34;
    private const int SPRITE_WIDTH = 32;
    private const int MAGAZINE_SIZE = 10;
    private const double ATTACK_RATE = 100;

    private readonly RenderStateController renderStateController = new RenderStateController();

    private float Speed { get; set; } = 2;
    public double Health { get; set; } = 10;
    public Vector2 Position { get; set; }
    public Order UpdateOrder { get; set; } = Order.Player;
    public Order DrawOrder { get; set; } = Order.Player;
    public string CurrentState { get; private set; } = nameof(PlayerTextureContainer.WalkUp);
    public double Damage { get; set; }
    public bool IsAlive { get; set; } = true;
    public Rectangle Rectangle { get; set; }
    public EntityType Type { get; set; } = EntityType.Player;
    public int AttackCounter { get; set; } = MAGAZINE_SIZE;
    public double AttackDelayTimer { get; set; }

    public Texture2D bulletTexture;


    public Player(TheGameName game, PlayerTextureContainer textureContainer, Vector2 position)
    {
        Position = position;
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDown),
            new SpriteAnimation(textureContainer.WalkDown, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDownLeft),
            new SpriteAnimation(textureContainer.WalkDownLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDownRight),
            new SpriteAnimation(textureContainer.WalkDownRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUp),
            new SpriteAnimation(textureContainer.WalkUp, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.SetState(nameof(PlayerTextureContainer.WalkUp));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUpRight),
            new SpriteAnimation(textureContainer.WalkUpRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUpLeft),
            new SpriteAnimation(textureContainer.WalkUpLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkRight),
            new SpriteAnimation(textureContainer.WalkRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkLeft),
            new SpriteAnimation(textureContainer.WalkLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT));

        /*        var attackAnimation = new SpriteAnimation(textureContainer.Attack,
                    ATTACK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT)
                { Fps = ATTACK_ANIM_FPS};
                attackAnimation.AnimationCompleted += Attack_AnimationCompleted;
                renderStateController.AddState(nameof(PlayerTextureContainer.Attack), attackAnimation);*/
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, SPRITE_WIDTH, SPRITE_HEIGHT);
    }

    /*    private void Attack_AnimationCompleted(object sender, AnimationCompletedEventArgs e)
        {
            renderStateController.SetState(nameof(PlayerTextureContainer.Idle));
        }*/

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        renderStateController.Draw(spriteBatch, Position, SpriteEffects.None);
    }

    public void Update(GameTime gameTime)
    {
        renderStateController.Update(gameTime);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, SPRITE_WIDTH, SPRITE_HEIGHT);
    }

    public void Move(Vector2 direction, string state)
    {
        CurrentState = state;
        renderStateController.SetState(state);
        renderStateController.CurrentState.Animation.Play();
        var nextPosition = Position + direction * Speed;
        var nextTile = Globals.tileMap.GetTileByVectorPosition(nextPosition);
        if (nextTile.Collision != TileCollision.Walkable && nextTile.Rectangle.Contains(nextPosition.ToPoint()))
            return;
        Position += direction * Speed;
    }

    public void Halt()
    {
        renderStateController.SetState(CurrentState);
        renderStateController.CurrentState.Animation.Play();
        renderStateController.CurrentState.Animation.Pause();
    }

    public bool Attack(GameTime gameTime)
    {
        if (AttackCounter <= 0) return false;
        AttackDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (AttackDelayTimer > ATTACK_RATE)
        {
            AttackDelayTimer = 0;
            //renderStateController.SetState(nameof(PlayerTextureContainer.Attack));
            //renderStateController.CurrentState.Animation.Play();
            Globals.bulletsContoller.AddBulletToFired(this, Globals.cursor.Position);
            AttackCounter--;
            return true;
        }
        return false;
    }

    public void Reload(GameTime gameTime)
    {
        AttackCounter = MAGAZINE_SIZE;
        AttackDelayTimer = -1000;
    }

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) IsAlive = false;
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        if(entityToIntersect.Type == EntityType.Tile)
        {
            var tile = (Tile)entityToIntersect;
            if (tile.Collision == TileCollision.Attackable)
                TakeDamage(tile.Damage);
        }
    }
}
