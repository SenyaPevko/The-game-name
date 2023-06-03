using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using System;

namespace TheGameName;

public class Enemy : IGameEntity, ICanAttack
{
    private const int ATTACK_DISTANCE = 300;
    private const int ATTACK_RATE = 1000;
    private const int MAGAZINE_SIZE = 10;
    private const int WALK_SPRITE_COUNT = 1;

    private float rotation;
    private double attackDelayTimer = 1001;
    private double protectionTimer = 0;
    private double protectionTime = 1000;
    private bool isProtected;
    private double healRate = 3500;
    private double healTimer;
    private EnemyMovementAI pathFinder = new ();
    private readonly RenderStateController renderStateController = new ();
    private EnemyTextureContainer textureContainer;
    public bool CurrentlyFindingPath { get; private set; }
    public ProgressBar HealthBar { get; private set; }
    public UpdateOrder UpdateOrder { get; private set; } = UpdateOrder.Enemy;
    public DrawOrder DrawOrder { get; private set; } = DrawOrder.Enemy;
    public Vector2 Position { get; private set; }
    public float Speed { get; private set; } = 1.5f;
    public double Health { get; private set; } = 4;
    public bool IsAlive { get; private set; } = true;
    public double Damage { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public EntityType Type { get; private set; } = EntityType.Enemy;
    public Player Player { get; private set; }
    public int AttackCounter { get; private set; } = MAGAZINE_SIZE;
    public double AttackDelayTimer { get; private set; }
    public Vector2 SpriteOrigin { get; private set; }
    public string CurrentState { get; private set; }
    public Vector2 CurrentDirection { get; private set; }
    public int SpriteHeight { get; private set; } = 34;
    public int SpriteWidth { get; private set; } = 34;
    public Color Color { get; private set; }

    public Enemy(Vector2 position, EnemyTextureContainer textureContainer, Player player, double health)
    {
        Position = position;
        SpriteHeight = textureContainer.WalkDown.Height;
        SpriteWidth = textureContainer.WalkDown.Width;
        SpriteOrigin = new Vector2(SpriteWidth / 2f, SpriteHeight / 2f);
        this.textureContainer = textureContainer;
        AddAnimationStates();
        CurrentState = nameof(EnemyTextureContainer.WalkDown);
        Player = player;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, SpriteWidth, SpriteHeight);
        Health = health;
        CurrentlyFindingPath = false;
        HealthBar = new ProgressBar(textureContainer.HealthBarBg, textureContainer.HealthBarFg, Health,
                Position - SpriteOrigin, this);
        Color = Color.White;
    }

    public void Update(GameTime gameTime)
    {
        renderStateController.Update(gameTime);
        if (!Move()) Attack(gameTime);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, SpriteWidth, SpriteHeight);
        HealthBar.Update(Health);
        Heal(gameTime);
        SetProtection(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        renderStateController.Draw(spriteBatch, Position, SpriteEffects.None, 1f, Color);
        HealthBar.Draw(spriteBatch, gameTime);
    }

    public bool Move()
    {
        ChooseState();
        if (CheckIfCanAttack()) return false;
        if (!CurrentlyFindingPath)
        {
            var findPathTask = new Task(() =>
            {
                CurrentlyFindingPath = true;
                var nextTile = pathFinder.FindPath(TheGameName.TileMap.GetTileByVectorPosition(Position),
            TheGameName.TileMap.GetTileByVectorPosition(Player.Position));
                if (nextTile == null) return;
                var directionToNextTile = MathOperations.GetDirectionToPoint(Position.ToPoint(), nextTile.Rectangle.Center);
                var angleToNextTile = MathOperations.GetAngleBetweenPoints(Position.ToPoint(), nextTile.Rectangle.Center);
                directionToNextTile.Normalize();
                CurrentDirection = directionToNextTile;
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

    public void SetProtection(GameTime gameTime)
    {
        isProtected = true;
        Color = Color.DarkGray;
        protectionTimer += gameTime.ElapsedGameTime.Milliseconds;
        if(protectionTimer> protectionTime)
        {
            UndoCollorEffects();
            isProtected = false;
        }
    }

    public void Reload(GameTime gameTime)
    {
        AttackCounter = MAGAZINE_SIZE;
    }

    public void TakeDamage(double damage)
    {
        if(isProtected) return;
        Health -= damage;
        if (Health <= 0) IsAlive = false;
        if (!IsAlive)
        {
            TheGameName.DropSpawner.Spawn(Position + new Vector2(5,-5), DropType.Health, 2);
            TheGameName.DropSpawner.Spawn(Position, DropType.Energy, 1);
        }
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
       
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
            !TheGameName.TileMap.GetTilesOnDirection(Position, Player.Position)
            .Where(tile => tile.Collision == TileCollision.Impassable).Any())
        {
            rotation = angleToPlayer;
            return true;
        }
        return false;
    }

    public void UndoCollorEffects()
    {
        if (protectionTimer > protectionTime)
        {
            Color = Color.White;
        }
    }

    public void AddAnimationStates()
    {
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkDown),
            new SpriteAnimation(textureContainer.WalkDown, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkDownLeft),
            new SpriteAnimation(textureContainer.WalkDownLeft, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkDownRight),
            new SpriteAnimation(textureContainer.WalkDownRight, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkUp),
            new SpriteAnimation(textureContainer.WalkUp, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
        renderStateController.SetState(nameof(EnemyTextureContainer.WalkUp));
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkUpRight),
            new SpriteAnimation(textureContainer.WalkUpRight, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkUpLeft),
            new SpriteAnimation(textureContainer.WalkUpLeft, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkRight),
            new SpriteAnimation(textureContainer.WalkRight, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
        renderStateController.AddState(nameof(EnemyTextureContainer.WalkLeft),
            new SpriteAnimation(textureContainer.WalkLeft, WALK_SPRITE_COUNT, SpriteWidth, SpriteHeight, SpriteOrigin));
    }

    public void ChooseState()
    {
        var right = rotation>=0 && rotation<=Math.PI/6;;
        var upRight = rotation > -Math.PI / 3 && rotation <= -Math.PI / 6;
        var up = rotation > -2 * Math.PI / 3 && rotation <= -Math.PI / 3;
        var upLeft = rotation > 2 * Math.PI / 3 && rotation <= 5 * Math.PI / 6;
        var left = rotation > 5 * Math.PI / 6 && rotation <= 7 * Math.PI / 6;
        var downLeft = rotation > 7 * Math.PI / 6 && rotation <= 4 * Math.PI / 3;
        var down = rotation > Math.PI / 3 && rotation <= 2 * Math.PI / 3;
        var downRight = rotation > Math.PI / 6 && rotation <= Math.PI / 3;
        if (right) 
            CurrentState = nameof(EnemyTextureContainer.WalkRight);
        else if (upRight) 
            CurrentState = nameof(EnemyTextureContainer.WalkUpRight);
        else if (up) CurrentState = nameof(EnemyTextureContainer.WalkUp);
        else if (upLeft) CurrentState = nameof(EnemyTextureContainer.WalkUpLeft);
        else if (left) CurrentState = nameof(EnemyTextureContainer.WalkLeft);
        else if (downLeft) CurrentState = nameof(EnemyTextureContainer.WalkDownLeft);
        else if (down) CurrentState = nameof(EnemyTextureContainer.WalkDown);
        else if(downRight) CurrentState = nameof(EnemyTextureContainer.WalkDownRight);
        renderStateController.SetState(CurrentState);
        renderStateController.CurrentState.Play();
    }
}