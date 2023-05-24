using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tetris_Adventures.Managers;
using static Tetris_Adventures.Managers.MenuManager;

namespace Tetris_Adventures.Menus
{
    public class InstructionsPage
    {
        public Texture2D MenuBackground;
        public Texture2D Logo;
        public Texture2D InstructionsSheet;
        public MenuManager MenuManager { get; set; }

        public InstructionsPage(MenuManager menuManager, Texture2D menuBackground, Texture2D logo, Texture2D instructionsSheet) 
        {
            MenuManager = menuManager;
            MenuBackground = menuBackground;
            Logo = logo;
            InstructionsSheet = instructionsSheet;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(MenuBackground, new Rectangle(0, 0, 1440, 800), Color.White);
            spriteBatch.Draw(Logo, new Vector2(50, 10), new Rectangle(0, 0, 480, 160), Color.White);
            spriteBatch.Draw(InstructionsSheet, new Vector2(50, 200), new Rectangle(0, 0, 708, 417), Color.White);
        }

        public void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Back))
            {
                MenuManager.GameState = MenuManager.LastGameState;
            }
        }
    }
}
