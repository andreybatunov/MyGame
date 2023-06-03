using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tetris_Adventures.Managers;
using Tetris_Adventures.Objects;

namespace Tetris_Adventures.Menus
{
    public class MainPage : MenuManager
    {
        private const int delay = 200;
        private readonly Texture2D menuBackground;
        private readonly Texture2D logo;
        private readonly Texture2D newGameSheet;
        private readonly Texture2D howToPlaySheet;
        private readonly Texture2D exitSheet;
        private MenuOption currentOption;
        private double changeOptionCheck;
        private MenuManager MenuManager { get; set; }

        public MainPage(MenuManager menuManager, Texture2D menuBackground, Texture2D logo, Texture2D newGameSheet, Texture2D howToPlaySheet, Texture2D exitSheet)
        {
            this.newGameSheet = newGameSheet;
            this.howToPlaySheet = howToPlaySheet;
            this.exitSheet = exitSheet;
            this.menuBackground = menuBackground;
            this.logo = logo;
            currentOption = new MenuOption();
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
                && gameTime.TotalGameTime.TotalMilliseconds - changeOptionCheck > delay)
            {
                currentOption.Option = (MenuOptions)((int)(currentOption.Option + 1) % 3);
                changeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Up)
                && gameTime.TotalGameTime.TotalMilliseconds - changeOptionCheck > delay)
            {
                currentOption.Option = (MenuOptions)((int)(currentOption.Option + 2) % 3);
                changeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                switch (currentOption.Option)
                {
                    case MenuOptions.Exit:
                        MenuManager.GameState = GameStates.Exit;
                        break;
                    case MenuOptions.Game:
                        if (gameTime.TotalGameTime.TotalMilliseconds - MenuManager.JumpTimeCheck > delay)
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
            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, 1440, 800), Color.White);
            spriteBatch.Draw(logo, new Vector2(50, 10), new Rectangle(0, 0, 480, 160), Color.White);
            spriteBatch.Draw(newGameSheet, new Vector2(50, 250), new Rectangle(0, 0, 304, 77), currentOption.Option == MenuOptions.Game ? Color.GhostWhite : Color.SlateGray);
            spriteBatch.Draw(howToPlaySheet, new Vector2(50, 320), new Rectangle(0, 0, 304, 77), currentOption.Option == MenuOptions.HowToPlay ? Color.GhostWhite : Color.SlateGray);
            spriteBatch.Draw(exitSheet, new Vector2(50, 390), new Rectangle(0, 0, 304, 77), currentOption.Option == MenuOptions.Exit ? Color.GhostWhite : Color.SlateGray);
        }
    }
}
