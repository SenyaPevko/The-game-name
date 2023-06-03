using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TheGameName;

public class PlayerController : IUpdatable
{
    private readonly Player player;
    public string angle;

    public PlayerController(Player player)
    {
        this.player = player;
    }

    public void Update(GameTime gameTime)
    {
        ProcessInput(gameTime);
    }

    public void ProcessInput(GameTime gameTime)
    {
        var currentKeyboardState = Keyboard.GetState();
        var currentMouseState = Mouse.GetState();

        if (currentKeyboardState.IsKeyDown(Keys.A))
        {
            if (currentKeyboardState.IsKeyDown(Keys.S))
                player.Move(new Vector2(-(float)Math.Sqrt(2)/2, (float)Math.Sqrt(2) / 2), nameof(PlayerTextureContainer.WalkDownLeft));
            else if (currentKeyboardState.IsKeyDown(Keys.W))
                player.Move(new Vector2(-(float)Math.Sqrt(2) / 2, -(float)Math.Sqrt(2) / 2), nameof(PlayerTextureContainer.WalkUpLeft));
            else player.Move(new Vector2(-1, 0), nameof(PlayerTextureContainer.WalkLeft));
        }
        else if (currentKeyboardState.IsKeyDown(Keys.D))
        {
            if (currentKeyboardState.IsKeyDown(Keys.S))
                player.Move(new Vector2((float)Math.Sqrt(2) / 2, (float)Math.Sqrt(2) / 2), nameof(PlayerTextureContainer.WalkDownRight));
            else if (currentKeyboardState.IsKeyDown(Keys.W))
                player.Move(new Vector2((float)Math.Sqrt(2) / 2, -(float)Math.Sqrt(2) / 2), nameof(PlayerTextureContainer.WalkUpRight));
            else player.Move(new Vector2(1, 0), nameof(PlayerTextureContainer.WalkRight));
        }
        else if (currentKeyboardState.IsKeyDown(Keys.S))
        {
            player.Move(new Vector2(0, 1), nameof(PlayerTextureContainer.WalkDown));
        }
        else if (currentKeyboardState.IsKeyDown(Keys.W))
        {
            player.Move(new Vector2(0, -1), nameof(PlayerTextureContainer.WalkUp));
        }
        else player.Halt();

        if (currentMouseState.LeftButton == ButtonState.Pressed)
        {
            player.Attack(gameTime);
        }
        if (currentMouseState.LeftButton == ButtonState.Released)
        {
            player.AttackDelayTimer = 0;
        }
        if (currentKeyboardState.IsKeyDown(Keys.R))
        {
            player.Reload(gameTime);
        }
        if (currentKeyboardState.IsKeyDown(Keys.Q))
        {
            player.Drop(1, gameTime);
        }
        if (currentKeyboardState.IsKeyDown(Keys.LeftShift))
        {
            player.Dodge(gameTime);
        }
    }
}