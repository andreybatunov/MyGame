using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tetris_Adventures
{
    public class Animation
    {
        Texture2D spritesheet;
        int frames;
        int rows;
        int pointer = 0;
        int width;
        int height;
        float timeSinceLastFrame;
        public Animation(Texture2D spritesheet, int width, int height)
        {
            this.spritesheet = spritesheet;
            frames = spritesheet.Width / width;
            this.width = width;
            this.height = height;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, int row, float millisecondsPerFrames = 75)
        {
            if (pointer <=  frames)
            {
                var rectangle = new Rectangle(width * pointer, height * (row - 1), width, height);
                spriteBatch.Draw(spritesheet, position, rectangle, Color.White);
                timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeSinceLastFrame > millisecondsPerFrames) 
                {
                    timeSinceLastFrame -= millisecondsPerFrames;
                    pointer++;
                    if (pointer == frames)
                    {
                        pointer = 0;
                    }
                }
            } 
        }
    }
}
