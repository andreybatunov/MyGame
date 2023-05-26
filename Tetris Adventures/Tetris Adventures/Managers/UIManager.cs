using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Security.Policy;
using Tetris_Adventures.Objects;

namespace Tetris_Adventures.Managers
{
    public class UIManager
    {
        public MenuManager MenuManager { get; set; }
        public int Timer;
        public int FirstPos;
        public int SecondPos;
        public int ThirdPos;
        public List<Rectangle> NumberRectangles;
        public Texture2D NumbersSheet;
        public Texture2D GameOverSheet;
        public Texture2D GameOverReturnSheet;
        public Texture2D TetrisFiguresSheet;
        public Texture2D BubbleSheet;
        public Texture2D MissionCompletedSheet;
        public Texture2D MissionCompletedReturnSheet;
        public Texture2D LevelTitle;
        public Texture2D LevelNumbers;
        public TetrisManager TetrisManager;
        public TilemapManager TilemapManager;
        public Player Player;
        public bool GameOverStatus;
        public bool GameCompleted;
        public bool ResetLevelAfterGameOver = false;
        public bool CurrentLevelComplited;
        public int TetrisPointer = 1;

        public UIManager(MenuManager menuManager, TilemapManager tilemapManager, TetrisManager tetrisManager, Player player, 
            Texture2D numbersSheet, Texture2D gameOverSheet, Texture2D gameOverReturnSheet, Texture2D tetrisFigures, 
            Texture2D bubble, Texture2D missionCompleted, Texture2D missionComlReturn, Texture2D level, Texture2D levelNumbers) 
        {
            MenuManager = menuManager;
            TilemapManager = tilemapManager;
            TetrisManager = tetrisManager;
            Player = player;
            GameOverStatus = false;
            GameCompleted = false;
            Timer = 3600;
            NumbersSheet = numbersSheet;
            GameOverSheet = gameOverSheet;
            GameOverReturnSheet = gameOverReturnSheet;
            TetrisFiguresSheet = tetrisFigures;
            BubbleSheet = bubble;
            MissionCompletedSheet = missionCompleted;
            MissionCompletedReturnSheet = missionComlReturn;
            LevelTitle = level;
            LevelNumbers = levelNumbers;
        }

        public void Update(GameTime gameTime)
        {
            if (!GameOverStatus && !GameCompleted) 
            {
                TimerCountDown();
            }
            var keyboard = Keyboard.GetState();
            GetHandleInput(keyboard, gameTime);
            if (Timer == 0 || !Player.IsAlive)
            {
                GameOverStatus = true;
            }
             if (Player.IsReachedFinish)
            {
                if (TilemapManager.Level == 4)
                {
                    GameCompleted = true;
                }
                else
                {
                    GoToNextLevel();
                }
            }
        }

        public void GetHandleInput(KeyboardState keyboard, GameTime gameTime)
        {
            if (keyboard.IsKeyDown(Keys.Escape)
                && gameTime.TotalGameTime.TotalMilliseconds - MenuManager.JumpTimeCheck > 200)
            {
                MenuManager.GameState = MenuManager.GameStates.Pause;
                MenuManager.JumpTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Enter) && (GameOverStatus || GameCompleted)
                && gameTime.TotalGameTime.TotalMilliseconds - MenuManager.JumpTimeCheck > 200) 
            {
                //GameOverStatus = false;
                ResetLevelAfterGameOver = true;
                MenuManager.JumpTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
                MenuManager.GameState = MenuManager.GameStates.Menu;
            }
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            if (GameOverStatus)
            {
                DrawGameOverTitle(spriteBatch);
            }
            else if (GameCompleted)
            {
                DrawGameCompletedTitle(spriteBatch);
            }
            else
            {
                DrawLevelTitle(spriteBatch);
                DrawCountDown(spriteBatch);
                DrawInventory(spriteBatch);
            }
        }

        public void TimerCountDown()
        {
            if (Timer % 60 == 0)
            {
                ThirdPos = Timer / 60 % 10;
                SecondPos = Timer / 60 / 10 % 6;
                FirstPos = Timer / 3600;
            }
            if (Timer != 0)
            {
                Timer--;
            }
        }

