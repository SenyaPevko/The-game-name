using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TheGameName;

public class DropSpawner
{
    private Dictionary<DropType, Drop> drops;
    private List<IDropable> spawnedDrops;

    public DropSpawner(DropTextureContainer dropTextureContainer)
    {
        drops = dropTextureContainer.Drops;
        spawnedDrops = new List<IDropable>();
    }

    public void Spawn(Vector2 position, DropType dropType, int amount)
    {
        var drop = drops[dropType];
        IDropable dropToSpawn = null;
        if (drop.Type == DropType.Health)
            dropToSpawn = new HealthDrop(drop, position);
        if (drop.Type == DropType.Energy)
            dropToSpawn = new EnergyDrop(drop, position);
        if (drop.Type == DropType.Activator)
            dropToSpawn = new ActivatorDrop(drop, position);
        for (int i = 0; i < amount; i++)
        {
            TheGameName.EntityController.AddEntity(dropToSpawn);
            spawnedDrops.Add(dropToSpawn);
        }
    }

    public void Restart()
    {
        foreach(var drop in spawnedDrops)
        {
            drop.TakeDamage(drop.Health);
        }
        spawnedDrops.Clear();
    }
}