using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class RenderStateController: IUpdatable
{
    private readonly Dictionary<string, RenderState> states = new Dictionary<string, RenderState>();
    public RenderState CurrentState { get; private set; } = null;

    public RenderStateController()
    {
    }

    public RenderState GetState(string name) => states[name];

    public void SetState(string name)
    {
        CurrentState = GetState(name);
    }

    public RenderState AddState(string name, IAnimation animation)
    {
        var state = new RenderState(name, animation);
        states[name] = state;
        return state;
    }

    public void RemoveState(RenderState state)
    {
        states.Remove(state.Name);

        if (CurrentState == state)
            CurrentState = null;
    }

    public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
    {
        CurrentState.Animation.Draw(spriteBatch, position, spriteEffects);
    }

    public void Update(GameTime gameTime)
    {
        CurrentState.Animation.Update(gameTime);
    }
}