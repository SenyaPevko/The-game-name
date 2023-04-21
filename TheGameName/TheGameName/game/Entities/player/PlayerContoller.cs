using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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
                player.Move(new Vector2(-1, 1), nameof(PlayerTextureContainer.WalkDownLeft));
            else if (currentKeyboardState.IsKeyDown(Keys.W))
                player.Move(new Vector2(-1, -1), nameof(PlayerTextureContainer.WalkUpLeft));
            else player.Move(new Vector2(-1, 0), nameof(PlayerTextureContainer.WalkLeft));
        }
        else if (currentKeyboardState.IsKeyDown(Keys.D))
        {
            if (currentKeyboardState.IsKeyDown(Keys.S))
                player.Move(new Vector2(1, 1), nameof(PlayerTextureContainer.WalkDownRight));
            else if (currentKeyboardState.IsKeyDown(Keys.W))
                player.Move(new Vector2(1, -1), nameof(PlayerTextureContainer.WalkUpRight));
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
            player.shootDelayTimer = 0;
        }
        if (currentKeyboardState.IsKeyDown(Keys.C))
        {
            player.Reload();
        }
    }
}