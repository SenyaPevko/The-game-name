using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameName;

public class Player : IGameEntity, ICanAttack
{
    private const int WALK_SPRITE_COUNT = 3;
    private const int SPRITE_HEIGHT = 34;
    private const int SPRITE_WIDTH = 32;

    private const double ATTACK_RATE = 100;

    private readonly RenderStateController renderStateController = new RenderStateController();
    private PlayerTextureContainer textureContainer;
    private double damageRate = 5000;
    private double damageTimer;
    private double maxHealth;
    private double dodgeRate = 3000;
    private double dodgeTimer = 3001;
    private double dodgingTime = 700;
    private double takingDamageTimer;
    private double takingDamageDuration = 100;
    private double dropRate = 200;
    private double dropTimer = 201;
    public bool IsDodging { get; private set; }

    public float Speed { get; private set; } = 2;
    public double Health { get; private set; }
    public Vector2 Position { get; private set; }
    public UpdateOrder UpdateOrder { get; private set; } = UpdateOrder.Player;
    public DrawOrder DrawOrder { get; private set; } = DrawOrder.Player;
    public string CurrentState { get; private set; } = nameof(PlayerTextureContainer.WalkUp);
    public double Damage { get; private set; }
    public bool IsAlive { get; private set; } = true;
    public Rectangle Rectangle { get; private set; }
    public EntityType Type { get; private set; } = EntityType.Player;
    public int MagazineSize { get; private set; } = 10;
    public int AttackCounter { get; private set; } = 10;
    public double AttackDelayTimer { get; set; }
    public float TextureOpacity { get; private set; } = 1;
    public Color Color { get; private set; }
    public Vector2 CurrentDirection { get; private set; }
    public Vector2 SpriteOrigin { get; private set; }


    public Player(PlayerTextureContainer textureContainer, Vector2 position, double health)
    {
        Position = position;
        SpriteOrigin = new Vector2(SPRITE_WIDTH / 2f, SPRITE_HEIGHT / 1.3f);
        this.textureContainer = textureContainer;
        AddAnimationStates();
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        Health = health;
        maxHealth = health;
        IsDodging = false;
        Color = Color.White;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        renderStateController.Draw(spriteBatch, Position, SpriteEffects.None, TextureOpacity, Color);
    }

    public void Update(GameTime gameTime)
    {
        renderStateController.Update(gameTime);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, SPRITE_WIDTH, SPRITE_HEIGHT);
        damageTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (damageRate < damageTimer)
        {
            damageTimer = gameTime.ElapsedGameTime.Milliseconds; ;
            TakeDamage(TheGameName.TileMap.LevelDamage);
        }
        StopDodging(gameTime);
        UndoDamageEffects(gameTime);
    }

    public void Move(Vector2 direction, string state)
    {
        CurrentDirection = direction;
        CurrentState = state;
        renderStateController.SetState(state);
        renderStateController.CurrentState.Play();
        var nextPosition = Position + direction * Speed;
        var nextRectangle = new Rectangle(nextPosition.ToPoint(), Rectangle.Size);
        var nextTile = TheGameName.TileMap.GetTileByVectorPosition(nextPosition);
        if (nextTile.Collision != TileCollision.Walkable && nextTile.Rectangle.Intersects(nextRectangle))
            return;
        Position += direction * Speed;
    }

    public void Halt()
    {
        renderStateController.SetState(CurrentState);
        renderStateController.CurrentState.Play();
        renderStateController.CurrentState.Pause();
    }

    public bool Attack(GameTime gameTime)
    {
        if (AttackCounter <= 0) return false;
        AttackDelayTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (AttackDelayTimer > ATTACK_RATE)
        {
            AttackDelayTimer = 0;
            TheGameName.BulletsContoller.AddBulletToFired(this, TheGameName.WorldСursor.Position);
            AttackCounter--;
            return true;
        }
        return false;
    }

    public void Reload(GameTime gameTime)
    {
        AttackCounter = MagazineSize;
        AttackDelayTimer = -1000;
    }

    public void TakeDamage(double damage)
    {
        takingDamageTimer = 0;
        Health -= damage;
        if (Health <= 0) IsAlive = false;
        Color = Color.PaleVioletRed;
    }

    public void UndoDamageEffects(GameTime gameTime)
    {
        takingDamageTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (takingDamageTimer > takingDamageDuration)
        {
            UndoCollorEffects();
            takingDamageTimer = gameTime.ElapsedGameTime.Milliseconds;
        }
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

    public void Heal(double health)
    {
        if (health + Health > maxHealth) Health = maxHealth;
        else
        {
            Health += health;
        }
    }

    public void Dodge(GameTime gameTime)
    {
        dodgeTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (dodgeTimer > dodgeRate)
        {
            IsDodging = true;
            TextureOpacity = 0.8f;
            dodgeTimer = 0;
            Speed *= 1.8f;
            Color = Color.SlateGray;
        }
    }

    public void StopDodging(GameTime gameTime)
    {
        dodgeTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (dodgingTime < dodgeTimer)
        {
            IsDodging = false;
            TextureOpacity = 1;
            Speed = 2;
            UndoCollorEffects();
        }   
    }

    public void UndoCollorEffects()
    {
        if (dodgingTime < dodgeTimer && takingDamageTimer > takingDamageDuration)
        {
            Color = Color.White;
        }
    }

    public void Drop(int amount, GameTime gameTime)
    {
        dropTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (dropRate < dropTimer)
        {
            TheGameName.Inventory.Drop(Position + CurrentDirection *
                new Vector2(SPRITE_WIDTH + 5, SPRITE_HEIGHT + 5), amount);
            dropTimer = gameTime.ElapsedGameTime.Milliseconds;
        }
    }

    public void AddAnimationStates()
    {
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDown),
            new SpriteAnimation(textureContainer.WalkDown, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDownLeft),
            new SpriteAnimation(textureContainer.WalkDownLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkDownRight),
            new SpriteAnimation(textureContainer.WalkDownRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUp),
            new SpriteAnimation(textureContainer.WalkUp, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
        renderStateController.SetState(nameof(PlayerTextureContainer.WalkUp));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUpRight),
            new SpriteAnimation(textureContainer.WalkUpRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkUpLeft),
            new SpriteAnimation(textureContainer.WalkUpLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkRight),
            new SpriteAnimation(textureContainer.WalkRight, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
        renderStateController.AddState(nameof(PlayerTextureContainer.WalkLeft),
            new SpriteAnimation(textureContainer.WalkLeft, WALK_SPRITE_COUNT, SPRITE_WIDTH, SPRITE_HEIGHT, SpriteOrigin));
    }
}
