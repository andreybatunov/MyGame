﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Tetris_Adventures.Images
{
    public class Animation
    {
        Texture2D spritesheet;
        int frames;
        int pointer = 0;
        int width;
        int height;
        float timeSinceLastFrame;
        bool IsLooping;
        public Animation(Texture2D spritesheet, int width, int height, bool isLooping)
        {
            this.spritesheet = spritesheet;
            frames = spritesheet.Width / width;
            this.width = width;
            this.height = height;
            IsLooping = isLooping;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, int row, float millisecondsPerFrames)
        {
            if (pointer <= frames)
            {
                var rectangle = new Rectangle(width * pointer, height * (row - 1), width, height);
                spriteBatch.Draw(spritesheet, position, rectangle, Color.White);
                timeSinceLastFrame += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (timeSinceLastFrame > millisecondsPerFrames)
                {
                    timeSinceLastFrame -= millisecondsPerFrames;
                    if (pointer != frames && IsLooping)
                    {
                        pointer++;
                    }

                    if (pointer == frames)
                    {
                        pointer = 0;
                    }
                }
            }
        }
    }
}