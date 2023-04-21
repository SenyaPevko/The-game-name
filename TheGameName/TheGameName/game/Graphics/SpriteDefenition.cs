using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;
public struct SpriteDefinition
{
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public SpriteDefinition(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

}