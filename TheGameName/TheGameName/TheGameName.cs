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
using System.Runtime.CompilerServices;
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
        private Vector2 playerSpawnPosition = new(520, 920);
        private PlayerController playerController;
        private PlayerTextureContainer playerTextureContainer;
        private Camera camera;
        private List<EnemySpawner> enemySpawners = new();
        private bool hasGameStarted = false;
        private bool isActive = true;
        private bool hasGameEnded = false;
        private Texture2D startScreen;
        private Texture2D pauseScreen;
        private Texture2D endScreen;
        private Portal playerSpawn;

        public static int ScreenHeight { get; private set; } = 704;
        public static int ScreenWidth { get; private set; } = 704;
        public static GraphicsDevice _GraphicsDevice { get; private set; }
        public static BulletsContoller BulletsContoller { get; private set; }
        public static EntityController EntityController { get; set; }
        public static TileMap TileMap { get; private set; }
        public static DropSpawner DropSpawner { get; private set; }
        public static Inventory Inventory { get; private set; }
        public static Cursor WorldСursor { get; private set; }
        public static SpriteFont FontThin { get; private set; }

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
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            IsMouseVisible = false;
            _GraphicsDevice = GraphicsDevice;
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _GraphicsDevice = GraphicsDevice;
            EntityController = new EntityController();

            var mapData = File.ReadAllText($"{Content.RootDirectory}/map2.txt");
            TileMap = new TileMap(mapData, Content);

            startScreen = Content.Load<Texture2D>("Screens/StartScreen");
            pauseScreen = Content.Load<Texture2D>("Screens/PauseScreen");
            endScreen = Content.Load<Texture2D>("Screens/EndScreen");

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
            var activatorsMessage = Content.Load<Texture2D>("Portal/ActivatorsMessage");
            var spawnTextureContainer = new PortalTextureContainer
            {
                Stage0 = portalStage0Texture,
                Stage1 = portalStage1Texture,
                Stage2 = portalStage2Texture,
                BarBg = portalBarBg,
                BarFg = portalBarFg,
                EnergyMessage = energyMessage,
                ActivatatorsMessage = activatorsMessage
            };
            playerSpawn = new Portal(spawnTextureContainer, playerSpawnPosition + new Vector2(20, 0), 15, enemySpawners.Count);
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
            var activatorDrop = Content.Load<Texture2D>("Drop/activator");
            var dropTextureContainer = new DropTextureContainer
            {
                HealthDrop = new Drop(DropType.Health, healthDrop),
                EnergyDrop = new Drop(DropType.Energy, energyDrop),
                ActivatorDrop = new Drop(DropType.Activator, activatorDrop)
            };
            DropSpawner = new DropSpawner(dropTextureContainer);

            var energyInventory = Content.Load<Texture2D>("inventory/energy");
            var activatorInventory = Content.Load<Texture2D>("inventory/EnemyHead");
            var inventoryTextureContainer = new InventoryTextureContainer
            {
                Energy = energyInventory,
                Activator = activatorInventory
            };

            FontThin = Content.Load<SpriteFont>("font");

            PlayerTextureContainer container = new()
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
            Inventory = new Inventory(inventoryTextureContainer,
                camera.Position - new Vector2(ScreenWidth / 2, -ScreenHeight / 2 + 50), camera);
            EntityController.AddEntity(Inventory);
            playerHealthBar = new ProgressBar(playerHealthBarBg, playerHealthBarFg, playerHealth,
                camera.Position - new Vector2(ScreenWidth / 2, ScreenHeight / 2), camera);
            EntityController.AddEntity(player);
            EntityController.AddEntity(playerSpawn);
            playerController = new PlayerController(player);
            WorldСursor = new Cursor(cursorTexture, camera);
            EntityController.AddEntity(WorldСursor);
            BulletsContoller = new BulletsContoller(bulletTexture);
            enemySpawners.Add(new EnemySpawner(enemySpawnerTexture, enemyTextureContainer,
                new Vector2(128, 128), player, enemyHealthBarFg, enemyHealthBarBg));
            enemySpawners.Add(new EnemySpawner(enemySpawnerTexture, enemyTextureContainer,
                new Vector2(96, 864), player, enemyHealthBarFg, enemyHealthBarBg));
            foreach (var enemySpawner in enemySpawners)
                EntityController.AddEntity(enemySpawner);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!isActive)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    Exit();
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    isActive = true;
                return;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                isActive = false;
                return;
            }
            if (!hasGameStarted||hasGameEnded)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    hasGameStarted = true;
                    isActive = true;
                    hasGameEnded = false;
                }
                return;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);

            camera.Update(gameTime);
            EntityController.Update(gameTime);
            playerController.Update(gameTime);
            playerHealthBar.Update(player.Health);
            if (!player.IsAlive)
            {
                Restart();
            }
            EndGame();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            if (!isActive)
            {
                DrawScreen(pauseScreen);
                return;
            }
            if (!hasGameStarted)
            {
                DrawScreen(startScreen);
                return;
            }
            if (hasGameEnded)
            {
                DrawScreen(endScreen);
                return;
            }
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                  camera.GetTransforamtion(GraphicsDevice));
            TileMap.Draw(_spriteBatch, gameTime);
            EntityController.Draw(_spriteBatch, gameTime);
            playerHealthBar.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
        }

        public void Restart()
        {
            player = new Player(playerTextureContainer, playerSpawnPosition, playerHealth, playerShootingBar);
            hasGameStarted = false;
            EntityController.AddEntity(player);
            playerController = new PlayerController(player);
            camera = new Camera(player);
            foreach (var enemySpawner in enemySpawners)
            {
                enemySpawner.Restart();
                enemySpawner.ChangeTarget(player);
                EntityController.AddEntity(enemySpawner);
            }
            playerHealthBar = new ProgressBar(playerHealthBar.Background, playerHealthBar.Foreground, playerHealth,
                camera.Position - new Vector2(ScreenWidth / 2, ScreenHeight / 2), camera);
            WorldСursor.Restart(camera);
            Inventory.Restart(camera);
            DropSpawner.Restart();
            playerSpawn.Restart();
        }

        public void EndGame()
        {
            if (playerSpawn.IsActivated)
            {
                hasGameEnded = true;
                player.TakeDamage(player.Health);
            }
        }

        public void DrawScreen(Texture2D screen)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(screen, new Rectangle(0, 0, ScreenWidth, ScreenHeight),
                Color.White);
            _spriteBatch.End();
            return;
        }
    }
}