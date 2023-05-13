using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using System;
using TheGameName;

public class Cursor : IComponent
{
    public Texture2D Texture { get; private set; }
    public Vector2 Scale { get; private set; }
    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public Order UpdateOrder { get; private set; } = Order.Cursor;
    public Order DrawOrder { get; private set; } = Order.Cursor;

    private Vector2 cameraPreviousPosition;
    private Camera camera;

    private Vector2 mousePreviousPosition;

    public Cursor(Texture2D texture, Camera camera)
    {
        Texture = texture;
        Scale = new Vector2(0, 0);
        var viewPort = Globals.graphicsDevice.Viewport;
        var x = viewPort.X + (viewPort.Width / 2);
        var y = viewPort.Y + (viewPort.Height / 2);
        Position = new Vector2(x,y);
        cameraPreviousPosition = camera.Position;
        this.camera = camera;
        Mouse.SetPosition((int)Vector2.Transform(Position, camera.GetTransforamtion(Globals.graphicsDevice)).X, 
            (int)Vector2.Transform(Position, camera.GetTransforamtion(Globals.graphicsDevice)).Y);
        mousePreviousPosition = Mouse.GetState().Position.ToVector2();
    }

    public void Update(GameTime gameTime)
    {
        var cameraCoordinatesChange = camera.Position - cameraPreviousPosition;
        Position += cameraCoordinatesChange;
        cameraPreviousPosition = camera.Position;
        var mouse = Mouse.GetState();
        var mouseChagePosition = mouse.Position.ToVector2() - mousePreviousPosition;
        mousePreviousPosition = mouse.Position.ToVector2();
        Position += mouseChagePosition;
        
    }

    public void Draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        spritebatch.Draw(Texture, Position, Color.AliceBlue);
    }
}