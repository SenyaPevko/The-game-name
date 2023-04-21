using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class Sprite
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Texture2D Texture { get; set; }
    public int TexturePosX { get; set; }
    public int TexturePosY { get; set; }
    public Rectangle SpriteBox { get => new Rectangle(TexturePosX, TexturePosY, Width, Height); }
    public Vector2 Origin { get; set; }

    public Sprite(int width, int height, Texture2D texture, int texturePosX, int texturePosY)
    {
        Width = width;
        Height = height;
        Texture = texture;
        TexturePosX = texturePosX;
        TexturePosY = texturePosY;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {

        spriteBatch.Draw(Texture, position, SpriteBox, Color.White, 0, Origin, Vector2.One, SpriteEffects.None, 0);
    }
}