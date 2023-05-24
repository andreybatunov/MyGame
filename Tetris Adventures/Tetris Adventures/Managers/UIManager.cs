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
        public TetrisManager TetrisManager;
        public Player Player;
        public bool GameOverStatus;
        public bool ResetLevelAfterGameOver = false;

        public UIManager(MenuManager menuManager, TetrisManager tetrisManager, Player player, Texture2D numbersSheet, Texture2D gameOverSheet, Texture2D gameOverReturnSheet) 
        {
            MenuManager = menuManager;
            Timer = 3600;
            NumbersSheet = numbersSheet;
            TetrisManager = tetrisManager;
            Player = player;
            GameOverStatus = false;
            GameOverSheet = gameOverSheet;
            GameOverReturnSheet = gameOverReturnSheet;
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
                spriteBatch.Draw(GameOverSheet, new Vector2(370, 320), new Rectangle(0, 0, 700, 83), Color.White);
                spriteBatch.Draw(GameOverReturnSheet, new Vector2(507, 410), new Rectangle(0, 0, 427, 27), Color.White);
            }
            else
            {
                spriteBatch.Draw(NumbersSheet, new Vector2(741, 20), new Rectangle(29 * ThirdPos, 0, 29, 38), Timer <= 900 ? Color.Red : Color.White);
                spriteBatch.Draw(NumbersSheet, new Vector2(710, 20), new Rectangle(29 * SecondPos, 0, 29, 38), Timer <= 900 ? Color.Red : Color.White);
                spriteBatch.Draw(NumbersSheet, new Vector2(697, 30), new Rectangle(290, 0, 9, 38), Timer <= 900 ? Color.Red : Color.White);
                spriteBatch.Draw(NumbersSheet, new Vector2(670, 20), new Rectangle(29 * FirstPos, 0, 29, 38), Timer <= 900 ? Color.Red : Color.White);
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
    }
}
