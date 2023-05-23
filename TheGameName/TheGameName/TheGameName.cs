using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Authentication.ExtendedProtection;
using static System.Net.Mime.MediaTypeNames;

namespace TheGameName
{
    public class TheGameName : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private ProgressBar playerHealthBar;
        private ProgressBar playerShootingBar;
        private double playerHealth = 30;
        private Player player;
        private Vector2 playerSpawnPosition = new Vector2(520, 920);
        private PlayerController playerController;
        private PlayerTextureContainer playerTextureContainer;
        private Camera camera;
        private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
        private bool hasGameStarted = false;
        private Texture2D StartScreen;
        private Portal playerSpawn;

        public TheGameName()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            _graphics.PreferredBackBufferWidth = Globals.screenWidth;
            _graphics.PreferredBackBufferHeight = Globals.screenHeight;
            IsMouseVisible = false;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.graphicsDevice = GraphicsDevice;
            Globals.entityController = new EntityController();

            var mapData = File.ReadAllText($"{Content.RootDirectory}/map2.txt");
            Globals.tileMap = new TileMap(mapData, Content);
            Globals.tileMap.BuildMap();

            StartScreen = Content.Load<Texture2D>("Screens/StartScreen");

            var walkUpTexture = Content.Load<Texture2D>("Player/up");
            var walkUpLeftTexture = Content.Load<Texture2D>("Player/up-left");
            var walkUpRightTexture = Content.Load<Texture2D>("Player/up-right");
            var walkDownTexture = Content.Load<Texture2D>("Player/down");
            var walkDownLeftTexture = Content.Load<Texture2D>("Player/down-left");
            var walkDownRightTexture = Content.Load<Texture2D>("Player/down-right");
            var walkLeftTexture = Content.Load<Texture2D>("Player/left");
            var walkRightTexture = Content.Load<Texture2D>("Player/right");

            var portalStage0Texture = Content.Load<Texture2D>("Portal/PortalStage0");
            var portalStage1Texture = Content.Load<Texture2D>("Portal/PortalStage1");
            var portalStage2Texture = Content.Load<Texture2D>("Portal/PortalStage2");
            var portalBarBg = Content.Load<Texture2D>("Portal/PortalBar_Bg");
            var portalBarFg = Content.Load<Texture2D>("Portal/PortalBar_Fg");
            var energyMessage = Content.Load<Texture2D>("Portal/EnergyMessage");
            var spawnTextureContainer = new PortalTextureContainer
            {
                Stage0 = portalStage0Texture,
                Stage1 = portalStage1Texture,
                Stage2 = portalStage2Texture,
                BarBg = portalBarBg,
                BarFg = portalBarFg,
                EnergyMessage = energyMessage
            };
            playerSpawn = new Portal(spawnTextureContainer, playerSpawnPosition + new Vector2(12,0),100);
            var cursorTexture = Content.Load<Texture2D>("Cursor/cursor");
            var bulletTexture = Content.Load<Texture2D>("bullet/bullet");
            
            var playerHealthBarBg = Content.Load<Texture2D>("Bars/HealthBar_Player_Dying");
            var playerHealthBarFg = Content.Load<Texture2D>("Bars/HealthBar_Player_Alive");
            var playerShootingBarFg = Content.Load<Texture2D>("Bars/ShootingBar_Fg");
            var playerShootingBarBg = Content.Load<Texture2D>("Bars/ShootingBar_Bg");
            
            var enemyHealthBarFg = Content.Load<Texture2D>("Bars/HealthBar_Enemy_Fg");
            var enemyHealthBarBg = Content.Load<Texture2D>("Bars/HealthBar_Enemy_Bg");

            var enemyMinionTexture = Content.Load<Texture2D>("Enemy/test");
            var enemyBossTexture = Content.Load<Texture2D>("Enemy/slug_boss");
            var enemySpawnerTexture = Content.Load<Texture2D>("Enemy/EnemySpawner");

            var enemyTextureContainer = new EnemyTextureContainer
            {
                Minion = enemyMinionTexture,
                Boss = enemyBossTexture
            };

