using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class EntityManager:IUpdatable
{
    private readonly List<IGameEntity> gameEntities = new List<IGameEntity>();
    private readonly List<IGameEntity> entitiesToAdd = new List<IGameEntity>();
    private readonly List<IGameEntity> entitiesToRemove = new List<IGameEntity>();

    public EntityManager()
    {

    }

    public bool HasEntity(IGameEntity entity) => gameEntities.Contains(entity) ||
        entitiesToAdd.Contains(entity) || entitiesToRemove.Contains(entity);

    public bool AddEntity(IGameEntity entity)
    {
        if (entity is null) throw new ArgumentNullException("U're trying to add an empty entity");
        if(HasEntity(entity)) return false;
        entitiesToAdd.Add(entity);
        return true;
    }

    public bool RemoveEntity(IGameEntity entity)
    {
        if (entity is null) throw new ArgumentNullException("U're trying to remove an empty entity");
        if (HasEntity(entity)) return false;
        entitiesToRemove.Add(entity);
        return true;
    }

    public void Update(GameTime gameTime)
    {
        foreach (var entity in gameEntities.OrderBy(entity => entity.UpdateOrder))
        {
            entity.Update(gameTime);
        }

        foreach(var entity in entitiesToAdd)
        {
            gameEntities.Add(entity);
        }

        foreach(var entity in entitiesToRemove)
        {
            gameEntities.Remove(entity);
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach(var entity in gameEntities.OrderBy(entity => entity.DrawOrder))
        {
            entity.Draw(spriteBatch, gameTime);
        }
    }
}