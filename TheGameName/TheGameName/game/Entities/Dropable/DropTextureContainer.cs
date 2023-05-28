using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;
public class DropTextureContainer
{
    public Dictionary<DropType, Drop> Drops { get; private set; } = new Dictionary<DropType, Drop>();
    public Drop HealthDrop 
    { 
        get { return health; } 
        set 
        {
            health = new Drop(DropType.Health, value.Texture);
            Drops.Add(health.Type, health);
        } 
    }
    private Drop health;

    public Drop EnergyDrop
    {
        get { return energy; }
        set
        {
            energy = new Drop(DropType.Energy, value.Texture);
            Drops.Add(energy.Type, energy);
        }
    }
    private Drop energy;

    public Drop ActivatorDrop
    {
        get { return activator; }
        set
        {
            activator = new Drop(DropType.Activator, value.Texture);
            Drops.Add(activator.Type, activator);
        }
    }
    private Drop activator;
}

public struct Drop
{
    public DropType Type { get; private set; }
    public Texture2D Texture { get; private set; }

    public Drop(DropType type, Texture2D texture)
    {
        Type = type;
        Texture = texture;
    }
}