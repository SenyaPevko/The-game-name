using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGameName;

public enum TileCollision
{
    Passable = 0,
    Impassable = 1,
    Attackable = 2,
    Breakable = 3,
    Walkable = 4
}

public class Tile: IGameEntity
{
    public Texture2D Texture { get; private set; }
    public TileCollision Collision { get; private set; }
    public UpdateOrder UpdateOrder { get; private set; } = UpdateOrder.Tile;
    public DrawOrder DrawOrder { get; private set; } = DrawOrder.Tile;
    public Rectangle Rectangle { get; private set; }
    public Vector2 Position { get; private set; }
    public double Health { get; private set; } = 100;
    public double Damage { get; private set; } = 0;
    public bool IsAlive { get; private set; } = true;
    public EntityType Type { get; private set; } = EntityType.Tile;
    public Rectangle TileRectangleInTileset { get; private set; }

    public Tile(Texture2D texture, TileCollision collision, Rectangle rectangle, Vector2 position, Rectangle tilesetRectangle)
    {
        Texture = texture;
        Collision = collision;
        Rectangle = rectangle;
        Position = position;
        TileRectangleInTileset = tilesetRectangle;
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        
    }

    public void Update(GameTime gameTime)
    {
        
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, TileRectangleInTileset, Color.White);
    }

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) IsAlive = false;
    }
}