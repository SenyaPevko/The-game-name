using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class Unit : IGameEntity
{
    private readonly RenderStateController renderStateController = new RenderStateController();

    public string Name { get; set; }
    public int Health { get; set; }
    public Vector2 Position { get; set; }
    public int UpdateOrder { get; set; }
    public int DrawOrder { get; set; }
    public float Speed { get; set; }
    public string CurrentState { get; set; } = nameof(PlayerTextureContainer.WalkUp);

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        renderStateController.Draw(spriteBatch, Position, SpriteEffects.None);
    }

    public void Update(GameTime gameTime)
    {
        renderStateController.Update(gameTime);
    }

    public void Move(Vector2 direction, string state)
    {
        CurrentState = state;
        renderStateController.SetState(state);
        renderStateController.CurrentState.Animation.Play();
        Position += direction * Speed;
    }

    public void Halt()
    {
        renderStateController.SetState(CurrentState);
        renderStateController.CurrentState.Animation.Play();
        renderStateController.CurrentState.Animation.Pause();
    }
}
