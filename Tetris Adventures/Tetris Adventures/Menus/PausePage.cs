using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tetris_Adventures.Managers;
using Tetris_Adventures.Objects;

namespace Tetris_Adventures.Menus
{
    public class PausePage : MenuManager
    {
        public bool ResetLevelAfterExit;
        private const int delay = 200;
        private readonly Texture2D MenuBackground;
        private readonly Texture2D PauseLogo;
        private readonly Texture2D ContinueSheet;
        private readonly Texture2D HowToPlaySheet;
        private readonly Texture2D PauseExitSheet;
        private MenuOption CurrentOption;
        private double ChangeOptionCheck;
        
        public MenuManager MenuManager { get; set; }

        public PausePage(MenuManager menuManager, Texture2D menuBackground, Texture2D pauseLogo, Texture2D continueSheet, Texture2D howToPlaySheet, Texture2D pauseExitSheet)
        {
            MenuBackground = menuBackground;
            PauseLogo = pauseLogo;
            ContinueSheet = continueSheet;
            HowToPlaySheet = howToPlaySheet;
            PauseExitSheet = pauseExitSheet;
            CurrentOption = new MenuOption();
            MenuManager = menuManager;
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            GetHandleInput(keyboard, gameTime);
        }

        public void GetHandleInput(KeyboardState keyboard, GameTime gameTime)
        {
            if (keyboard.IsKeyDown(Keys.Escape) 
                && gameTime.TotalGameTime.TotalMilliseconds - MenuManager.JumpTimeCheck > delay)
            {
                MenuManager.GameState = GameStates.Game;
                MenuManager.JumpTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (keyboard.IsKeyDown(Keys.Down)
                && gameTime.TotalGameTime.TotalMilliseconds - ChangeOptionCheck > delay)
            {
                CurrentOption.Option = (MenuOptions)((int)(CurrentOption.Option + 1) % 3);
                ChangeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Up)
                && gameTime.TotalGameTime.TotalMilliseconds - ChangeOptionCheck > delay)
            {
                CurrentOption.Option = (MenuOptions)((int)(CurrentOption.Option + 2) % 3);
                ChangeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                MakeResponse(CurrentOption, gameTime);
            }
        }

        public void MakeResponse(MenuOption option, GameTime gameTime)
        {
            switch (option.Option)
            {
                case MenuOptions.Game:
                    MenuManager.GameState = GameStates.Game;
                    MenuManager.JumpTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
                    break;
                case MenuOptions.HowToPlay:
                    MenuManager.LastGameState = GameStates.Pause;
                    MenuManager.GameState = GameStates.HowToPlay;
                    break;
                case MenuOptions.Exit:
                    MenuManager.GameState = GameStates.Menu;
                    MenuManager.JumpTimeCheck = gameTime.TotalGameTime.TotalMilliseconds;
                    CurrentOption.Option = MenuOptions.Game;
                    ResetLevelAfterExit = true;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MenuBackground, new Rectangle(0, 0, 1440, 800), Color.White);
            spriteBatch.Draw(PauseLogo, new Vector2(50, 40), new Rectangle(0, 0, 259, 116), Color.White);
            spriteBatch.Draw(ContinueSheet, new Vector2(50, 250), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOptions.Game ? Color.GhostWhite : Color.SlateGray);
            spriteBatch.Draw(HowToPlaySheet, new Vector2(50, 300), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOptions.HowToPlay ? Color.GhostWhite : Color.SlateGray);
            spriteBatch.Draw(PauseExitSheet, new Vector2(50, 350), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOptions.Exit ? Color.GhostWhite : Color.SlateGray);
        }        
    }
}
