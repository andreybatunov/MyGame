using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.VectorDraw;
using Tetris_Adventures.Managers;
using Tetris_Adventures.Objects;
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
        #region Menu
        private MenuManager menuManager;
        private Texture2D menuBackground;
        private Texture2D logo;
        private Texture2D newGameSheet;
        private Texture2D howToPlaySheet;
        private Texture2D exitSheet;
        private Texture2D instructionsSheet;
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

            #region Player
            playerSprite = Content.Load<Texture2D>("idleSprite");
            spawningPlayerSprite = Content.Load<Texture2D>("spawningSprite");
            runningPlayerSprite = Content.Load<Texture2D>("runningSpritePlayer");
            fallingPlayerSprite = Content.Load<Texture2D>("fallingSprite");
            jumpingPlayerSprite = Content.Load<Texture2D>("jumpingSprite");
            player = new Player(tilemapManager, spawningPlayerSprite, runningPlayerSprite, playerSprite, fallingPlayerSprite, jumpingPlayerSprite);
            #endregion

            #region TetrisManager
            tetrisSprite = Content.Load<Texture2D>("tetrisFigures");
            tetris = new TetrisManager(tetrisSprite, tilemapManager, player);
            #endregion

            #region MenuManager
            menuBackground = Content.Load<Texture2D>("menuBackground");
            logo = Content.Load<Texture2D>("mainMenuLogo");
            newGameSheet = Content.Load<Texture2D>("newGame");
            howToPlaySheet = Content.Load<Texture2D>("howToPlay");
            exitSheet = Content.Load<Texture2D>("exit");
            instructionsSheet = Content.Load<Texture2D>("Instructions");

            menuManager = new MenuManager(menuBackground, logo, newGameSheet, howToPlaySheet, exitSheet, instructionsSheet);
            #endregion

        }

        protected override void Update(GameTime gameTime)
        {
            switch (menuManager.State)
            {
                case MenuManager.GameState.Game:
                    tetris.Update(gameTime);
                    player.Update(gameTime);
                    break;
                case MenuManager.GameState.Menu:
                    menuManager.Update(gameTime);
                    break;
                case MenuManager.GameState.HowToPlay:
                    menuManager.Update(gameTime);
                    break;
                case MenuManager.GameState.Exit:
                    Exit();
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            switch (menuManager.State)
            {
                case MenuManager.GameState.Game:
                    tilemapManager.Draw(spriteBatch);
                    tetris.Draw(spriteBatch);
                    player.Draw(spriteBatch, gameTime);
                    break;
                case MenuManager.GameState.Menu:
                    menuManager.Draw(spriteBatch);
                    break;
                case MenuManager.GameState.HowToPlay:
                    menuManager.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}