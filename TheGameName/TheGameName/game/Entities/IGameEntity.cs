using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public interface IGameEntity: IUpdatable
{
    int UpdateOrder { get; }
    int DrawOrder { get; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch, GameTime gameTime);
}
