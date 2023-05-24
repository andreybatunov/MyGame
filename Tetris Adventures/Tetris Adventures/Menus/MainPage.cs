using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tetris_Adventures.Managers;
using Tetris_Adventures.Objects;

namespace Tetris_Adventures.Menus
{
    public class MainPage : MenuManager
    { 
        public MenuOption CurrentOption;
        public Texture2D MenuBackground;
        public Texture2D Logo;
        public Texture2D NewGameSheet;
        public Texture2D HowToPlaySheet;
        public Texture2D ExitSheet;
        public double ChangeOptionCheck;
        public MenuManager MenuManager { get; set; }

        public MainPage(MenuManager menuManager, Texture2D menuBackground, Texture2D logo, Texture2D newGameSheet, Texture2D howToPlaySheet, Texture2D exitSheet)
        {
            NewGameSheet = newGameSheet;
            HowToPlaySheet = howToPlaySheet;
            ExitSheet = exitSheet;
            MenuBackground = menuBackground;
            Logo = logo;
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
            if (keyboard.IsKeyDown(Keys.Down) 
                && gameTime.TotalGameTime.TotalMilliseconds - ChangeOptionCheck > 200)
            {
                CurrentOption.Option = (MenuOptions)((int)(CurrentOption.Option + 1) % 3);
                ChangeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Up)
                && gameTime.TotalGameTime.TotalMilliseconds - ChangeOptionCheck > 200)
            {
                CurrentOption.Option = (MenuOptions)((int)(CurrentOption.Option + 2) % 3);
                ChangeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                switch (CurrentOption.Option)
                {
                    case MenuOptions.Exit:
                        MenuManager.GameState = GameStates.Exit;
                        break;
                    case MenuOptions.Game:
                        if (gameTime.TotalGameTime.TotalMilliseconds - MenuManager.JumpTimeCheck > 200)
                        {
                            MenuManager.GameState = GameStates.Game;
                            MenuManager.JumpTimeCheck = 0;
                        }
                        break;
                    case MenuOptions.HowToPlay:
                        MenuManager.LastGameState = GameStates.Menu;
                        MenuManager.GameState = GameStates.HowToPlay;
                        break;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MenuBackground, new Rectangle(0, 0, 1440, 800), Color.White);
            spriteBatch.Draw(Logo, new Vector2(50, 10), new Rectangle(0, 0, 480, 160), Color.White);
            spriteBatch.Draw(NewGameSheet, new Vector2(50, 250), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOptions.Game ? Color.GhostWhite : Color.SlateGray);
            spriteBatch.Draw(HowToPlaySheet, new Vector2(50, 320), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOptions.HowToPlay ? Color.GhostWhite : Color.SlateGray);
            spriteBatch.Draw(ExitSheet, new Vector2(50, 390), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOptions.Exit ? Color.GhostWhite : Color.SlateGray);
        }
    }
}
