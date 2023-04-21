﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net.Http.Headers;
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
        private Camera camera;

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

            player = new Player(this, container)
            {
                Position = new Vector2(150, 100)
            };

            playerController = new PlayerController(player);
            camera = new Camera(player);
            Globals.cursor = new Cursor(cursorTexture);
            Globals.bulletsContoller = new BulletsContoller(bulletTexture);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);

            player.Update(gameTime);
            playerController.Update(gameTime);
            camera.Update(gameTime);
            Globals.cursor.Update(gameTime);
            Globals.bulletsContoller.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Wheat);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                  camera.get_transformation(GraphicsDevice));

            _spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);

            player.Draw(_spriteBatch, gameTime);
            Globals.bulletsContoller.Draw(_spriteBatch, gameTime);
            Globals.cursor.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
        }
    }
}