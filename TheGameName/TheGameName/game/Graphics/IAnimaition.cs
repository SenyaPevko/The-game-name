using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;
public interface IAnimation
{
    void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float opacity, Color color);
    void Update(GameTime gameTime);

    void Stop();
    void Play();
    void Pause();
}