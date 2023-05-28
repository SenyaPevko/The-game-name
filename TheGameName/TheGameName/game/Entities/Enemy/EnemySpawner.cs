using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class EnemySpawner: IGameEntity
{
    private List<IGameEntity> spawnedEnemies = new List<IGameEntity>();
    private EnemyTextureContainer textureContainer;
    private Player player;
    private double enemySpawnRate = 7000;
    private double enemySpawnTimer;
    private double enemyBossSpawnRate = 40000;
    private double enemyBossSpawnTimer;
    private ProgressBar healthBar;

    public Vector2 Position { get; private set; }
    public UpdateOrder UpdateOrder => UpdateOrder.Enemy;
    public DrawOrder DrawOrder => DrawOrder.Enemy;
    public double Health { get; private set; } = 400;
    public double Damage { get; private set; } = 0.01;
    public bool IsAlive { get; private set; } = true;
    public Rectangle Rectangle { get; private set; }
    public EntityType Type => EntityType.Enemy;
    public Texture2D Texture;

    public EnemySpawner(Texture2D texture, EnemyTextureContainer textureContainer, Vector2 position, Player player, Texture2D healthBarFg, Texture2D healthBarBg)
    {
        this.textureContainer = textureContainer;
        Position = position;
        this.player = player;
        healthBar = new ProgressBar(healthBarBg, healthBarFg, Health, Position, this);
        Texture = texture;
        Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
    }

    public void Update(GameTime gameTime)
    {
        SpawnEnemy(gameTime);
        healthBar.Update(Health);
    }

    public void SpawnEnemy(GameTime gameTime)
    {
        
        enemyBossSpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (enemyBossSpawnRate < enemyBossSpawnTimer)
        {
            enemyBossSpawnTimer = gameTime.ElapsedGameTime.Milliseconds;
            var enemyBoss = new Enemy(Position, textureContainer.Boss, player, 100);
            var bossHealthBar = new ProgressBar(healthBar.Background, healthBar.Foreground, 100,
                Position + new Vector2(-textureContainer.Boss.Width / 4, -textureContainer.Boss.Height / 2), enemyBoss);
            enemyBoss.SetHealthBar(bossHealthBar);
            TheGameName.EntityController.AddEntity(enemyBoss);
            spawnedEnemies.Add(enemyBoss);
        }
        enemySpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (enemySpawnRate < enemySpawnTimer)
        {
            enemySpawnTimer = gameTime.ElapsedGameTime.Milliseconds;
            var enemy = new Enemy(Position, textureContainer.Minion, player, 12);
            var healthBar = new ProgressBar(this.healthBar.Background, this.healthBar.Foreground, 8, 
                Position + new Vector2(-textureContainer.Minion.Width/4,-textureContainer.Minion.Height/2), enemy);
            enemy.SetHealthBar(healthBar);
            TheGameName.EntityController.AddEntity(enemy);
            spawnedEnemies.Add(enemy);
        }
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