            var healthDrop = Content.Load<Texture2D>("Drop/health");
            var energyDrop = Content.Load<Texture2D>("Drop/energy");
            var dropTextureContainer = new DropTextureContainer
            {
                HealthDrop = new Drop(DropType.Health, healthDrop),
                EnergyDrop = new Drop(DropType.Energy, energyDrop)
            };
            Globals.dropSpawner = new DropSpawner(dropTextureContainer);

            var energyInventory = Content.Load<Texture2D>("inventory/energy");
            var inventoryTextureContainer = new InventoryTextureContainer
            {
                Energy = energyInventory
            };
            
            Globals.fontThin = Content.Load<SpriteFont>("font");

            PlayerTextureContainer container = new PlayerTextureContainer
            {
                WalkUp = walkUpTexture,
                WalkUpLeft = walkUpLeftTexture,
                WalkUpRight = walkUpRightTexture,
                WalkDown = walkDownTexture,
                WalkDownLeft = walkDownLeftTexture,
                WalkDownRight = walkDownRightTexture,
                WalkLeft = walkLeftTexture,
                WalkRight = walkRightTexture
            };
            playerTextureContainer = container;
            playerShootingBar = new ProgressBar(playerShootingBarBg, playerShootingBarFg, 0, Vector2.Zero, null);
            player = new Player(container, playerSpawnPosition, playerHealth, playerShootingBar);
            camera = new Camera(player);
            Globals.inventory = new Inventory(inventoryTextureContainer, 
                camera.Position - new Vector2(Globals.screenWidth/2, -Globals.screenHeight/2+50), camera);
            playerHealthBar = new ProgressBar(playerHealthBarBg, playerHealthBarFg, playerHealth, 
                camera.Position - new Vector2(Globals.screenWidth/2, Globals.screenHeight/2), camera);
            Globals.entityController.AddEntity(player);
            Globals.entityController.AddEntity(playerSpawn);
            playerController = new PlayerController(player);
            Globals.cursor = new Cursor(cursorTexture, camera);
            Globals.entityController.AddEntity(Globals.cursor);
            Globals.bulletsContoller = new BulletsContoller(bulletTexture);
            enemySpawners.Add(new EnemySpawner(enemySpawnerTexture, enemyTextureContainer,
                new Vector2(128, 128), player, enemyHealthBarFg, enemyHealthBarBg));
            enemySpawners.Add(new EnemySpawner(enemySpawnerTexture, enemyTextureContainer,
                new Vector2(96, 864), player, enemyHealthBarFg, enemyHealthBarBg));
            foreach(var enemySpawner in enemySpawners)
                Globals.entityController.AddEntity(enemySpawner);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) 
                hasGameStarted = true;
            if (!hasGameStarted) return;

            // TODO: Add your update logic here

            base.Update(gameTime);

            Globals.entityController.Update(gameTime);
            playerController.Update(gameTime);
            camera.Update(gameTime);
            playerHealthBar.Update(player.Health);
            Globals.inventory.Update(gameTime);
            if (!player.IsAlive)
            {
                Restart();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            if (!hasGameStarted)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(StartScreen, new Rectangle(0, 0, Globals.screenWidth, Globals.screenHeight), Color.White);
                _spriteBatch.End();
                return;
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                  camera.GetTransforamtion(GraphicsDevice));

            
            Globals.tileMap.Draw(_spriteBatch, gameTime);
            Globals.entityController.Draw(_spriteBatch, gameTime);
            Globals.inventory.Draw(_spriteBatch, gameTime);
            playerHealthBar.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
        }

        public void Restart()
        {
            player = new Player(playerTextureContainer, playerSpawnPosition, playerHealth, playerShootingBar);
            hasGameStarted = false;
            Globals.entityController.AddEntity(player);
            playerController = new PlayerController(player);
            camera = new Camera(player);
            foreach (var enemySpawner in enemySpawners)
            {
                enemySpawner.Restart();
                enemySpawner.ChangeTarget(player);
                Globals.entityController.AddEntity(enemySpawner);
            }
            playerHealthBar = new ProgressBar(playerHealthBar.Background, playerHealthBar.Foreground, playerHealth,
                camera.Position - new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2), camera);
            Globals.cursor.Restart(camera);
            Globals.inventory.Restart(camera);
        }
    }
}