using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;
public class Camera: IUpdatable
{
    public Matrix Transform;
    public float Rotation { get; set; }
    public Vector2 Position { get; set; }
    public Player Player { get; set; }

    public Camera(Player player)
    {
        Position = Vector2.Zero;
        Player = player;
    }

    public void Move(Vector2 amount)
    {
        Position += amount;
    }

    public Matrix get_transformation(GraphicsDevice graphicsDevice)
    {
        Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0))*
            Matrix.CreateRotationZ(Rotation)
            * Matrix.CreateTranslation(
                new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
        return Transform;
    }

    public void Update(GameTime gameTime)
    {
        Position = Player.Position;
    }
}

