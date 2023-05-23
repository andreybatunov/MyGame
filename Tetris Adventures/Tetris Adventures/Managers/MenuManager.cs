using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Policy;
using Tetris_Adventures.Objects;

namespace Tetris_Adventures.Managers
{
    public class MenuManager
    {
        public enum GameState
        {
            Menu,
            HowToPlay,
            Game,
            Pause,
            Exit
        }

        public GameState State { get; set; }
        public MenuOption CurrentOption;
        public Texture2D MenuBackground;
        public Texture2D Logo;
        public Texture2D NewGameSheet;
        public Texture2D HowToPlaySheet;
        public Texture2D ExitSheet;
        public Texture2D InstructionsSheet;
        public double ChangeOptionCheck;


        public MenuManager(Texture2D menuBackground, Texture2D logo, Texture2D newGameSheet, Texture2D howToPlaySheet, Texture2D exitSheet, Texture2D instructionsSheet)
        {
            NewGameSheet = newGameSheet;
            HowToPlaySheet = howToPlaySheet;
            ExitSheet = exitSheet;
            MenuBackground = menuBackground;
            Logo = logo;
            CurrentOption = new MenuOption();
            InstructionsSheet = instructionsSheet;
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            GetHandleInput(keyboard, gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MenuBackground, new Rectangle(0, 0, 1440, 800), Color.White);
            spriteBatch.Draw(Logo, new Vector2(50,10), new Rectangle(0, 0, 480, 160), Color.White);
            if (State == GameState.Menu)
            {
                spriteBatch.Draw(NewGameSheet, new Vector2(50, 250), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOption.MainMenuOptions.NewGame ? Color.GhostWhite : Color.SlateGray);
                spriteBatch.Draw(HowToPlaySheet, new Vector2(50, 320), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOption.MainMenuOptions.HowToPlay ? Color.GhostWhite : Color.SlateGray);
                spriteBatch.Draw(ExitSheet, new Vector2(50, 390), new Rectangle(0, 0, 304, 77), CurrentOption.Option == MenuOption.MainMenuOptions.Exit ? Color.GhostWhite : Color.SlateGray);
            }
            if (State == GameState.HowToPlay)
            {
                spriteBatch.Draw(InstructionsSheet, new Vector2(50, 200), new Rectangle(0, 0, 708, 417), Color.White);
            }
        }

        public void GetHandleInput(KeyboardState keyboard, GameTime gameTime)
        {
            if (keyboard.IsKeyDown(Keys.Down) && gameTime.TotalGameTime.TotalMilliseconds - ChangeOptionCheck > 200)
            {
                CurrentOption.Option = (MenuOption.MainMenuOptions)((int)(CurrentOption.Option + 1) % 3);
                ChangeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Up) && gameTime.TotalGameTime.TotalMilliseconds - ChangeOptionCheck > 200)
            {
                CurrentOption.Option = (MenuOption.MainMenuOptions)((int)(CurrentOption.Option + 2) % 3);
                ChangeOptionCheck = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (keyboard.IsKeyDown(Keys.Back))
            {
                State = GameState.Menu;
            }
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                switch (CurrentOption.Option)
                {
                    case MenuOption.MainMenuOptions.Exit:
                        State = GameState.Exit;
                        break;
                    case MenuOption.MainMenuOptions.NewGame:
                        State = GameState.Game;
                        break;
                    case MenuOption.MainMenuOptions.HowToPlay:
                        State = GameState.HowToPlay;
                        break;
                }
            }
        }
    }
}
