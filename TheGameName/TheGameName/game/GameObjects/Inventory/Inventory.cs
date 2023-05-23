using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class Inventory : IComponent
{
    private Dictionary<DropType,InventoryItem> inventoryItems;

    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public DrawOrder UpdateOrder => DrawOrder.Inventory;
    public DrawOrder DrawOrder => DrawOrder.Inventory;
    public InventoryTextureContainer TextureContainer { get; private set; }
    public Camera Camera { get; private set; }
    public Vector2 CameraPreviousPosition { get; private set; }
    public Vector2 Offset { get; private set; } = new Vector2(10, 0);

    public Inventory(InventoryTextureContainer textureContainer, Vector2 position, Camera camera)
    {
        Position = position;
        this.TextureContainer = textureContainer;
        Camera = camera;
        CameraPreviousPosition = Camera.Position;
        Initialize();
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var item in inventoryItems) 
            item.Value.Draw(spriteBatch, gameTime);
    }

    public void Update(GameTime gameTime)
    {
        foreach(var item in inventoryItems) item.Value.Update(gameTime);
        var changePosition = Move();
        foreach (var item in inventoryItems)
            item.Value.Move(changePosition);
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
        inventoryItems = new Dictionary<DropType,InventoryItem>();
        inventoryItems.Add(DropType.Energy,new InventoryItem(TextureContainer.Energy, Position+Offset*0,0));
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
        foreach(var item in inventoryItems) 
            item.Value.DecreaseAmount(item.Value.Amount);
    }
}