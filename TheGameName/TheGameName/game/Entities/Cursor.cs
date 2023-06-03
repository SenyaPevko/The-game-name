using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace TheGameName;

public class Cursor : IGameEntity
{
    public Texture2D Texture { get; private set; }
    public Vector2 Position { get; private set; }
    public Rectangle Rectangle { get; private set; }
    public UpdateOrder UpdateOrder { get; private set; } = UpdateOrder.Cursor;
    public DrawOrder DrawOrder { get; private set; } = DrawOrder.Cursor;


    public double Health => 0;

    public double Damage => 0;

    public bool IsAlive => true;

    public EntityType Type => EntityType.Cursor;

    private Vector2 cameraPreviousPosition;
    private Camera camera;

    private Vector2 mousePreviousPosition;

    public Cursor(Texture2D texture, Camera camera)
    {
        Texture = texture;
        SetPosition(camera);
        Position -= new Vector2(-Texture.Width, Texture.Height);
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); 
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
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void SetPosition(Camera camera)
    {
        var viewPort = TheGameName._GraphicsDevice.Viewport;
        var x = viewPort.X + (viewPort.Width / 2);
        var y = viewPort.Y + (viewPort.Height / 2);
        Position = new Vector2(x, y);
        cameraPreviousPosition = camera.Position;
        this.camera = camera;
        Mouse.SetPosition((int)Vector2.Transform(Position, camera.GetTransforamtion(TheGameName._GraphicsDevice)).X,
            (int)Vector2.Transform(Position, camera.GetTransforamtion(TheGameName._GraphicsDevice)).Y);
        mousePreviousPosition = Mouse.GetState().Position.ToVector2();
    }

    public void Draw(SpriteBatch spritebatch, GameTime gameTime)
    {
        spritebatch.Draw(Texture, Position, Color.AliceBlue);
    }

    public void Restart(Camera camera)
    {
        SetPosition(camera);
    }

    public void TakeDamage(double damage)
    {
        
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        
    }
}