using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        private Player player;
        private PlayerController playerController;
        private PlayerTextureContainer playerTextureContainer;
        private Camera camera;
        private EnemySpawner enemySpawner;
        private SpriteFont font;
        private bool hasGameStarted = true;


        private Texture2D background;

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

            Globals.entityController = new EntityController();

            var mapData = File.ReadAllText($"{Content.RootDirectory}/map.txt");
            Globals.tileMap = new TileMap(mapData, Content);
            Globals.tileMap.BuildMap();

            Texture2D cursorTexture = Content.Load<Texture2D>("Cursor/cursor");
            Texture2D bulletTexture = Content.Load<Texture2D>("bullet/bullet");
            Texture2D bg = Content.Load<Texture2D>("toTest/bg");
            background = bg;

            Texture2D walkUpTexture = Content.Load<Texture2D>("Player/up");
            Texture2D walkUpLeftTexture = Content.Load<Texture2D>("Player/up-left");
            Texture2D walkUpRightTexture = Content.Load<Texture2D>("Player/up-right");
            Texture2D walkDownTexture = Content.Load<Texture2D>("Player/down");
            Texture2D walkDownLeftTexture = Content.Load<Texture2D>("Player/down-left");
            Texture2D walkDownRightTexture = Content.Load<Texture2D>("Player/down-right");
            Texture2D walkLeftTexture = Content.Load<Texture2D>("Player/left");
            Texture2D walkRightTexture = Content.Load<Texture2D>("Player/right");
            Texture2D enemyTexture = Content.Load<Texture2D>("Enemy/test");

            font = Content.Load<SpriteFont>("font");

            //Texture2D attackTexture = Content.Load<Texture2D>("Player/attack");

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

            player = new Player(this, container, new Vector2(100, 200));

            Globals.entityController.AddEntity(player);
            playerController = new PlayerController(player);
            camera = new Camera(player);
            Globals.cursor = new Cursor(cursorTexture);
            Globals.bulletsContoller = new BulletsContoller(bulletTexture);
            enemySpawner = new EnemySpawner(enemyTexture, new Vector2(100, 200), player);
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
            Globals.cursor.Update(gameTime);
            enemySpawner.SpawnEnemy(gameTime);
            if (!player.IsAlive)
            {
                Restart();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Wheat);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                  camera.get_transformation(GraphicsDevice));
            _spriteBatch.Draw(background, new Rectangle(0, 0, Globals.screenWidth, Globals.screenHeight), Color.White);

            Globals.tileMap.Draw(_spriteBatch, gameTime);

            Globals.entityController.Draw(_spriteBatch, gameTime);
            Globals.cursor.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
        }

        public void Restart()
        {
            player = new Player(this, playerTextureContainer, new Vector2(100, 200));
            hasGameStarted = false;
            Globals.entityController.AddEntity(player);
            playerController = new PlayerController(player);
            camera = new Camera(player);
            enemySpawner.ChangeTarget(player);
        }
    }
}