using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
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
        public TetrisManager TetrisManager;
        public Player Player;
        public bool GameOverStatus;
        public bool ResetLevelAfterGameOver = false;
        public int TetrisPointer = 1;

        public UIManager(MenuManager menuManager, TetrisManager tetrisManager, Player player, Texture2D numbersSheet, Texture2D gameOverSheet, Texture2D gameOverReturnSheet, Texture2D tetrisFigures, Texture2D bubble) 
        {
            MenuManager = menuManager;
            Timer = 3600;
            NumbersSheet = numbersSheet;
            TetrisManager = tetrisManager;
            Player = player;
            GameOverStatus = false;
            GameOverSheet = gameOverSheet;
            GameOverReturnSheet = gameOverReturnSheet;
            TetrisFiguresSheet = tetrisFigures;
            BubbleSheet = bubble;
        }

        public void Update(GameTime gameTime)
        {
            if (!GameOverStatus) 
            {
                TimerCountDown();
            }
            var keyboard = Keyboard.GetState();
            GetHandleInput(keyboard, gameTime);
            if (Timer == 0 || !Player.IsAlive)
            {
                GameOverStatus = true;
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
            if (keyboard.IsKeyDown(Keys.Enter) && GameOverStatus
                && gameTime.TotalGameTime.TotalMilliseconds - MenuManager.JumpTimeCheck > 200) 
            {
                GameOverStatus = false;
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
            else
            {
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

        public void DrawInventory(SpriteBatch spriteBatch)
        {
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.IShape, new Vector2(60, 60));
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.JShape, new Vector2(160, 60));
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.LShape, new Vector2(260, 60));
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.OShape, new Vector2(360, 60));
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.ZShape, new Vector2(460, 60));
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.TShape, new Vector2(560, 60));
            DrawInventoryBubble(spriteBatch, TetrisManager.TetrisFigure.SShape, new Vector2(660, 60));
        }

        public void DrawInventoryBubble(SpriteBatch spriteBatch,TetrisManager.TetrisFigure tetrisFigure, Vector2 position)
        {
            var figureOrigin = new Vector2(TetrisManager.Shapes[tetrisFigure].Width / 2, TetrisManager.Shapes[tetrisFigure].Height / 2);
            spriteBatch.Draw(BubbleSheet, position, new Rectangle(0, 0, 100, 100), Color.White, 0f, new Vector2(50, 50),
                TetrisManager.CurrentTetrisObject.Figure == tetrisFigure ? 1.5f : 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(TetrisManager.TetrisSpriteSheet, position, TetrisManager.Shapes[tetrisFigure],
                TetrisManager.DelaysDictionary[tetrisFigure].Item2 ? Color.GhostWhite : Color.Black, 0f, figureOrigin,
                 TetrisManager.CurrentTetrisObject.Figure == tetrisFigure ? 1f : 0.5f, SpriteEffects.None, 0f);
        }
    }
}
