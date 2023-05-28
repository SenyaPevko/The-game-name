using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework.Internal;

namespace TheGameName;

public class Inventory : IGameEntity
{
    private Dictionary<DropType, InventoryItem> inventoryItems;

    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public UpdateOrder UpdateOrder => UpdateOrder.Inventory;
    public DrawOrder DrawOrder => DrawOrder.Inventory;
    public InventoryTextureContainer TextureContainer { get; private set; }
    public Camera Camera { get; private set; }
    public Vector2 CameraPreviousPosition { get; private set; }
    public Vector2 Offset { get; private set; } = new Vector2(10, 0);
    public float TextureWidth { get; private set; }
    public InventoryItem ChosenItem { get; private set; }
    public double Health => 0;
    public double Damage => 0;
    public bool IsAlive => true;
    public double Width { get; private set; }
    public double Height { get; private set; }

    public EntityType Type => EntityType.Inventory;

    public Inventory(InventoryTextureContainer textureContainer, Vector2 position, Camera camera)
    {
        Position = position;
        TextureContainer = textureContainer;
        Camera = camera;
        CameraPreviousPosition = Camera.Position;
        TextureWidth = textureContainer.Energy.Width;
        Initialize();
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var item in inventoryItems)
            item.Value.Draw(spriteBatch, gameTime);
    }

    public void Update(GameTime gameTime)
    {

        foreach (var item in inventoryItems) item.Value.Update(gameTime);
        var changePosition = Move();
        foreach (var item in inventoryItems)
            item.Value.Move(changePosition);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height);
    }

    public Vector2 Move()
    {
        var cameraChangePosition = Camera.Position - CameraPreviousPosition;
        Position += cameraChangePosition;
        CameraPreviousPosition = Camera.Position;
        return cameraChangePosition;
    }

    public void Initialize()
    {
        inventoryItems = new Dictionary<DropType, InventoryItem>();
        inventoryItems.Add(DropType.Energy, new InventoryItem(TextureContainer.Energy,
            Position + Offset * 1, DropType.Energy, 0));
        inventoryItems.Add(DropType.Activator, new InventoryItem(TextureContainer.Activator,
            Position + Offset * 2 + new Vector2(TextureWidth, 0), DropType.Activator, 0));
        var firstItem = inventoryItems.Values.First();
        var lastItem = inventoryItems.Values.Last();
        Position = firstItem.Position;
        ChooseItem(firstItem.Rectangle);
        Height = firstItem.Rectangle.Height;
        Width = lastItem.Rectangle.X + lastItem.Rectangle.Width - firstItem.Rectangle.X - 10;
        Rectangle = new Rectangle((int)firstItem.Position.X, (int)firstItem.Position.Y, (int)Width, (int)Height);
    }

    public void AddAmountToItem(DropType dropType, int amount)
    {
        inventoryItems[dropType].IncreaseAmount(amount);
    }

    public bool DecreaseAmountOfItem(DropType dropType, int amount)
    {
        return inventoryItems[dropType].DecreaseAmount(amount);
    }

    public void Restart(Camera camera)
    {
        Camera = camera;
        foreach (var item in inventoryItems)
            item.Value.DecreaseAmount(item.Value.Amount);
    }

    public void ChooseItem(Rectangle rectangle)
    {
        foreach (var item in inventoryItems)
        {
            item.Value.SetToStatic();
            if (TheGameName.EntityController.Collide(item.Value.Rectangle, rectangle))
            {
                item.Value.SetToHovered();
                ChosenItem = item.Value;
            }
        }
    }

    public void Drop(Vector2 position, int amount)
    {
        if (TheGameName.Inventory.DecreaseAmountOfItem(ChosenItem.DropType, amount))
            TheGameName.DropSpawner.Spawn(position, ChosenItem.DropType, amount);
    }

    public void TakeDamage(double damage)
    {

    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        if (entityToIntersect.Type == EntityType.Cursor)
        {
            var size = new Point((int)(entityToIntersect.Rectangle.Size.X / 1.2),
                (int)(entityToIntersect.Rectangle.Size.Y / 1.2));
            var rectangle = new Rectangle(entityToIntersect.Rectangle.Center, size);
            ChooseItem(rectangle);
        }
    }
}