using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Tetris_Adventures.Objects;

namespace Tetris_Adventures.Managers
{
    public class UIManager
    {
        private const int delay = 200;

        public bool GameOverStatus { get; set; }
        public bool GameCompleted { get; set; }
        public bool ResetLevelAfterGameOver = false;
        public bool CurrentLevelCompleted;
        public int TetrisPointer = 1;

        private readonly int levelTime;
        private readonly Texture2D numbersSheet;
        private readonly Texture2D gameOverSheet;
        private readonly Texture2D gameOverReturnSheet;
        private readonly Texture2D bubbleSheet;
        private readonly Texture2D missionCompletedSheet;
        private readonly Texture2D missionCompletedReturnSheet;
        private readonly Texture2D levelTitle;
        private readonly Texture2D levelNumbers;

        private int timer;
        private int firstPos;
        private int secondPos;
        private int thirdPos;
        private TetrisManager tetrisManager;
        private TilemapManager tilemapManager;
        private Player player;
        private MenuManager menuManager;
        
        public UIManager(MenuManager menuManager, TilemapManager tilemapManager, TetrisManager tetrisManager, Player player, 
            Texture2D numbersSheet, Texture2D gameOverSheet, Texture2D gameOverReturnSheet, 
            Texture2D bubble, Texture2D missionCompleted, Texture2D missionComlReturn, Texture2D level, Texture2D levelNumbers) 
        {
            this.menuManager = menuManager;
            this.tilemapManager = tilemapManager;
            this.tetrisManager = tetrisManager;
            this.player = player;
            GameOverStatus = false;
            GameCompleted = false;
            levelTime = 3600;
            timer = levelTime;
            this.numbersSheet = numbersSheet;
            this.gameOverSheet = gameOverSheet;
            this.gameOverReturnSheet = gameOverReturnSheet;
            bubbleSheet = bubble;
            missionCompletedSheet = missionCompleted;
            missionCompletedReturnSheet = missionComlReturn;
            levelTitle = level;
            this.levelNumbers = levelNumbers;
        }

        public void Update(GameTime gameTime)
        {
            if (!GameOverStatus && !GameCompleted) 
            {
                TimerCountDown();
            }
            var keyboard = Keyboard.GetState();
            GetHandleInput(keyboard, gameTime);
            if (timer == 0 || !player.IsAlive)
            {
                GameOverStatus = true;
            }
             if (player.IsReachedFinish)
            {
                if (tilemapManager.Level == 4)
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
                && gameTime.TotalGameTime.TotalMilliseconds - menuManager.JumpTimeCheck > delay)
            {
                menuManager.GameState = MenuManager.GameStates.Pause;
                menuManager.JumpTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Enter) && (GameOverStatus || GameCompleted)
                && gameTime.TotalGameTime.TotalMilliseconds - menuManager.JumpTimeCheck > delay) 
            {
                ResetLevelAfterGameOver = true;
                menuManager.JumpTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
                menuManager.GameState = MenuManager.GameStates.Menu;
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
            if (timer % 60 == 0)
            {
                thirdPos = timer / 60 % 10;
                secondPos = timer / 60 / 10 % 6;
                firstPos = timer / 3600;
            }
            if (timer != 0)
            {
                timer--;
            }
        }

        public void DrawCountDown(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(numbersSheet, new Vector2(1405, 20), new Rectangle(29 * thirdPos, 0, 29, 38), timer <= 900 ? Color.Red : Color.White);
            spriteBatch.Draw(numbersSheet, new Vector2(1374, 20), new Rectangle(29 * secondPos, 0, 29, 38), timer <= 900 ? Color.Red : Color.White);
            spriteBatch.Draw(numbersSheet, new Vector2(1361, 30), new Rectangle(290, 0, 9, 38), timer <= 900 ? Color.Red : Color.White);
            spriteBatch.Draw(numbersSheet, new Vector2(1334, 20), new Rectangle(29 * firstPos, 0, 29, 38), timer <= 900 ? Color.Red : Color.White);
        }

        public void DrawGameOverTitle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(gameOverSheet, new Vector2(370, 320), new Rectangle(0, 0, 700, 83), Color.White);
            spriteBatch.Draw(gameOverReturnSheet, new Vector2(507, 410), new Rectangle(0, 0, 427, 27), Color.White);
        }

        public void DrawGameCompletedTitle(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(missionCompletedSheet, new Vector2(480, 290), new Rectangle(0, 0, 480, 129), Color.White);
            spriteBatch.Draw(missionCompletedReturnSheet, new Vector2(507, 450), new Rectangle(0, 0, 427, 27), Color.White);
        }

        public void DrawLevelTitle(SpriteBatch spriteBatch)
        {
            if (timer > levelTime * 0.95)
            {
                spriteBatch.Draw(levelTitle, new Vector2(520, 320), Color.White);
                spriteBatch.Draw(levelNumbers, new Vector2(850, 332), new Rectangle((tilemapManager.Level) * 75, 0, 75, 101), Color.White);
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
            var figureOrigin = new Vector2(tetrisManager.Shapes[tetrisFigure].Width / 2, tetrisManager.Shapes[tetrisFigure].Height / 2);
            var numberPosition = new Vector2(position.X - 35, position.Y + 55);
            spriteBatch.Draw(bubbleSheet, position, new Rectangle(0, 0, 100, 100), Color.White, 0f, new Vector2(50, 50),
                tetrisManager.CurrentTetrisObject.Figure == tetrisFigure ? 1.5f : 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(tetrisManager.TetrisSpriteSheet, position, tetrisManager.Shapes[tetrisFigure],
                tetrisManager.DelaysDictionary[tetrisFigure].Item2 ? Color.GhostWhite : Color.Black, 0f, figureOrigin,
                 tetrisManager.CurrentTetrisObject.Figure == tetrisFigure ? 1f : 0.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(numbersSheet, numberPosition, new Rectangle(29 * number, 0, 29, 38), Color.White, 0f, new Vector2(15, 19), 0.5f, SpriteEffects.None, 0f);
        }

        public void GoToNextLevel()
        {
            tetrisManager.ClearMap();
            tilemapManager.Level += 1;
            tilemapManager.GetCollisionsObjects();
            player.Velocity = tilemapManager.StartPosition;
            player.Position = tilemapManager.StartPosition;
            player.IsReachedFinish = false;
            timer = 3600;
        }
    }
}
