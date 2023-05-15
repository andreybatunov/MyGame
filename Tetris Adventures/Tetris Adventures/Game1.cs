using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TiledSharp;

namespace Tetris_Adventures
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        #region TetrisManager
        private TetrisManager tetris;
        private Texture2D tetrisSprite;
        #endregion

        #region Tilemaps
        private TmxMap map;
        private Texture2D tileset;
        private TilemapManager tilemapManager;
        #endregion

        #region Player
        private Player player;
        private Texture2D playerSprite;
        private Texture2D runningPlayerSprite;
        private Texture2D fallingPlayerSprite;
        private Texture2D jumpingPlayerSprite;
        private Texture2D spawningPlayerSprite;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1440;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region Tilemap
            map = new TmxMap("Content\\map.tmx");
            tileset = Content.Load<Texture2D>("tileset");
            tilemapManager = new TilemapManager(map, tileset);
            #endregion

            

            #region TetrisManager
            tetrisSprite = Content.Load<Texture2D>("tetrisFigures");
            tetris = new TetrisManager(tetrisSprite, tilemapManager);
            #endregion

            #region Player
            playerSprite = Content.Load<Texture2D>("idleSprite");
            spawningPlayerSprite = Content.Load<Texture2D>("spawningSprite");
            runningPlayerSprite = Content.Load<Texture2D>("runningSpritePlayer");
            fallingPlayerSprite = Content.Load<Texture2D>("fallingSprite");
            jumpingPlayerSprite = Content.Load<Texture2D>("jumpingSprite");
            player = new Player(tilemapManager, tetris, spawningPlayerSprite, runningPlayerSprite, playerSprite, fallingPlayerSprite, jumpingPlayerSprite);
            #endregion


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            tetris.Update(gameTime);
            player.Update();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightCoral);
            spriteBatch.Begin();
            tilemapManager.Draw(spriteBatch);
            tetris.Draw(spriteBatch);
            player.Draw(spriteBatch, gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}