        public void DrawCountDown(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(NumbersSheet, new Vector2(1405, 20), new Rectangle(29 * ThirdPos, 0, 29, 38), Timer <= 900 ? Color.Red : Color.White);
            spriteBatch.Draw(NumbersSheet, new Vector2(1374, 20), new Rectangle(29 * SecondPos, 0, 29, 38), Timer <= 900 ? Color.Red : Color.White);
            spriteBatch.Draw(NumbersSheet, new Vector2(1361, 30), new Rectangle(290, 0, 9, 38), Timer <= 900 ? Color.Red : Color.White);
            spriteBatch.Draw(NumbersSheet, new Vector2(1334, 20), new Rectangle(29 * FirstPos, 0, 29, 38), Timer <= 900 ? Color.Red : Color.White);
        }

        public void DrawGameOverTitle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(GameOverSheet, new Vector2(370, 320), new Rectangle(0, 0, 700, 83), Color.White);
            spriteBatch.Draw(GameOverReturnSheet, new Vector2(507, 410), new Rectangle(0, 0, 427, 27), Color.White);
        }

        public void DrawGameCompletedTitle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MissionCompletedSheet, new Vector2(480, 290), new Rectangle(0, 0, 480, 129), Color.White);
            spriteBatch.Draw(MissionCompletedReturnSheet, new Vector2(507, 450), new Rectangle(0, 0, 427, 27), Color.White);
        }

        public void DrawLevelTitle(SpriteBatch spriteBatch)
        {
            if (Timer > 3420)
            {
                spriteBatch.Draw(LevelTitle, new Vector2(520, 320), Color.White);
                spriteBatch.Draw(LevelNumbers, new Vector2(850, 332), new Rectangle((TilemapManager.Level) * 75, 0, 75, 101), Color.White);
            }
        }

        public void DrawInventory(SpriteBatch spriteBatch)
        {
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.IShape, new Vector2(60, 60), 1);
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.JShape, new Vector2(160, 60), 2);
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.LShape, new Vector2(260, 60), 3);
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.OShape, new Vector2(360, 60), 4);
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.ZShape, new Vector2(460, 60), 5);
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.TShape, new Vector2(560, 60), 6);
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.SShape, new Vector2(660, 60), 7);
        }

        public void DrawInventoryBubble(SpriteBatch spriteBatch,TetrisManager.TetrisFigure tetrisFigure, Vector2 position, int number)
        {
            var figureOrigin = new Vector2(TetrisManager.Shapes[tetrisFigure].Width / 2, TetrisManager.Shapes[tetrisFigure].Height / 2);
            var numberPosition = new Vector2(position.X - 35, position.Y + 55);
            spriteBatch.Draw(BubbleSheet, position, new Rectangle(0, 0, 100, 100), Color.White, 0f, new Vector2(50, 50),
                TetrisManager.CurrentTetrisObject.Figure == tetrisFigure ? 1.5f : 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(TetrisManager.TetrisSpriteSheet, position, TetrisManager.Shapes[tetrisFigure],
                TetrisManager.DelaysDictionary[tetrisFigure].Item2 ? Color.GhostWhite : Color.Black, 0f, figureOrigin,
                 TetrisManager.CurrentTetrisObject.Figure == tetrisFigure ? 1f : 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(NumbersSheet, numberPosition, new Rectangle(29 * number, 0, 29, 38), Color.White, 0f, new Vector2(15, 19), 0.5f, SpriteEffects.None, 0f);
        }

        public void GoToNextLevel()
        {
            TilemapManager.Level += 1;
            TilemapManager.GetCollisionsObjects();
            TetrisManager.DrawnFigures.Clear();
            TetrisManager.EnvironmentTetrisSquares.Clear();
            TetrisManager.ResetDelays();
            Player.Velocity = TilemapManager.StartPosition;
            Player.Position = TilemapManager.StartPosition;
            Player.IsReachedFinish = false;
            Timer = 3600;
            
        }
    }
}
