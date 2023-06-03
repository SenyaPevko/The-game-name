using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TheGameName;

public class EnemySpawner: IGameEntity
{
    private HashSet<IGameEntity> spawnedEnemies = new();
    private Dictionary<EnemyType, EnemyTextureContainer> enemies;
    private Player player;
    private double enemySpawnRate = 4500;
    private double enemySpawnTimer;
    private double enemyBossSpawnRate = 41379;
    private double enemyBossSpawnTimer;
    private ProgressBar healthBar;

    public Vector2 Position { get; private set; }
    public UpdateOrder UpdateOrder => UpdateOrder.Enemy;
    public DrawOrder DrawOrder => DrawOrder.Enemy;
    public double Health { get; private set; } = 220;
    public double Damage { get; private set; } = 0.01;
    public bool IsAlive { get; private set; } = true;
    public Rectangle Rectangle { get; private set; }
    public EntityType Type => EntityType.Enemy;
    public Texture2D Texture;

    public EnemySpawner(Texture2D texture, Dictionary<EnemyType, EnemyTextureContainer> enemies, Vector2 position, Player player, Texture2D healthBarFg, Texture2D healthBarBg)
    {
        this.enemies = enemies;
        Position = position;
        this.player = player;
        healthBar = new ProgressBar(healthBarBg, healthBarFg, Health, Position, this);
        Texture = texture;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        healthBar.Update(Health);
        enemyBossSpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (enemyBossSpawnRate < enemyBossSpawnTimer)
        {
            enemyBossSpawnTimer = gameTime.ElapsedGameTime.Milliseconds;
            SpawnEnemy(gameTime, enemies[EnemyType.GhostBoss], 100);
        }
            
        enemySpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (enemySpawnRate < enemySpawnTimer)
        {
            enemySpawnTimer = gameTime.ElapsedGameTime.Milliseconds;
            SpawnEnemy(gameTime, enemies[EnemyType.GhostMinion], 12);
        }
            
    }

    public void SpawnEnemy(GameTime gameTime, EnemyTextureContainer textureContainer, double health)
    {
        var enemy = new Enemy(Position, textureContainer, player, health);
        TheGameName.EntityController.AddEntity(enemy);
        spawnedEnemies.Add(enemy);
        enemy.SetProtection(gameTime);
    }

    public void ChangeTarget(Player player)
    {
        this.player = player;
    }

    public void Restart()
    {
        enemySpawnTimer = 0;
        foreach(var enemy in spawnedEnemies) 
            TheGameName.EntityController.RemoveEntity(enemy);
        spawnedEnemies.Clear();
        Health = healthBar.MaxValue;
        IsAlive = true;
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.White, 0,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        healthBar.Draw(spriteBatch, gameTime);
    }

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            IsAlive = false;
            TheGameName.DropSpawner.Spawn(Position, DropType.Activator, 1);
            var enemy = new Enemy(Position, enemies[EnemyType.LastGhost], player, 120);
            TheGameName.EntityController.AddEntity(enemy);
            spawnedEnemies.Add(enemy);
        }
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        if(entityToIntersect.Type == EntityType.Player)
        {
            entityToIntersect.TakeDamage(Damage);
        }
    }
}