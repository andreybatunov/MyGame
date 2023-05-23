using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_Adventures.Objects
{
    public class MenuOption
    {
        public enum MainMenuOptions
        {
            NewGame = 0,
            HowToPlay = 1,
            Exit = 2,
        }
        public Texture2D OptionSpriteSheet;
        public MainMenuOptions Option;
        public Vector2 Position;
    }
}
