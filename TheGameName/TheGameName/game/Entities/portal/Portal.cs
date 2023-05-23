using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

    public Portal(PortalTextureContainer textureContainer, Vector2 position, int energyAmount)
    {
        this.textureContainer = textureContainer;
        Texture = textureContainer.Stage0;
        Position = position;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        CurrentEnergyAmount = 0;
        MaxEnergyAmount = energyAmount;
        SetProgressBar();
        Message = Texture;
    }
    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        if (entityToIntersect.Type == EntityType.Cursor)
        {
            Message = textureContainer.EnergyMessage;
        }
        else Message = Texture;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        energyAmountBar.Draw(spriteBatch, gameTime);
        spriteBatch.Draw(Message, Position, null, Color.White, 0,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        /*spriteBatch.DrawString(Globals.fontThin, 
            $"{CurrentEnergyAmount}/{MaxEnergyAmount}", 
            energyAmountBar.Position, Color.Black);*/
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
        if(CurrentEnergyAmount >= MaxEnergyAmount) return;
        CurrentEnergyAmount += energy;
    }
}