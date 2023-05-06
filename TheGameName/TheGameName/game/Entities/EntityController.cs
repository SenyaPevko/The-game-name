﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheGameName;

public class EntityController : IUpdatable
{
    private readonly List<IGameEntity> gameEntities = new List<IGameEntity>();
    private readonly List<IGameEntity> entitiesToAdd = new List<IGameEntity>();
    private readonly List<IGameEntity> entitiesToRemove = new List<IGameEntity>();

    public EntityController()
    {

    }

    public bool HasEntity(IGameEntity entity) => gameEntities.Contains(entity) ||
        entitiesToAdd.Contains(entity) || entitiesToRemove.Contains(entity);

    public bool AddEntity(IGameEntity entity)
    {
        if (entity is null) throw new ArgumentNullException("U're trying to add an empty entity");
        if (HasEntity(entity)) return false;
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
            // entities with tiles collision
            foreach (Tile tile in Globals.tileMap)
            {
                if (Collide(tile.Rectangle, entity.Rectangle))
                {
                    entity.Collide(entity, tile);
                }
            }

            foreach (var entityToIntersect in gameEntities)
            {
                if (entity == entityToIntersect) continue;
                // entities colission
                if(Collide(entity.Rectangle, entityToIntersect.Rectangle))
                {
                    entityToIntersect.Collide(entityToIntersect, entity);
                    entity.Collide(entity, entityToIntersect);
                }
            }
            
            if (entity.IsAlive)
                entity.Update(gameTime);
            else entitiesToRemove.Add(entity);
        }

        foreach (var entity in entitiesToAdd)
        {
            gameEntities.Add(entity);
        }

        foreach (var entity in entitiesToRemove)
        {
            gameEntities.Remove(entity);
        }
        entitiesToAdd.Clear();
        entitiesToRemove.Clear();
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (var entity in gameEntities.OrderBy(entity => entity.DrawOrder))
        {
            entity.Draw(spriteBatch, gameTime);
        }
    }

    public bool Collide(Rectangle entity, Rectangle entityToIntersect)
    {
        
        if (entity.Intersects(entityToIntersect)) 
            return true;
        return false;
    }
}