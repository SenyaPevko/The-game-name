using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;


public class RenderState
{

    public string Name { get; }
    public IAnimation Animation { get; }

    public RenderState(string name, IAnimation animation)
    {
        Name = name;
        Animation = animation;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float opacity, Color color)
    {
        Animation?.Draw(spriteBatch, position, spriteEffects, opacity, color);
    }

}