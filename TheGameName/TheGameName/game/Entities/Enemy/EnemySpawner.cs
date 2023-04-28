using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameName;

public class EnemySpawner
{
    private double enemySpawnedAmount;
    private Texture2D enemyTexture;
    private Player player;
    private double enemySpawnRate = 5000;
    private double enemySpawnTimer;
    private int enemyAllowedAmount = 10;
    private Vector2 Position { get; set; }
    public EnemySpawner(Texture2D enemyTexture, Vector2 position, Player player)
    {
        this.enemyTexture = enemyTexture;
        Position = position;
        this.player = player;
        enemySpawnedAmount = 0;
    }

    public void SpawnEnemy(GameTime gameTime)
    {
        if (enemySpawnedAmount >= enemyAllowedAmount) return;
        enemySpawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        if (enemySpawnRate < enemySpawnTimer)
        {
            enemySpawnTimer = gameTime.ElapsedGameTime.Milliseconds;
            var enemy = new Enemy(Position, enemyTexture, player);
            Globals.entityController.AddEntity(enemy);
        }
    }
}