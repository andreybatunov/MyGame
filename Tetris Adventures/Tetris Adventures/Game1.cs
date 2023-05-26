using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tetris_Adventures.Managers;
using Tetris_Adventures.Menus;
using Tetris_Adventures.Objects;
using TiledSharp;
using static Tetris_Adventures.Managers.MenuManager;

namespace Tetris_Adventures
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private MenuManager menuManager;
             
        #region TetrisManager
        private TetrisManager tetrisManager;
        private Texture2D tetrisSprite;
        #endregion
        #region Tilemaps
        private TmxMap map1;
        private TmxMap map2;
        private TmxMap map3;
        private TmxMap map4;
        private TmxMap map5;
        private List<TmxMap> maps = new();
        private Texture2D tileset;
        private TilemapManager tilemapManager;
        private Texture2D finishFlag;
        #endregion
        #region Player
        private Player player;
        private Texture2D playerSprite;
        private Texture2D runningPlayerSprite;
        private Texture2D fallingPlayerSprite;
        private Texture2D jumpingPlayerSprite;
        private Texture2D spawningPlayerSprite;
        #endregion

        #region UIManager
        private UIManager uiManager;
        private Texture2D numbersSheet;
        private Texture2D gameOverSheet;
        private Texture2D gameOverReturnSheet;
        private Texture2D bubbleSheet;
        #endregion

        #region Menu
        private MainPage menu;
        private Texture2D menuBackground;
        private Texture2D logo;
        private Texture2D newGameSheet;
        private Texture2D howToPlaySheet;
        private Texture2D exitSheet;
        #endregion
        #region InstructionsPage;
        private Texture2D instructionsSheet;
        private InstructionsPage instructionsPage;
        #endregion
        #region PausePage
        private PausePage pausePage;
        private Texture2D pauseLogo;
        private Texture2D continueSheet;
        private Texture2D pauseHowToPlay;
        private Texture2D pauseExitSheet;
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
            menuManager = new MenuManager();

            #region MenusAssets
            menuBackground = Content.Load<Texture2D>("menuBackground");
            logo = Content.Load<Texture2D>("mainMenuLogo");
            instructionsSheet = Content.Load<Texture2D>("Instructions");
            newGameSheet = Content.Load<Texture2D>("newGame");
            howToPlaySheet = Content.Load<Texture2D>("howToPlay");
            exitSheet = Content.Load<Texture2D>("exit");
            pauseLogo = Content.Load<Texture2D>("pauseLogo");
            continueSheet = Content.Load<Texture2D>("continueSheet");
            pauseHowToPlay = Content.Load<Texture2D>("pauseHowToPlay");
            pauseExitSheet = Content.Load<Texture2D>("pauseExit");

            #endregion

            #region Tilemap
            map1 = new TmxMap("Content\\level1.tmx");
            map2 = new TmxMap("Content\\level2.tmx");
            map3 = new TmxMap("Content\\level3.tmx");

            maps = new List<TmxMap> { map1, map2, map3 };
            tileset = Content.Load<Texture2D>("newTileset");
            finishFlag = Content.Load<Texture2D>("finishFlag");

            tilemapManager = new TilemapManager(maps, tileset, finishFlag);
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

            tetrisManager = new TetrisManager(tetrisSprite, tilemapManager, player);
            #endregion

            #region UIManager
            numbersSheet = Content.Load<Texture2D>("numbersSheet");
            gameOverSheet = Content.Load<Texture2D>("gameOver");
            gameOverReturnSheet = Content.Load<Texture2D>("gameOverReturn");
            bubbleSheet = Content.Load<Texture2D>("bubbleSheet");
            uiManager = new UIManager(menuManager, tilemapManager, tetrisManager, player, numbersSheet, gameOverSheet, gameOverReturnSheet, tetrisSprite, bubbleSheet);
            #endregion

            #region Menu
            menu = new MainPage(menuManager, menuBackground, logo, newGameSheet, howToPlaySheet, exitSheet);
            #endregion

            #region InstructionPage
            instructionsPage = new InstructionsPage(menuManager, menuBackground, logo, instructionsSheet);
            #endregion

            #region PausePage
            pausePage = new PausePage(menuManager, menuBackground, pauseLogo, continueSheet, pauseHowToPlay, pauseExitSheet);
            #endregion


        }

        protected override void Update(GameTime gameTime)
        {
            switch (menuManager.GameState)
            {
                case GameStates.Game:
                    if (!uiManager.GameOverStatus)
                    {
                        tetrisManager.Update(gameTime);
                        player.Update(gameTime);
                    }
                    uiManager.Update(gameTime);
                    break;
                case GameStates.Menu:
                    menu.Update(gameTime);
                    break;
                case GameStates.HowToPlay:
                    instructionsPage.Update(gameTime);
                    break;
                case GameStates.Pause:
                    pausePage.Update(gameTime);
                    break;
                case GameStates.Exit:
                    Exit();
                    break;
            }
            if (uiManager.ResetLevelAfterGameOver || pausePage.ResetLevelAfterExit)
            {
                ResetLevel();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            switch (menuManager.GameState)
            {
                case GameStates.Game:
                    GraphicsDevice.Clear(tilemapManager.Colors[tilemapManager.Level]);
                    tilemapManager.Draw(spriteBatch);
                    if (!uiManager.GameOverStatus)
                    {
                        tetrisManager.Draw(spriteBatch);
                        player.Draw(spriteBatch, gameTime);
                    }
                    uiManager.Draw(spriteBatch);
                    break;
                case GameStates.Menu:
                    menu.Draw(spriteBatch);
                    break;
                case GameStates.HowToPlay:
                    instructionsPage.Draw(spriteBatch);
                    break;
                case GameStates.Pause:
                    pausePage.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void ResetLevel()
        {
            tilemapManager = new TilemapManager(maps, tileset, finishFlag);
            player = new Player(tilemapManager, spawningPlayerSprite, runningPlayerSprite, playerSprite, fallingPlayerSprite, jumpingPlayerSprite);
            tetrisManager = new TetrisManager(tetrisSprite, tilemapManager, player);
            uiManager = new UIManager(menuManager, tilemapManager, tetrisManager, player, numbersSheet, gameOverSheet, gameOverReturnSheet, tetrisSprite, bubbleSheet);
            pausePage.ResetLevelAfterExit = false;
        }
    }
}