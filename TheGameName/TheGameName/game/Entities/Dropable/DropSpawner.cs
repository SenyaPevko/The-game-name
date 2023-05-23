using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class DropSpawner
{
    private Dictionary<DropType, Drop> drops;

    public DropSpawner(DropTextureContainer dropTextureContainer)
    {
        drops = dropTextureContainer.Drops;
    }

    public void Spawn(Vector2 position, DropType dropType, int amount)
    {
        var drop = drops[dropType];
        IDropable dropToSpawn = null;
        if (drop.Type == DropType.Health)
            dropToSpawn = new HealthDrop(drop, position);
        if(drop.Type == DropType.Energy)
        {
            dropToSpawn = new EnergyDrop(drop, position);
        }
        for(int i = 0; i < amount; i++) Globals.entityController.AddEntity(dropToSpawn);
    }
}