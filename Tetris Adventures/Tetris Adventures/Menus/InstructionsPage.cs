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
        public MenuManager MenuManager { get; set; }

        private readonly Texture2D menuBackground;
        private readonly Texture2D logo;
        private readonly Texture2D instructionsSheet;
        
        public InstructionsPage(MenuManager menuManager, Texture2D menuBackground, Texture2D logo, Texture2D instructionsSheet) 
        {
            MenuManager = menuManager;
            this.menuBackground = menuBackground;
            this.logo = logo;
            this.instructionsSheet = instructionsSheet;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, 1440, 800), Color.White);
            spriteBatch.Draw(logo, new Vector2(50, 10), new Rectangle(0, 0, 480, 160), Color.White);
            spriteBatch.Draw(instructionsSheet, new Vector2(50, 200), new Rectangle(0, 0, 708, 417), Color.White);
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
