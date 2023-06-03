using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGameName;

public class Portal : IGameEntity
{
    private ProgressBar energyAmountBar;
    private PortalTextureContainer textureContainer;
    public UpdateOrder UpdateOrder => UpdateOrder.Portal;
    public DrawOrder DrawOrder => DrawOrder.Portal;
    public double Health => 0;
    public double Damage => 0;
    public bool IsAlive => true;
    public Rectangle Rectangle { get; private set; }
    public EntityType Type => EntityType.Portal;
    public Vector2 Position { get; private set; }
    public Texture2D Texture { get; private set; }
    public int CurrentEnergyAmount { get; private set; }
    public int MaxEnergyAmount { get; private set; }
    public Texture2D Message { get; private set; }
    public int ActivatorsAmount { get; private set; }
    public int CurrentActivatorsAmount { get; private set; }
    public bool IsActivated { get; private set; }


    public Portal(PortalTextureContainer textureContainer, Vector2 position, int energyAmount, int activatorsAmount)
    {
        this.textureContainer = textureContainer;
        Texture = textureContainer.Stage0;
        Position = position;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        CurrentEnergyAmount = 0;
        CurrentActivatorsAmount = 0;
        MaxEnergyAmount = energyAmount;
        SetProgressBar();
        Message = Texture;
        ActivatorsAmount = activatorsAmount;
        IsActivated = false;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        energyAmountBar.Draw(spriteBatch, gameTime);
        spriteBatch.Draw(Message, Position, null, Color.White, 0,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
    }

    public void TakeDamage(double damage)
    {

    }

    public void Update(GameTime gameTime)
    {
        if (CurrentEnergyAmount <= MaxEnergyAmount / 3) Texture = textureContainer.Stage0;
        else if (CurrentEnergyAmount <= 2 * MaxEnergyAmount / 3) Texture = textureContainer.Stage1;
        else if (CurrentEnergyAmount >= MaxEnergyAmount) Texture = textureContainer.Stage2;
        energyAmountBar.Update(CurrentEnergyAmount);
    }

    public void SetProgressBar()
    {
        energyAmountBar = new ProgressBar(textureContainer.BarBg, textureContainer.BarFg,
            MaxEnergyAmount, Position - new Vector2(Texture.Width / 2, Texture.Height / 2), this);
    }

    public void TakeEnergy(int energy)
    {
        if (CurrentEnergyAmount >= MaxEnergyAmount)
        {
            Activate();
            return;
        }
        CurrentEnergyAmount += energy;
    }

    public void TakeActivator(int amount)
    {
        if(CurrentActivatorsAmount < ActivatorsAmount)
            CurrentActivatorsAmount += amount;
        Activate();
    }

    public void Activate()
    {
        if(CurrentActivatorsAmount >= ActivatorsAmount && CurrentEnergyAmount>=MaxEnergyAmount)
        {
            IsActivated = true;
        }
    }

    public void Restart()
    {
        CurrentActivatorsAmount = 0;
        CurrentEnergyAmount = 0;
        Texture = textureContainer.Stage0;
        IsActivated = false;
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        if (entityToIntersect.Type == EntityType.Cursor)
        {
            if (CurrentEnergyAmount < MaxEnergyAmount)
                Message = textureContainer.EnergyMessage;
            else
                Message = textureContainer.ActivatatorsMessage;
        }
        else Message = Texture;
    }
}