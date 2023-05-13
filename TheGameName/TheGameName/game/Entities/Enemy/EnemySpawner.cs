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
    private double enemySpawnedAmount;
    private EnemyTextureContainer textureContainer;
    private Player player;
    private double enemySpawnRate = 5000;
    private double enemySpawnTimer;
    private double enemyBossSpawnRate = 6000;
    private double enemyBossSpawnTimer;
    private int enemyAllowedAmount = 10;
    private ProgressBar healthBar;

    public Vector2 Position { get; private set; }
    public Order UpdateOrder => Order.Enemy;
    public Order DrawOrder => Order.Enemy;
    public double Health { get; private set; } = 1000;
    public double Damage { get; private set; } = 0;
    public bool IsAlive { get; private set; } = true;
    public Rectangle Rectangle { get; private set; }
    public EntityType Type => EntityType.Enemy;
    public Texture2D Texture;

    public EnemySpawner(Texture2D texture, EnemyTextureContainer textureContainer, Vector2 position, Player player, Texture2D healthBarFg, Texture2D healthBarBg)
    {
        this.textureContainer = textureContainer;
        Position = position;
        this.player = player;
        enemySpawnedAmount = 0;
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
        if (spawnedEnemies.Count >= enemyAllowedAmount) return;
        enemyBossSpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (enemyBossSpawnRate < enemyBossSpawnTimer)
        {
            var enemyBoss = new Enemy(Position, textureContainer.Boss, player, 100, 1);
            var bossHealthBar = new ProgressBar(this.healthBar.Background, this.healthBar.Foreground, 100,
                Position + new Vector2(-textureContainer.Boss.Width / 4, -textureContainer.Boss.Height / 2), enemyBoss);
            enemyBoss.SetHealthBar(bossHealthBar);
            Globals.entityController.AddEntity(enemyBoss);
            spawnedEnemies.Add(enemyBoss);
        }
        enemySpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (enemySpawnRate < enemySpawnTimer)
        {
            enemySpawnTimer = gameTime.ElapsedGameTime.Milliseconds;
            var enemy = new Enemy(Position, textureContainer.Minion, player, 8, 1);
            var healthBar = new ProgressBar(this.healthBar.Background, this.healthBar.Foreground, 8, 
                Position + new Vector2(-textureContainer.Minion.Width/4,-textureContainer.Minion.Height/2), enemy);
            enemy.SetHealthBar(healthBar);
            Globals.entityController.AddEntity(enemy);
            spawnedEnemies.Add(enemy);
            enemySpawnedAmount++;
        }
    }

    public void ChangeTarget(Player player)
    {
        this.player = player;
    }

    public void Restart()
    {
        enemySpawnTimer = 0;
        enemySpawnedAmount = 0;
        foreach(var enemy in spawnedEnemies) 
            Globals.entityController.RemoveEntity(enemy);
        spawnedEnemies.Clear();
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        spriteBatch.Draw(Texture, Position, null, Color.Aqua, 0,
            new Vector2(Texture.Width / 2, Texture.Height / 2), 1.0f, SpriteEffects.None, 0f);
        healthBar.Draw(spriteBatch, gameTime);
    }

    public void TakeDamage(double damage)
    {
        Health -= damage;
        if (Health <= 0) IsAlive = false;
    }

    public void Collide(IGameEntity entity, IGameEntity entityToIntersect)
    {
        
    }
}